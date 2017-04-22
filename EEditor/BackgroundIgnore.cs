using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EEditor
{
    public partial class BackgroundIgnore : Form
    {
        private ImageList imglist = new ImageList();
        public BackgroundIgnore()
        {
            InitializeComponent();
        }

        private void BackgroundIgnore_Load(object sender, EventArgs e)
        {
            listView1.View = View.Tile;
            listView1.TileSize = new Size(200, 24);
            var width = MainForm.decosBMD.Width / 16 + MainForm.miscBMD.Width / 16 + MainForm.foregroundBMD.Width / 16 + MainForm.backgroundBMD.Width / 16;
            if (imglist.Images.Count == 0)
            {
                Bitmap img1 = new Bitmap(width, 16);
                for (int i = 0; i < width; i++)
                {
                    if (i < 500 || i >= 1001)
                    {
                        if (MainForm.decosBMI[i] != 0)
                        {
                            img1 = MainForm.decosBMD.Clone(new Rectangle(MainForm.decosBMI[i] * 16, 0, 16, 16), MainForm.decosBMD.PixelFormat);
                        }
                        else if (MainForm.miscBMI[i] != 0)
                        {
                            img1 = MainForm.miscBMD.Clone(new Rectangle(MainForm.miscBMI[i] * 16, 0, 16, 16), MainForm.miscBMD.PixelFormat);
                        }
                        else if (MainForm.foregroundBMI[i] != 0)
                        {
                            img1 = MainForm.foregroundBMD.Clone(new Rectangle(MainForm.foregroundBMI[i] * 16, 0, 16, 16), MainForm.foregroundBMD.PixelFormat);
                        }
                    }
                    else if (i >= 500 && i <= 999)
                    {
                        if (MainForm.backgroundBMI[i] != 0)
                        {
                            img1 = MainForm.backgroundBMD.Clone(new Rectangle(MainForm.backgroundBMI[i] * 16, 0, 16, 16), MainForm.backgroundBMD.PixelFormat);
                        }
                    }
                    imglist.Images.Add(img1);

                }
                listView1.LargeImageList  = imglist;
            }
            if (MainForm.userdata.IgnoreBlocks.Count > 0)
            {
                for (int i = 0; i < MainForm.userdata.IgnoreBlocks.Count(); i++)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = "BlockID: " + MainForm.userdata.IgnoreBlocks[i];
                    lvi.ImageIndex = (int)MainForm.userdata.IgnoreBlocks[i];
                    listView1.Items.Add(lvi);
                }
            }
        }
    }
}
