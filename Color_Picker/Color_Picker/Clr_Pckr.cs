using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.Foundation;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using System.Reflection;
using System.IO;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace Color_Picker
{
    public  sealed class Clr_Pckr : Control
    {
        Grid pointer;
        Border reference;
        CompositeTransform rtrnsfrm;
        Canvas innerCanvas;
        Grid innerEll;
        byte[] tempBuffer;
        BitmapDecoder bd;
        
        Ellipse FinalColor;
        Rectangle rectColor;
        Image ColorImg;
        GradientStop gdStop;
        Viewbox clrViewbox;

        Thumb thumbInnerEll;
     

      static TextBlock testblock;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Loaded += Clr_Pckr_Loaded;

            clrViewbox = GetTemplateChild("clrViewbox") as Viewbox;
            pointer = GetTemplateChild("pointer") as Grid;
            reference = GetTemplateChild("reference") as Border;
            rtrnsfrm = GetTemplateChild("rtrnsfrm") as CompositeTransform;
            innerCanvas = GetTemplateChild("innerCanvas") as Canvas;
            innerEll = GetTemplateChild("innerEll") as Grid;
            ColorImg = GetTemplateChild("ColorImg") as Image;
           
            thumbInnerEll = GetTemplateChild("thumbInnerEll") as Thumb;
            rectColor = GetTemplateChild("rectColor") as Rectangle;
            gdStop = GetTemplateChild("gdStop") as GradientStop;
            FinalColor = GetTemplateChild("FinalColor") as Ellipse;
            testblock = GetTemplateChild("test") as TextBlock;


            ColorImg.Tapped += ColorImg_Tapped_1;
          
            rectColor.PointerPressed += Rectangle_PointerPressed_1;
            thumbInnerEll.DragDelta += Thumb_DragDelta_1;

            ColorImg.PointerReleased += ColorImg_PointerReleased_1;
            ColorImg.PointerPressed += ColorImg_PointerPressed_1;
            ColorImg.PointerMoved += ColorImg_PointerMoved_1;

            gdStop.Color = SelectedColor;
            FinalColor.Fill = new SolidColorBrush(SelectedColor);



            GeneralTransform gt = pointer.TransformToVisual(reference);

            Point p = new Point();

            p = gt.TransformPoint(p);
            px = p.X;
            py = p.Y;
            loadnew();
        }

        bool loadedFlag = false;
        void Clr_Pckr_Loaded(object sender, RoutedEventArgs e)
        {
            loadedFlag = true;
        }


        private void ColorImg_PointerMoved_1(object sender, PointerRoutedEventArgs e)
        {

            if (ispressed)
            {

                px = e.GetCurrentPoint(reference).Position.X;
                py = e.GetCurrentPoint(reference).Position.Y;



                rtrnsfrm.Rotation = Math.Atan2(py, px) * (180 / Math.PI) + 135;
               
                fillColor();
            }
        }

        bool ispressed = false;

        private void ColorImg_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            ispressed = true;
        }

        private void ColorImg_PointerReleased_1(object sender, PointerRoutedEventArgs e)
        {
            ispressed = false;

        }

        public Clr_Pckr()
        {
            this.DefaultStyleKey = typeof(Clr_Pckr);


            
        }

        private async void loadnew()
        {

            Assembly assembly = typeof(Clr_Pckr).GetTypeInfo().Assembly;
            //Stream imgStream =  assembly.GetManifestResourceStream("Color_Picker.Assets.c.png");


            InMemoryRandomAccessStream res = new InMemoryRandomAccessStream();

            using (var imgstream = assembly.GetManifestResourceStream("Color_Picker.Assets.c.png"))
            {
                await imgstream.CopyToAsync(res.AsStreamForWrite());
            }

           BitmapImage bmp  = new BitmapImage();
           

                                 
           
                using (IRandomAccessStream fileStream =res)// await( await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/c.png"))).OpenAsync(FileAccessMode.Read))
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);

                    fileStream.Seek(0);


                    bmp.SetSource(fileStream);
                    ColorImg.Source = bmp;


                    BitmapTransform bt = new BitmapTransform();

                    bt.ScaledHeight = 100;
                    bt.ScaledWidth = 100;



                    PixelDataProvider pixelData = await decoder.GetPixelDataAsync(
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Straight,
                       bt,
                        ExifOrientationMode.IgnoreExifOrientation,
                        ColorManagementMode.DoNotColorManage);
                    bd = decoder;

                    //byte[] sourcePixels = pixelData.DetachPixelData();
                    tempBuffer = pixelData.DetachPixelData();
                }
           

        }
        private void ColorImg_Tapped_1(object sender, TappedRoutedEventArgs e)
        {

            px = e.GetPosition(reference).X;
            py = e.GetPosition(reference).Y;

            rtrnsfrm.Rotation = Math.Atan2(py, px) * (180 / Math.PI) + 135;

            fillColor();
        }
        private void Rectangle_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            double x, y;

            x = e.GetCurrentPoint(innerCanvas).Position.X;
            y = e.GetCurrentPoint(innerCanvas).Position.Y;



            innerEll.SetValue(Canvas.LeftProperty, x - 4);
            innerEll.SetValue(Canvas.TopProperty, y - 4);
            FinalColor.Fill = new SolidColorBrush(LinearGdHelperClass.GetColorAtPoint(rectColor, new Point(x + 4, y + 4)));

        }
        double px, py;
     
        private void fillColor()
        {
            GeneralTransform gt = pointer.TransformToVisual(ColorImg);
            Point p = new Point();
            p = gt.TransformPoint(p);
            int dx = (int)p.X;
            int dy = (int)p.Y;


            double k = (dy * 100 + dx) * 4;
            try
            {
                int b = tempBuffer[(int)k + 0];
                int g = tempBuffer[(int)k + 1];
                int r = tempBuffer[(int)k + 2];
                int a = tempBuffer[(int)k + 3];
                 gdStop.Color = Color.FromArgb(255, (byte)r, (byte)g, (byte)b);

                FinalColor.Fill = new SolidColorBrush(LinearGdHelperClass.GetColorAtPoint(rectColor, new Point((double)innerEll.GetValue(Canvas.LeftProperty) + 4, (double)innerEll.GetValue(Canvas.TopProperty) + 4)));
                SelectedColor = LinearGdHelperClass.GetColorAtPoint(rectColor, new Point((double)innerEll.GetValue(Canvas.LeftProperty) + 4, (double)innerEll.GetValue(Canvas.TopProperty) + 4));
           

               
            }
            catch { }
        }

        private void Thumb_DragDelta_1(object sender, DragDeltaEventArgs e)
        {
            Grid gd = (Grid)((Thumb)sender).Parent;
            double x, y;
            x = (double)gd.GetValue(Canvas.LeftProperty) + e.HorizontalChange;
            y = (double)gd.GetValue(Canvas.TopProperty) + e.VerticalChange;

            if (x < innerCanvas.Width - 4 && y < innerCanvas.Height - 4 && x > -4 && y > -4)
            {
                innerEll.SetValue(Canvas.LeftProperty, x);
                innerEll.SetValue(Canvas.TopProperty, y);

                FinalColor.Fill = new SolidColorBrush(LinearGdHelperClass.GetColorAtPoint(rectColor, new Point(x + 4, y + 4)));
                SelectedColor = LinearGdHelperClass.GetColorAtPoint(rectColor, new Point(x + 4, y + 4));

            }

        }

        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value);
            OncolorChanged();
                 }
        }

      
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(Clr_Pckr), new PropertyMetadata(Colors.Yellow));

    

        
        public event EventHandler colorChanged;

        private void OncolorChanged()
        {
            EventHandler eh = colorChanged;
            if (eh != null &&  loadedFlag == true)
            {
                eh(this, new PropertyChangedEventArgs("SelectedColor"));
            }
        }

    }
}
