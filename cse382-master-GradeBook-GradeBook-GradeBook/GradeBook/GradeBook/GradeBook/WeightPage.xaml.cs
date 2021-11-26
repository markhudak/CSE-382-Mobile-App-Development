using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace GradeBook
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeightPage : ContentPage
    {
        string course;
      //  double valu1;
       // double valu2;
        //double valu3;
        SKCanvasView canvasView;

        public WeightPage(string courseT)
        {
            InitializeComponent();

            course = courseT;
            crsLab.Text = course;
            double val = 0;
            if (!Preferences.ContainsKey("Test" + course))
                Preferences.Set("Test" + course, val);
            sli1.Value = Preferences.Get("Test" + course, val);

            double val2 = 0;
            if (!Preferences.ContainsKey("Quiz" + course))
                Preferences.Set("Quiz" + course, val2);
            sli2.Value = Preferences.Get("Quiz" + course, val2);

            double val3 = 0;
            if (!Preferences.ContainsKey("HW" + course))
                Preferences.Set("HW" + course, val3);
            sli3.Value = Preferences.Get("HW" + course, val3);
/*
            valu1 = sli1.Value;
            valu2 = sli2.Value;
            valu3 = sli3.Value;
*/
            if ((sli1.Value + sli2.Value + sli3.Value > 99) && (sli1.Value + sli2.Value + sli3.Value < 101))
            {
                ideal.Text = "Ideal calculating conditions!";
            }
            else if ((sli1.Value + sli2.Value + sli3.Value >= 99) || ((sli1.Value + sli2.Value + sli3.Value <= 101) && (sli1.Value + sli2.Value + sli3.Value > 0)))
            {
                ideal.Text = "Not ideal, but we can do it.";
            }
            else
            {
                ideal.Text = "Just waiting on your weights, thanks.";
            }





            canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            stack.Children.Add(canvasView);
        }
    

    void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
        SKImageInfo info = args.Info;
        SKSurface surface = args.Surface;
        SKCanvas canvas = surface.Canvas;

        canvas.Clear();

        using (SKPath path = new SKPath())
        {
                if ((sli1.Value + sli2.Value + sli3.Value > 0) && (sli1.Value + sli2.Value + sli3.Value < 10))
                {
                    path.MoveTo(0, 0);                                // Center 
                    path.CubicTo(50, -50, 95, -100, 150, -100); // To top of right loop
                    path.CubicTo(205, -100, 250, -55, 250, 0); // To far right of right loop
                    path.CubicTo(250, 55, 205, 100, 150, 100); // To bottom of right loop

                }
                else if ((sli1.Value + sli2.Value + sli3.Value >= 10) && (sli1.Value + sli2.Value + sli3.Value < 35))
                {
                    path.MoveTo(0, 0);                                // Center 
                    path.CubicTo(50, -50, 95, -100, 150, -100); // To top of right loop
                    path.CubicTo(205, -100, 250, -55, 250, 0); // To far right of right loop
                    path.CubicTo(250, 55, 205, 100, 150, 100); // To bottom of right loop
                    path.CubicTo(95, 100, 50, 50, 0, 0); // Back to center
                }
                else if ((sli1.Value + sli2.Value + sli3.Value >= 35) && (sli1.Value + sli2.Value + sli3.Value < 66))
                {
                    path.MoveTo(0, 0);                                // Center 
                    path.CubicTo(50, -50, 95, -100, 150, -100); // To top of right loop
                    path.CubicTo(205, -100, 250, -55, 250, 0); // To far right of right loop
                    path.CubicTo(250, 55, 205, 100, 150, 100); // To bottom of right loop
                    path.CubicTo(95, 100, 50, 50, 0, 0); // Back to center
                    path.CubicTo(-50, -50, -95, -100, -150, -100); // To top of left loop
                }
                else if ((sli1.Value + sli2.Value + sli3.Value >= 66) && (sli1.Value + sli2.Value + sli3.Value < 99))
                {
                    path.MoveTo(0, 0);                                // Center 
                    path.CubicTo(50, -50, 95, -100, 150, -100); // To top of right loop
                    path.CubicTo(205, -100, 250, -55, 250, 0); // To far right of right loop
                    path.CubicTo(250, 55, 205, 100, 150, 100); // To bottom of right loop
                    path.CubicTo(95, 100, 50, 50, 0, 0); // Back to center
                    path.CubicTo(-50, -50, -95, -100, -150, -100); // To top of left loop
                    path.CubicTo(-205, -100, -250, -55, -250, 0); // To far left of left loop
                    path.CubicTo(-250, 55, -205, 100, -150, 100); // To bottom of left loop

                }
                else
                {
                    path.MoveTo(0, 0);                                // Center 
                    path.CubicTo(50, -50, 95, -100, 150, -100); // To top of right loop
                    path.CubicTo(205, -100, 250, -55, 250, 0); // To far right of right loop
                    path.CubicTo(250, 55, 205, 100, 150, 100); // To bottom of right loop
                    path.CubicTo(95, 100, 50, 50, 0, 0); // Back to center
                    path.CubicTo(-50, -50, -95, -100, -150, -100); // To top of left loop
                    path.CubicTo(-205, -100, -250, -55, -250, 0); // To far left of left loop
                    path.CubicTo(-250, 55, -205, 100, -150, 100); // To bottom of left loop
                    path.CubicTo(-95, 100, -50, 50, -150, 100); // Back to center
                    path.Close();
                }

            SKRect pathBounds = path.Bounds;
            canvas.Translate(info.Width / 2, info.Height / 2);
            canvas.Scale(0.9f * Math.Min(info.Width / pathBounds.Width,
                                         info.Height / pathBounds.Height));

            using (SKPaint paint = new SKPaint())
            {
                paint.Style = SKPaintStyle.Stroke;
                paint.Color = SKColors.Green;
                paint.StrokeWidth = 5;

                canvas.DrawPath(path, paint);
            }
        }
    }

    private void ValueChanged1(object sender, ValueChangedEventArgs e)
        {
                Preferences.Set("Test" + course, sli1.Value);
            if ((sli1.Value + sli2.Value + sli3.Value > 99) && (sli1.Value + sli2.Value + sli3.Value < 101))
            {
                ideal.Text = "Ideal calculating conditions!";
            }
            else if ((sli1.Value + sli2.Value + sli3.Value >= 101) || ((sli1.Value + sli2.Value + sli3.Value <= 99) && (sli1.Value + sli2.Value + sli3.Value > 0)))
            {
                ideal.Text = "Not ideal, but we can do it.";
            }
            else 
            {
                ideal.Text = "Just waiting on your weights, thanks.";
            }
            if (canvasView != null)
            {
                canvasView.InvalidateSurface();
            }
        }

        private void ValueChanged2(object sender, ValueChangedEventArgs e)
        {
            Preferences.Set("Quiz" + course, sli2.Value);
            if ((sli1.Value + sli2.Value + sli3.Value > 99) && (sli1.Value + sli2.Value + sli3.Value < 101))
            {
                ideal.Text = "Ideal calculating conditions!";
            }
            else if ((sli1.Value + sli2.Value + sli3.Value >= 101) || ((sli1.Value + sli2.Value + sli3.Value <= 99) && (sli1.Value + sli2.Value + sli3.Value > 0)))
            {
                ideal.Text = "Not ideal, but we can do it.";
            }
            else
            {
                ideal.Text = "Just waiting on your weights, thanks.";
            }

            if (canvasView != null) 
            {
                canvasView.InvalidateSurface();
            }

        }

        private void ValueChanged3(object sender, ValueChangedEventArgs e)
        {
            Preferences.Set("HW" + course, sli3.Value);
            if ((sli1.Value + sli2.Value + sli3.Value > 99) && (sli1.Value + sli2.Value + sli3.Value < 101))
            {
                ideal.Text = "Ideal calculating conditions!";
            }
            else if ((sli1.Value + sli2.Value + sli3.Value >= 99) || ((sli1.Value + sli2.Value + sli3.Value <= 101) && (sli1.Value + sli2.Value + sli3.Value > 0)))
            {
                ideal.Text = "Not ideal, but we can do it.";
            }
            else
            {
                ideal.Text = "Just waiting on your weights, thanks.";
            }
            if (canvasView != null)
            {
                canvasView.InvalidateSurface();
            }
        }

    

    }
}