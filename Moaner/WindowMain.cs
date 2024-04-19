using System;
using System.Windows.Forms;

namespace Moaner
{
    public partial class WindowMain : Form
    {
        private Moaner _m;

        public WindowMain()
        {
            InitializeComponent();

            // Initialize
            MoanSound.LoadSounds();
            _m = new Moaner();
            _m.Active = false;

            // Position the button exactly at center
            BToggle.Location = new System.Drawing.Point(this.ClientRectangle.Width / 2 - BToggle.Width / 2, this.ClientRectangle.Height / 2 - BToggle.Height / 2);
        }

        private void BToggle_Click(object sender, EventArgs e)
        {
            _m.Active = !_m.Active; // Invert "Active"
            BToggle.Text = _m.Active ? "Deactivate" : "Activate"; // If active, display "Deactivate", otherwise display "Activate"
        }

        private void WindowMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _m.Dispose(); // Dispose of everything (free all the memory)
        }
    }
}
