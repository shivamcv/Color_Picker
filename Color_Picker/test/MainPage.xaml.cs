using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace test
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            
            //HelloWorldWithEvents.Blinked += (object sender, EventArgs args) =>
            //{
            //    System.Diagnostics.Debug.WriteLine("HelloWorldWithEvents Blinked");
            //};


            asd.colorChanged += (object sender, EventArgs args) =>
                {
                    testEll.Fill = new SolidColorBrush(asd.SelectedColor);
                };

        }

        private void asd_selectioncolor_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void asd_PointerMoved_1(object sender, PointerRoutedEventArgs e)
        {
            testEll.Fill = new SolidColorBrush(asd.SelectedColor);

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
     
    }
}
