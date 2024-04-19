using System;
using System.IO;
using WaveLib;

namespace Moaner
{
    public class MoanSound
    {
        private static Random rnd = new Random();

        // All of the samples extracted from https://www.youtube.com/watch?v=IQk4OC0no5Y
        private static readonly UnmanagedMemoryStream[] samplesStreams = new UnmanagedMemoryStream[] // List of all the moan-sound-samples
        {
            Properties.Resources.sample_01,
            Properties.Resources.sample_02,
            Properties.Resources.sample_03,
            Properties.Resources.sample_04,
            Properties.Resources.sample_05,
            Properties.Resources.sample_06
        };

        private static byte[][] samples; // Will contain the audio data directly in RAM, array of byte arrays, each byte array inside the array is one sound

        // Load all the sounds from Resources-Streams into byte arrays in memory for fast access
        public static void LoadSounds()
        {
            samples = new byte[samplesStreams.Length][];
            byte[] buffer = new byte[1024];
            for (int i = 0; i < samples.Length; i++)
            {
                using(WaveStream ws = new WaveStream(samplesStreams[i]))
                {
                    samples[i] = new byte[ws.Length];
                    int r;
                    int pos = 0;
                    while ((r = ws.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        Array.Copy(buffer, 0, samples[i], pos, r);
                        pos += r;
                    }
                }
            }
        }

        public MoanSound() : this(rnd.Next(0, samples.Length)) // Select random sound
        {

        }

        private int _idx;
        private int _samplePos = 0;

        public bool Active { get; private set; } = true; // Used as a flag to see if sound is over
        
        public MoanSound(int soundIdx)
        {
            _idx = soundIdx;
        }

        // Gets the amount of samples (sampleCount) and increases the sound position so the next Call of this method proceeds there
        public short[] Next(int sampleCount)
        {
            /* What is done here?
             * The raw PCM Wave bytes get convesterted to 16-Bit audio samples (2 Bytes are always one sample)
             * NOTE: Relies on the fact that each sample is precisely 48KHz, 16Bit, 1 Channel
             */

            _samplePos += sampleCount * 2;
            
            short[] @out = new short[sampleCount];
            int cPos = _samplePos;
            
            if (_samplePos >= samples[_idx].Length) // If we reached end
                Active = false;

            for (int i = 0; i < sampleCount; i++)
            {
                if (i * 2 + cPos >= samples[_idx].Length) // If the array is not long enough, use zero instead
                {
                    @out[i] = 0;
                } 
                else
                {
                    // Convert the two bytes into 1 single sample ranging from -32768 to 32767 (short-type)
                    @out[i] = (short)(samples[_idx][i * 2 + cPos] | (samples[_idx][i * 2 + 1 + cPos] << 8));
                }
            }
            
            return @out;
        }
    }
}
