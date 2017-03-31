using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace EEditor
{
    public partial class InsertImageForm : Form
    {
        private string[,] Area;
        private string[,] Back;
        private string[,] Coins;
        private string[,] Id;
        private string[,] Target;
        private string[,] Text1;
        int n;
        float[] H;
        float[] S;
        float[] B;

        private Thread thread;
        public static List<int> Blocks = new List<int>();
        public static List<int> Background = new List<int>();
        public static List<int> SpecialMorph = new List<int>();
        public static List<int> SpecialAction = new List<int>();

        public InsertImageForm()
        {
            InitializeComponent();
            checkBoxBackground.Checked = MainForm.userdata.imageBackgrounds;
            checkBoxBlocks.Checked = MainForm.userdata.imageBlocks;
            MorphablecheckBox.Checked = MainForm.userdata.imageSpecialblocksMorph;
            ActionBlockscheckBox.Checked = MainForm.userdata.imageSpecialblocksAction;
            n = Minimap.ImageColor.Count();
            H = new float[n];
            S = new float[n];
            B = new float[n];
            for (int i = 0; i < n; ++i)
                if (Minimap.ImageColor[i])
                {
                    Color c = Color.FromArgb((int)Minimap.Colors[i]);
                    H[i] = c.GetHue();
                    S[i] = c.GetSaturation();
                    B[i] = c.GetBrightness();
                }
        }

        private void loadImageButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog imageFileDialog = new OpenFileDialog();
            imageFileDialog.Filter = "Images |*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.ico";
            imageFileDialog.Title = "Choose an Image";
            if (imageFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap originalImage = new Bitmap(Bitmap.FromFile(imageFileDialog.FileName));
                thread = new Thread(delegate () { Transform(originalImage); });
                thread.Start();

                //pictureBox1.Image = image;
                this.Invalidate();
            }
        }

        public void loadDroppedImage(string filename)
        {
            Bitmap originalImage = new Bitmap(Bitmap.FromFile(filename));
            thread = new Thread(delegate () { Transform(originalImage); });
            thread.Start();

            //pictureBox1.Image = image;
            //this.Invalidate();
        }
        #region EEditor from image to ee
        private double DistanceHSB(float H0, float S0, float B0, int k)
        {
            return Math.Pow(H0 - H[k], 2) + Math.Pow(S0 - S[k], 2) + Math.Pow(B0 - B[k], 2);
        }

        private int BestMatchHSB(Color c)
        {
            int j = 0;
            float H = c.GetHue();
            float S = c.GetSaturation();
            float B = c.GetBrightness();
            double d = DistanceHSB(H, S, B, 0);
            for (int i = 1; i < n; i++)
                if (Minimap.ImageColor[i])
                {
                    double dist = DistanceHSB(H, S, B, i);
                    if (dist < d)
                    {
                        d = dist;
                        j = i;
                    }
                }
            return j;
        }

        private double Distance(Color a, Color b)
        {
            return Math.Pow(a.R - b.R, 2) + Math.Pow(a.G - b.G, 2) + Math.Pow(a.B - b.B, 2);
        }

        private int BestMatchRGB(Color c)
        {
            int j = 0;
            int a = 0;
            double d = Distance(c, Color.FromArgb((int)Minimap.Colors[0]));
            if (checkBoxBackground.Checked)
                foreach (int i in Background)
                    if (Minimap.ImageColor[i])
                    {
                        double dist = Distance(c, Color.FromArgb((int)Minimap.Colors[i]));
                        if (dist < d)
                        {
                            d = dist;
                            j = i;
                        }
                    }

            if (checkBoxBlocks.Checked)
                foreach (int i in Blocks)
                    if (Minimap.ImageColor[i])
                    {
                        double dist = Distance(c, Color.FromArgb((int)Minimap.Colors[i]));
                        if (dist < d)
                        {
                            d = dist;
                            j = i;
                        }
                    }
            if (MorphablecheckBox.Checked)
            {
                foreach (int i in SpecialMorph)
                {
                    if (Minimap.ImageColor[i])
                    {
                        double dist = Distance(c, Color.FromArgb((int)Minimap.Colors[i]));
                        if (dist < d)
                        {
                            d = dist;
                            j = i;
                        }
                    }
                }
            }
            if (ActionBlockscheckBox.Checked)
            {
                foreach (int i in SpecialAction)
                {
                    if (Minimap.ImageColor[i])
                    {
                        double dist = Distance(c, Color.FromArgb((int)Minimap.Colors[i]));
                        if (dist < d)
                        {
                            d = dist;
                            j = i;
                        }
                    }
                }
            }

            return j;

        }
        private int closestColor(Color col, int bid)
        {
            Color black = Color.Black;
            Color color2 = Color.Black;
            int bids = 0;
            double num = 999.0;
            double d = 0.0;
            foreach (int i in Background)
            {
                if (Minimap.ImageColor[i])
                {
                    color2 = Color.FromArgb((int)Minimap.Colors[i]);

                    if (color2.R >= col.R)
                    {
                        d += Math.Pow((double)(color2.R - col.R), 2.0);
                    }
                    else
                    {
                        d += Math.Pow((double)(col.R - color2.R), 2.0);
                    }
                    if (color2.G >= col.G)
                    {
                        d += Math.Pow((double)(color2.G - col.G), 2.0);
                    }
                    else
                    {
                        d += Math.Pow((double)(col.G - color2.G), 2.0);
                    }
                    if (color2.B >= col.B)
                    {
                        d += Math.Pow((double)(color2.B - col.B), 2.0);
                    }
                    else
                    {
                        d += Math.Pow((double)(col.B - color2.B), 2.0);
                    }
                    d = Math.Sqrt(d);
                    if (d < num)
                    {
                        num = d;
                        black = color2;
                    }
                    if (bid >= 500 && bid <= 999) bids = Background[i];
                }
                

            }
            return bids;
        }
        #endregion

        private void Transform(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;
            Area = new string[height, width];
            Back = new string[height, width];
            Coins = new string[height, width];
            Id = new string[height, width];
            Target = new string[height, width];
            Text1 = new string[height, width];
            if (width > MainForm.editArea.BlockWidth || height > MainForm.editArea.BlockHeight)
            {
                DialogResult rs = MessageBox.Show("The image is bigger than the world you have. Do you want to continue?", "Image bigger than world", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (rs == DialogResult.Yes)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            int c;
                            if (image.GetPixel(x, y).A == 255 && image.GetPixel(x, y).R == 0 && image.GetPixel(x, y).G == 0 && image.GetPixel(x, y).G == 0)
                            {
                                c = 0;
                            }
                            else if (image.GetPixel(x, y).A == 0 && image.GetPixel(x, y).R == 0 && image.GetPixel(x, y).G == 0 && image.GetPixel(x, y).G == 0)
                            {
                                c = 0;
                            }
                            else
                            {
                                c = BestMatchRGB(image.GetPixel(x, y));
                            }
                            if (c < 500 || c >= 1001 || c == -1)
                                Area[y, x] = Convert.ToString(c);
                            else
                                Back[y, x] = Convert.ToString(c);
                        }
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        int c;
                        if (image.GetPixel(x, y).A == 255 && image.GetPixel(x, y).R == 0 && image.GetPixel(x, y).G == 0 && image.GetPixel(x, y).G == 0)
                        {
                            c = 0;
                        }
                        else if (image.GetPixel(x, y).A == 0 && image.GetPixel(x, y).R == 0 && image.GetPixel(x, y).G == 0 && image.GetPixel(x, y).G == 0)
                        {
                            c = 0;
                        }
                        else
                        {
                            c = BestMatchRGB(image.GetPixel(x, y));
                        }
                        if (c < 500 || c >= 1001 || c == -1)
                            Area[y, x] = Convert.ToString(c);
                        else
                            Back[y, x] = Convert.ToString(c);
                    }
                }

            }
            DialogResult imagedone = MessageBox.Show("The image has been loaded! Would you like to insert it now?\n\nYes - inserts loaded image at current world position\nNo - adds loaded image to clipboard, so you can paste it with Ctrl + V\nCancel - lets you pick another image", "Image loaded", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (imagedone == DialogResult.Yes)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Clipboard.SetData("EEData", new string[][,] { Area, Back, Coins, Id, Target, Text1 });
                    MainForm.editArea.Focus();
                    SendKeys.Send("^{v}");
                    Close();
                });

            }
            else if (imagedone == DialogResult.No)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Clipboard.SetData("EEData", new string[][,] { Area, Back, Coins, Id, Target, Text1 });
                    Close();
                });
            }
            try
            {
                thread.Abort();
            }
            catch
            {
            }


        }

        /*if (button1.InvokeRequired) this.Invoke((MethodInvoker)delegate { button1.Enabled = true; });
        else button1.Enabled = true;
        if (button1.InvokeRequired) this.Invoke((MethodInvoker)delegate { addToMapButton.Enabled = true; });
        else addToMapButton.Enabled = true;*/

        private void InsertImageForm_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void InsertImageForm_Load(object sender, EventArgs e)
        {

        }

        private void checkBoxBackground_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.userdata.imageBackgrounds = checkBoxBackground.Checked;
        }

        private void checkBoxBlocks_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.userdata.imageBlocks = checkBoxBlocks.Checked;
        }

        private void MorphablecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.userdata.imageSpecialblocksMorph = MorphablecheckBox.Checked;
        }

        private void ActionBlockscheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.userdata.imageSpecialblocksAction = ActionBlockscheckBox.Checked;
        }
    }
}
