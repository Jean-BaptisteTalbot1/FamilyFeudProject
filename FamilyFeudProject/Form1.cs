using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FamilyFeudProject
{
    public partial class Form1 : Form
        {
            private const int ANIMATION_TICK_INTERVAL = 50; // milliseconds
            private const int ANIMATION_FRAMES = 10; // number of animation frames
            private const int BLOCK_WIDTH = 100;
            private const int BLOCK_HEIGHT = 100;
            private const int BLOCK_DEPTH = 100;
            private readonly Color HIDDEN_BLOCK_COLOR = Color.Blue;
            private readonly Color REVEALED_BLOCK_COLOR = Color.LightBlue;
            private readonly Color TOP_BLOCK_COLOR = Color.Blue;
            private readonly Font BLOCK_FONT = new Font("Arial", 12, FontStyle.Bold);

            private Timer _animationTimer;
            private PictureBox _selectedPictureBox;

            public Form1()
            {
                InitializeComponent();

                currentFrame = 0;



                // Create PictureBox controls for each block
                for (int i = 0; i < 6; i++)
                {
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.BorderStyle = BorderStyle.Fixed3D;
                    pictureBox.Cursor = Cursors.Hand;
                    pictureBox.Padding = new Padding(2);
                    pictureBox.Size = new Size(BLOCK_WIDTH, BLOCK_HEIGHT);
                    pictureBox.Location = new Point(20 + (i * (BLOCK_WIDTH + 10)), 20);
                    pictureBox.BackColor = HIDDEN_BLOCK_COLOR;
                    pictureBox.Click += PictureBox_Click;
                    Controls.Add(pictureBox);
                }

                // Create a Timer control for the animation
                _animationTimer = new Timer();
                _animationTimer.Interval = ANIMATION_TICK_INTERVAL;
                _animationTimer.Tick += AnimationTimer_Tick;
            }

            private void PictureBox_Click(object sender, EventArgs e)
            {
                if (_animationTimer.Enabled)
                {
                    return;
                }

                // Save a reference to the clicked PictureBox
                _selectedPictureBox = (PictureBox)sender;

                // Start the animation
                _animationTimer.Start();
            }

            private int currentFrame;



            //private void DrawBlock(Graphics graphics, Color color)
            //{
            //    int width = BLOCK_WIDTH;
            //    int height = BLOCK_HEIGHT;
            //    int depth = BLOCK_DEPTH;

            //    // Create the block path
            //    GraphicsPath path = new GraphicsPath();
            //    path.AddPolygon(new Point[]
            //    {
            //        new Point(0, 0),
            //        new Point(width, 0),
            //        new Point(width, height),
            //        new Point(0, height)
            //    });

            //    // Apply the perspective transform
            //    Matrix transform = new Matrix();
            //    transform.Translate(-width / 2, -height / 2);
            //    transform.RotateX(ANGLE);
            //    transform.Translate(width / 2, height / 2);
            //    graphics.Transform = transform;

            //    // Fill the block with the appropriate color
            //    using (SolidBrush brush = new SolidBrush(color))
            //    {
            //        graphics.FillPath(brush, path);
            //    }

            //    // Draw the block outline
            //    using (Pen pen = new Pen(Color.Black))
            //    {
            //        graphics.DrawPath(pen, path);
            //    }
            //}


        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            // Calculate the current frame of the animation
            currentFrame++;

            // Calculate the rotation angle for the current frame
            double angle = Math.PI / 2 * currentFrame / ANIMATION_FRAMES;

            // Calculate the scale factor for the current frame
            double scale = Math.Sin(angle);

            // Calculate the perspective factor for the current frame
            double perspective = 1 + (Math.Cos(angle) - 1) / 2;

            // Create a bitmap to draw the rotated block onto
            Bitmap bitmap = new Bitmap(BLOCK_WIDTH, BLOCK_HEIGHT);
            Graphics graphics = Graphics.FromImage(bitmap);

            // Apply the perspective transform
            Matrix perspectiveTransform = new Matrix();
            perspectiveTransform.Translate(BLOCK_WIDTH / 2, BLOCK_HEIGHT / 2);
            perspectiveTransform.Scale((float)perspective, (float)perspective);
            graphics.Transform = perspectiveTransform;

            // Apply the rotation transform
            Matrix rotationTransform = new Matrix();
            rotationTransform.Rotate((float)(angle * 180 / Math.PI), MatrixOrder.Append);
            graphics.Transform = rotationTransform;

            // Fill the block with the appropriate color
            graphics.FillRectangle(new SolidBrush(HIDDEN_BLOCK_COLOR), -BLOCK_WIDTH / 2, -BLOCK_HEIGHT / 2, BLOCK_WIDTH, BLOCK_HEIGHT);

            // Draw the answer text
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            graphics.DrawString("Answer", BLOCK_FONT, Brushes.Black, new RectangleF(-BLOCK_WIDTH / 2, -BLOCK_HEIGHT / 2, BLOCK_WIDTH, BLOCK_HEIGHT), stringFormat);

            // If the animation is complete, stop the Timer and set the final image
            if (currentFrame == ANIMATION_FRAMES)
            {
                currentFrame = 0;
                _animationTimer.Stop();
                _selectedPictureBox.BackColor = REVEALED_BLOCK_COLOR;
                graphics.FillRectangle(new SolidBrush(REVEALED_BLOCK_COLOR), -BLOCK_WIDTH / 2, -BLOCK_HEIGHT / 2, BLOCK_WIDTH, BLOCK_HEIGHT);
                graphics.DrawString("Answer", BLOCK_FONT, Brushes.Black, new RectangleF(-BLOCK_WIDTH / 2, -BLOCK_HEIGHT / 2, BLOCK_WIDTH, BLOCK_HEIGHT), stringFormat);
                _selectedPictureBox.Image = null;
            }

            // Set the current frame of the animation as the PictureBox's image
            _selectedPictureBox.Image = bitmap;

            graphics.Dispose();
        }



    }
}
