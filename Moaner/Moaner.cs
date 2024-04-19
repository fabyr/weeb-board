using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using WaveLib;

namespace Moaner
{
    public class Moaner : IDisposable
    {
        public EventHandler Triggered; // Unused event
        private LLKH _llkh;

        private WaveOutPlayer _out;
        public static readonly WaveFormat fmt = new WaveFormat(48000, 16, 1); // This format is kept fixed, the sample moan-sound files

        private List<MoanSound> _activeSounds = new List<MoanSound>();
        private ConcurrentQueue<MoanSound> _waiting = new ConcurrentQueue<MoanSound>();

        public bool Active { get; set; } = false;
        private bool _isDisposed = false;

        private bool _inProc = false;

        public Moaner()
        {
            // -1 for device = Default Windows Output Device
            _out = new WaveOutPlayer(-1, fmt, 16384, 1, Filler);

            _llkh = new LLKH();
            _llkh.OnKey += (s, e) => { OnTrigger(); }; // Don't check "e" for any keycode, just trigger no matter what key has been pressed

            ThreadLoop();
        }

        private void ThreadLoop()
        {
            // This continously checks if new sounds are to be added (cannot be added directly because of multithreaded code (Filler))
            new Thread(() =>
            {
                MoanSound ms;
                while (!_isDisposed) // Continous loop until this Moaner-Object gets disposed
                {
                    if (!_inProc && _waiting.TryDequeue(out ms))
                    {
                        _activeSounds.Add(ms);
                    }
                    Thread.Sleep(10); // To lighten CPU-Usage
                }
            }).Start();
        }


        // Standard Audio-Mix Procedure found in http://www.vttoth.com/CMS/index.php/technical-notes/68
        /*
         * (Z = Mixed Sample)
         * 
         * Z = 2AB                    if A < 0.5 and B < 0.5,
         * Z = 2(A + B) - 2AB - 1     otherwise
         */
        private static float Mix(float a, float b)
        {
            if (a < 0.5f && b < 0.5f)
                return 2 * a * b;
            else
                return 2 * (a + b) - 2 * a * b - 1;
        }

        // Called by Audio Player to retrieve the audio data while playing
        private void Filler(IntPtr data, int size)
        {
            byte[] @out = new byte[size];
            int samplesToGet = size / 2; // 2-Bytes are always 1 sample (16-Bit PCM)
            float[] outSamples = new float[samplesToGet];
            for (int i = 0; i < samplesToGet; i++)
                outSamples[i] = 0.5f; // This would mean zero in 16-Bit Short sample

            if (Active && _activeSounds.Count != 0) // Only if activated or some sounds are playing
            {
                _inProc = true; // This flag indicated whether or not we are currently looping through the sounds (important, because if we try to add sound during this time, we crash)
                foreach (MoanSound ms in _activeSounds) // Run through all sounds and mix them
                {
                    if (ms.Active) // Double-Check if it really is still active
                    {
                        short[] s = ms.Next(samplesToGet);
                        for (int i = 0; i < samplesToGet; i++)
                            outSamples[i] = Mix(outSamples[i], (s[i] / (float)short.MaxValue + 1) * 0.5f); // Mix the sounds, and also convert the short-Sample into a sample ranging from 0 to 1
                    }
                }
                _activeSounds.RemoveAll(_ => !_.Active); // Remove all the sounds which ended (not active anymore)
                _inProc = false;
            }

            // Convert the Samples back to bytes (2 bytes per sample)
            for (int i = 0; i < samplesToGet; i++)
            {
                short sample = (short)((outSamples[i] * 2 - 1) * short.MaxValue);

                // Convert sample into the two bytes
                @out[i * 2] = (byte)(sample & 0xFF);
                @out[i * 2 + 1] = (byte)((sample >> 8) & 0xFF);
            }

            Marshal.Copy(@out, 0, data, size); // Copy the byte-audio-buffer into the required destination (Managed behind the scenes by WaveLib & Windows)
        }

        protected void OnTrigger()
        {
            Triggered?.Invoke(this, EventArgs.Empty); // This event is never used but why not just leave it ;)
            if (Active) // Only add if currently active
                MakeSound();
        }

        // Adds a new Random Moaning sound
        public void MakeSound()
        {
            if (_inProc)
                _waiting.Enqueue(new MoanSound());
            else // Only add directly if not in Audio-Procedure
                _activeSounds.Add(new MoanSound());
        }

        public void Dispose()
        {
            _llkh.Dispose(); // Unhook keyboard listener
            _out.Dispose(); // End audio playback
            _isDisposed = true; // Set flag
        }
    }
}
