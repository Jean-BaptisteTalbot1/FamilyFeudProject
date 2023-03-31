using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WpfApp1
{
    public partial class AnswerBlock : UserControl
    {
        private const int ANIMATION_DURATION_MS = 500;

        private bool _isAnimating = false;

        public AnswerBlock()
        {
            InitializeComponent();
        }

        private void UserControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!_isAnimating)
            {
                // Create the animation
                DoubleAnimation animation = new DoubleAnimation(90, TimeSpan.FromMilliseconds(ANIMATION_DURATION_MS));
                ScaleTransform scaleTransform = new ScaleTransform();
                textBlock.RenderTransformOrigin = new Point(0.5, 0.5);
                textBlock.RenderTransform = scaleTransform;
                textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                // Define the animation's timeline
                animation.Completed += (s, a) =>
                {
                    // When the animation is complete, set the background to light blue to indicate that the answer has been revealed
                    border.Background = new SolidColorBrush(Colors.LightBlue);
                };
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
                _isAnimating = true;
            }
        }
    }
}