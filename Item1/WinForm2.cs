using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    public partial class UI2_Body : Form
    {
        public UI2_Body()
        {
            InitializeComponent();
        }

        private void UI_Body_2_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(UI2_TextBox.Text);
        }

        private void UI2_Body_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            //MessageBox.Show("The app is completely free and copyrighted by Mnaisuka","Tip", MessageBoxButtons.OK);
            //e.Cancel = true
            e.Cancel = true;
            new ImageForm(Properties.Resources.sponsor);
        }
    }
    public class ImageForm : Form
    {
        private PictureBox pictureBox;

        public ImageForm(Bitmap image)
        {
            pictureBox = new PictureBox();
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Dock = DockStyle.Fill;
            this.Controls.Add(pictureBox);

            pictureBox.Image = image;
            Width = (int)(image.Width * 0.6);
            Height = (int)(image.Height * 0.6);

            this.ShowInTaskbar = false;

            this.Text = "如果这个项目对你有帮助，请考虑请我喝杯咖啡。";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.ShowIcon = false;

            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.Show();
        }
    }
}
