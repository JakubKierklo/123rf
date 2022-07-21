using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _123rf
{
    /// <summary>
    /// Interaction logic for DebugModeUserControl.xaml
    /// </summary>
    public partial class DebugModeUserControl : UserControl
    {
        public static int DebugModeVar { get; set; }
        public DebugModeUserControl()
        {
            InitializeComponent();
        }



        private void DebugMode_Checked(object sender, RoutedEventArgs e)
        {

            TestTest();

        }

        public  void TestTest()
        {
            if (DebugMode.IsChecked == true)
            {
                DebugModeVar = 1;
                DebugMode.IsEnabled = false;
            }
            else
            {
                DebugModeVar = 0;
            }



        }
    }
}
