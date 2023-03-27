using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
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
using CLASS_LIB;

namespace LAB1_S2_VAR2 {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        ViewData viewData = new ViewData();
        public MainWindow() {
            InitializeComponent();
            this.DataContext = viewData;

            init_type.ItemsSource = Enum.GetValues(typeof(FRawEnum));
        }

        private void create_Click(object sender, RoutedEventArgs e) {
            try {
                viewData.ExiquteSpline();
                integral.Text = viewData.Spline.IntergralVal.ToString();
                string[] info = new string[viewData.NodesNum];
                for (int i = 0; i < viewData.NodesNum; i++) {
                    info[i] = $"Coord = {viewData.Raw.Coord[i].ToString("0.000")}\nVal = {viewData.Raw.Val[i].ToString("0.000")}";
                }
                RawData_info.ItemsSource = info;
            }
            catch (Exception ex){
                string messageBoxText = ex.Message;
                string caption = "Ошибочка";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "rawdata";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";
            dialog.ShowDialog();

            viewData.CreateRawData();
            viewData.Raw.Save(dialog.FileName);
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "rawdata";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";
            dialog.ShowDialog();

            try {
                viewData.Load(dialog.FileName);
                viewData.ExiquteSpline();
                integral.Text = viewData.Spline.IntergralVal.ToString();

                string[] info = new string[viewData.NodesNum];
                for (int i = 0; i < viewData.NodesNum; i++) {
                    info[i] = $"Coord = {viewData.Raw.Coord[i].ToString("0.000")}\nVal = {viewData.Raw.Val[i].ToString("0.000")}";
                }
                RawData_info.ItemsSource = info;
            }
            catch (Exception ex) {
                string messageBoxText = ex.Message;
                string caption = "Ошибочка";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
        }
    }
}
