using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

using Wmb.Drawing;

namespace Wmb.ImageResizer {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            DialogResult dialogResult = openFileDialog1.ShowDialog();

            if (dialogResult == DialogResult.OK) {
                foreach (string fileName in openFileDialog1.FileNames) {
                    listBox1.Items.Add(fileName);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();

            using (Image image = Image.FromFile(listBox1.SelectedItem as string)) {
                pictureBox1.Image = image.Resize(pictureBox1.Width, pictureBox1.Height, GraphicsQuality.High, true);
            }

            Image pictureBoxImage = pictureBox1.Image;

            if (!pictureBoxImage.HasIndexedPixelFormat())
                pictureBoxImage.Transform(new CopyrightTransform("Test"));

            if (toolStripComboBox1.Text != "None") {
                switch (toolStripComboBox1.Text) {
                    case "Grayscale":
                        pictureBoxImage.Transform(new GrayscaleTransform());
                        break;

                    case "Sepia":
                        pictureBoxImage.Transform(new SepiaTransform());
                        break;

                    case "Negative":
                        pictureBoxImage.Transform(new NegativeTransform());
                        break;
                }
            }

            float brightness;
            if (float.TryParse(toolStripTextBox1.Text, out brightness) && brightness != 0f) {
                pictureBoxImage.Transform(new BrightnessTransform(brightness));
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
                string extension = Path.GetExtension(saveFileDialog1.FileName).ToLower();
                switch (extension) {
                    case ".gif":
                        pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Gif);
                        break;

                    case ".png":
                        pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Png);
                        break;

                    case ".bmp":
                        pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Bmp);
                        break;

                    default:
                        pictureBox1.Image.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
                        break;
                }
            }
        }
    }
}
