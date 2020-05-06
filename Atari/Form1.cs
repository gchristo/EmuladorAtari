using Emulator.Models;
using Emulator.Utils;
using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace Emulator
{
    public partial class Form1 : Form
    {
        Chip8 chip;

        Thread chip8Thread;

        public Form1()
        {
            InitializeComponent();

            modeToolStripMenuItem.ComboBox.SetEnumSource<EmulatorModes>(true, (int)EmulatorModes.Interpreter, ommitSelector: true);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = $@"{Application.StartupPath}\Games";
                ofd.Filter = "Chip8 compiled files|*.c8";
                ofd.Multiselect = false;
                ofd.CheckFileExists = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    chip.Reset();
                    chip.LoadGame(File.ReadAllBytes(ofd.FileName));

                    if (chip8Thread != null && chip8Thread.IsAlive)
                    {
                        chip8Thread.Abort();
                    }

                    chip8Thread = new Thread(() => chip.Run());
                    chip8Thread.IsBackground = true;
                    chip8Thread.Start();
                }
            }
        }

        private void Chip_Draw(object sender, byte[] buffer, Size resolution)
        {
            //var g = CreateGraphics();
            var blackBrush = new SolidBrush(Color.Black);
            var whiteBrush = new SolidBrush(Color.White);

            float pixelWidth = Width / (resolution.Width * 1.0f);
            float pixelHeight = Height / (resolution.Height * 1.0f);

            //var bmp = new Bitmap(resolution.Width, resolution.Height);
            var bmp = new Bitmap(Width, Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int y = 0; y < resolution.Height; ++y)
                {
                    for (int x = 0; x < resolution.Width; ++x)
                    {
                        var pixel = new RectangleF
                        {
                            Width = pixelWidth,
                            Height = pixelHeight,
                            X = x * pixelWidth,
                            Y = y * pixelHeight
                        };

                        if ((buffer[(y * resolution.Width) + x]) == 0)
                        {
                            g.FillRectangle(whiteBrush, pixel);
                            //bmp.SetPixel(x, y, Color.White);
                        }
                        else
                        {
                            g.FillRectangle(blackBrush, pixel);
                            //bmp.SetPixel(x, y, Color.Black);
                        }
                    }
                }
            }

            BackgroundImage = bmp;
        }

        private void modeToolStripMenuItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chip8Thread != null && chip8Thread.IsAlive)
            {
                chip8Thread.Abort();
            }

            switch ((EmulatorModes)(((CustomItem)modeToolStripMenuItem.SelectedItem).Id))
            {
                case EmulatorModes.Compiler:
                    chip = new Compiler();
                    break;
                case EmulatorModes.Interpreter:
                    chip = new Interpreter();
                    break;
            }

            chip.Draw += Chip_Draw;
            chip.PlaySound += Chip_PlaySound;
        }

        private void Chip_PlaySound(object sender)
        {
            SystemSounds.Beep.Play();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            chip?.KeyUp(e.KeyCode);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            chip?.KeyDown(e.KeyCode);
        }
    }
}
