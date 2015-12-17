using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using RandomIconGen.Properties;

namespace RandomIconGen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private Bitmap _icon;
        private readonly Random _random = new Random(DateTime.Now.Second);
        private string _location;


        #region " Icon Generator Buttons "

        private void generateButton_Click(object sender, EventArgs e)
        {
            _icon = GenerateRandomIcon(Convert.ToInt16(sizeUpDown.Value));
            pictureBox1.Image = _icon;
        }



        private void saveButton_Click(object sender, EventArgs e)
        {
            if (_icon == null) return;
            using (var sfd = new SaveFileDialog())
            {
                sfd.FileName = "icon_" + _random.Next(100, 999) + ".ico";
                sfd.Filter = Resources.IconFilter;
                if (sfd.ShowDialog() != DialogResult.OK) return;

                var icon = Icon.FromHandle(_icon.GetHicon());
                using (var fileStream = new FileStream(sfd.FileName, FileMode.Create))
                {
                    icon.Save(fileStream);
                }
            }
        }

        #endregion

        #region " Mass generator Buttons "

        private void locationButton_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.RootFolder = Environment.SpecialFolder.Desktop;
                fbd.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;

                if (fbd.ShowDialog() != DialogResult.OK) return;
                _location = fbd.SelectedPath;
            }
        }

        private void massGenerateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_location)) return;
            System.Threading.Tasks.Task.Run(() =>
            {
                MassGenerateIcon(Convert.ToInt16(amountUpDown.Value), Convert.ToInt16(sizeUpDown.Value), _location);
            });
        }

        #endregion

        #region " Icons Methods "

        private void MassGenerateIcon(int amount, int size, string directory)
        {
            var done = 0;
            while (done <= amount)
            {
                var icon = Icon.FromHandle(GenerateRandomIcon(size).GetHicon());
                using (
                    var fileStream = new FileStream(directory + "\\icon_" + _random.Next(100, 999) + ".ico", FileMode.Create))
                {
                    icon.Save(fileStream);
                }
                done++;
            }
        }

        private Bitmap GenerateRandomIcon(int size)
        {
            var img = new Bitmap(size, size);

            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    img.SetPixel(x, y, RandomColor());
                }
            }

            return img;
        }

        private Color RandomColor()
        {
            return Color.FromArgb(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255));
        }

        #endregion
    }
}
