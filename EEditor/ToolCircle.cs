using System;
using System.Drawing;
using System.Windows.Forms;

namespace EEditor
{
    class ToolCircle : Tool
    {
        private Point Q;
        private Point P;
        private Pen borderPen;
        private bool select = false;
        private bool selected = false;
        private int dx;
        private int dy;
        private string incfg = null;
        private Bitmap img1 = new Bitmap(3000, 3000);
        private Bitmap img2 = new Bitmap(3000, 3000);
        private bool hide = true;
        public Rectangle Rect { get; set; }
        public ToolCircle(EditArea editArea)
            : base(editArea)
        {
            borderPen = new Pen(Color.Red);
            borderPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
        }

        #region Mouse events
        public override void MouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point cur1 = GetLocation(e);
                if (IsPaintable(cur1.X, cur1.Y))
                {
                    if (selected)
                    {
                        Point cur = GetLocation(e);
                        if (Rect.Contains(cur))
                        {
                            dx = cur.X - Rect.X;
                            dy = cur.Y - Rect.Y;
                        }
                        else
                        {
                            RemoveBorder();
                            selected = false;
                            select = false;
                        }
                    }
                    else
                    {
                        P = GetLocation(e);
                        PlaceBorder(P);
                        select = true;
                    }
                }
            }
        }

        public override void MouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //Rect(yStart, xStart, p.Y, p.X);
                Point cur1 = GetLocation(e);
                if (IsPaintable(cur1.X, cur1.Y))
                {
                    if (select)
                    {
                        Point p = GetLocation(e);
                        PlaceBorder(p);
                    }
                    if (selected)
                    {
                        Point cur = GetLocation(e);
                        Rectangle nextRect = new Rectangle(new Point(cur.X - dx, cur.Y - dy), Rect.Size);
                        if (nextRect != Rect)
                        {
                            RemoveBorderRect();
                            Rect = nextRect;
                            PlaceBorderRect();
                        }
                    }
                }
            }
        }

        public override void MouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point cur1 = GetLocation(e);
                if (IsPaintable(cur1.X, cur1.Y))
                {
                    if (select)
                    {
                        Q = GetLocation(e);
                        if (P.X == Q.X && P.Y == Q.Y) { }
                        else
                        {
                            Circle(P, Q);
                        }
                        select = false;
                        selected = false;
                        RemoveBorder();
                    }
                }
            }
        }
        #endregion

        #region Draw Border
        public void PlaceBorder(Point q)
        {
            Graphics g = Graphics.FromImage(editArea.Back);
            RemoveBorder(g);
            Q = q;
            g.DrawRectangle(borderPen, GetRectangleScaled(P, Q));
            editArea.Invalidate();
        }

        public Rectangle GetRectangleScaled(Point p, Point q)
        {
            Rectangle r = new Rectangle(Math.Min(p.X, q.X) * 16, Math.Min(p.Y, q.Y) * 16,
                    (Math.Max(p.X, q.X) - Math.Min(p.X, q.X) + 1) * 16 - 1, (Math.Max(p.Y, q.Y) - Math.Min(p.Y, q.Y) + 1) * 16 - 1);
            return r;
        }

        public void RemoveBorder(Graphics g = null)
        {
            if (Q != null)
            {
                if (g == null) g = Graphics.FromImage(editArea.Back);
                Rectangle r = GetRectangle(P, Q);
                for (int x = 0; x < r.Width; ++x) editArea.Draw(x + r.X, r.Y, g, MainForm.userdata.thisColor);
                for (int x = 0; x < r.Width; ++x) editArea.Draw(x + r.X, r.Y + r.Height - 1, g, MainForm.userdata.thisColor);
                for (int y = 0; y < r.Height; ++y) editArea.Draw(r.X, y + r.Y, g, MainForm.userdata.thisColor);
                for (int y = 0; y < r.Height; ++y) editArea.Draw(r.X + r.Width - 1, y + r.Y, g, MainForm.userdata.thisColor);
                g.Save();
            }
        }

        public Rectangle GetRectangle(Point p, Point q)
        {
            Rectangle r = new Rectangle(Math.Min(p.X, q.X), Math.Min(p.Y, q.Y),
                    Math.Max(p.X, q.X) - Math.Min(p.X, q.X) + 1, Math.Max(p.Y, q.Y) - Math.Min(p.Y, q.Y) + 1);
            return r;
        }

        protected void RemoveBorderRect()
        {
            Graphics g = Graphics.FromImage(editArea.Back);
            Rectangle r = Rect;
            for (int x = 0; x < r.Width; ++x)
                for (int y = 0; y < r.Height; ++y)
                {
                    if (0 <= x + r.X && x + r.X < editArea.BlockWidth && 0 <= y + r.Y && y + r.Y < editArea.BlockHeight)
                        editArea.Draw(x + r.X, y + r.Y, g, MainForm.userdata.thisColor);
                }
            g.Save();
        }

        public void PlaceBorderRect()
        {
            Graphics g = Graphics.FromImage(editArea.Back);
            Rectangle r = Rect;
            for (int x = 0; x < r.Width; ++x)
                for (int y = 0; y < r.Height; ++y)
                {
                    if (0 <= x + r.X && x + r.X < editArea.BlockWidth && 0 <= y + r.Y && y + r.Y < editArea.BlockHeight)
                    {
                        editArea.Draw(x + r.X, y + r.Y, g, MainForm.userdata.thisColor);
                    }
                }
            Point p1 = r.Location;
            Point p2 = new Point(r.Location.X + r.Width - 1, r.Location.Y + r.Height - 1);
            g.DrawRectangle(borderPen, GetRectangleScaled(p1, p2));
            g.Save();
            editArea.Invalidate();
        }
        #endregion

        public void Circle(Point start, Point end)
        {
            Graphics g = Graphics.FromImage(editArea.Back);
            img1 = MainForm.foregroundBMD.Clone(new Rectangle(MainForm.foregroundBMI[PenID] * 16, 0, 16, 16), MainForm.foregroundBMD.PixelFormat);
            int x0 = (int)Math.Min(start.X, end.X);
            int x1 = (int)Math.Max(start.X, end.X);
            int y0 = (int)Math.Min(start.Y, end.Y);
            int y1 = (int)Math.Max(start.Y, end.Y);
            int a = Math.Abs(x1 - x0),
            b = Math.Abs(y1 - y0),
            b1 = b & 1; /* values of diameter */
            long dx = 4 * (1 - a) * b * b, dy = 4 * (b1 + 1) * a * a; /* error increment */
            long err = dx + dy + b1 * a * a, e2; /* error of 1.step */

            if (x0 > x1) { x0 = x1; x1 += a; } /* if called with swapped points */
            if (y0 > y1) y0 = y1; /* .. exchange them */
            y0 += (b + 1) / 2; y1 = y0 - b1;   /* starting pixel */
            a *= 8 * a; b1 = 8 * b * b;

            do
            {
                paintPixel(x1, y0); /*   I. Quadrant */
                paintPixel(x0, y0); /*  II. Quadrant */
                paintPixel(x0, y1); /* III. Quadrant */
                paintPixel(x1, y1); /*  IV. Quadrant */

                e2 = 2 * err;
                if (e2 <= dy) { y0++; y1--; err += dy += a; }  /* y step */
                if (e2 >= dx || 2 * err > dy) { x0++; x1--; err += dx += b1; } /* x step */
            } while (x0 <= x1);

            while (y0 - y1 < b)
            {  /* too early stop of flat ellipses a=1 */
                paintPixel(x0 - 1, y0); /* -> finish tip of ellipse */
                paintPixel(x1 + 1, y0++);
                paintPixel(x0 - 1, y1);
                paintPixel(x1 + 1, y1--);
            }
            editArea.Invalidate();
            if (incfg != null) ToolPen.undolist.Push(incfg);
            incfg = null;

            /*radius = radius / 2;
            using (Graphics g = Graphics.FromImage(editArea.Back))
            {
                for (double ii = 0.1; ii < 360.0; ii += 0.2)
                {
                    double theta = ii * (Math.PI / 180);
                    double r = radius;

                    int x = (int)((xx.X + Math.Cos(theta) * r) + radius);
                    int y = (int)((yy.Y + Math.Sin(theta) * r) + radius);

                    if (x < editArea.CurFrame.Width && y < editArea.CurFrame.Height)
                    {
                        if (x < 0 || y < 0) { }
                        else
                        {
                            if (IsPaintable(x, y, PenID))
                            {
                                //System.Threading.Thread.Sleep(1); // If you want to draw circle very slowly.
                                if (PenID >= 500 && PenID <= 999) editArea.CurFrame.Background[y, x] = PenID;
                                else if (PenID < 500 || PenID >= 1001) editArea.CurFrame.Foreground[y, x] = PenID;
                                editArea.Draw(x, y, g, Properties.Settings.Default.thiscolor);
                            }
                        }
                    }
                }
                editArea.Invalidate();

            }*/

        }

        public void paintPixel(int x, int y)
        {
            using (Graphics g = Graphics.FromImage(editArea.Back))
            {
                if (PenID >= 500 && PenID <= 999)
                {
                    if (PenID != editArea.CurFrame.Background[y, x]) incfg += PenID + ":" + editArea.CurFrame.Background[y, x] + ":" + x + ":" + y + ":";
                    editArea.CurFrame.Background[y, x] = PenID;
                }
                else if (PenID < 500 || PenID >= 1001)
                {
                    if (PenID != editArea.CurFrame.Foreground[y, x]) incfg += PenID + ":" + editArea.CurFrame.Foreground[y, x] + ":" + x + ":" + y + ":";
                    if (IsPaintable(x, y, PenID, true) && IsPaintable(x, y, PenID, false)) editArea.CurFrame.Foreground[y, x] = PenID;
                }
                editArea.Draw(x, y, g, MainForm.userdata.thisColor);
            }
        }
    }
}
