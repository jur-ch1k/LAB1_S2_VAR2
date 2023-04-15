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
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using static System.Reflection.Metadata.BlobBuilder;

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

        public PlotModel MyModel { get; private set; }

        private void LoadButton_Click(object sender, RoutedEventArgs e) {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "rawdata";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";
            dialog.ShowDialog();

            try {
                viewData.Load(dialog.FileName);
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
        private void CanSaveCreateHandler(object sender, CanExecuteRoutedEventArgs e) {
            if (grid != null) {
                foreach (FrameworkElement child in grid.Children) {
                    if (Validation.GetHasError(child) == true) {
                        e.CanExecute = false;
                        return;
                    }
                    e.CanExecute = true;
                }
            }
            else e.CanExecute = false;
        }
        private void SaveHandler(object sender, ExecutedRoutedEventArgs e) {
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "rawdata";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";
            dialog.ShowDialog();

            viewData.CreateRawData();
            viewData.Raw.Save(dialog.FileName);
        }
        private void LoadHandler(object sender, ExecutedRoutedEventArgs e) {
            viewData.ExiquteSpline(false);
            integral.Text = viewData.Spline.IntergralVal.ToString("0.000");

            string[] info = new string[viewData.NodesNum];
            for (int i = 0; i < viewData.NodesNum; i++) {
                info[i] = $"Coord = {viewData.Raw.Coord[i].ToString("0.000")}\nVal = {viewData.Raw.Val[i].ToString("0.000")}";
            }
            RawData_info.ItemsSource = info;
            DrawPlot();
        }
        private void CreateHandler(object sender, ExecutedRoutedEventArgs e) {
            try {
                viewData.ExiquteSpline();
                integral.Text = viewData.Spline.IntergralVal.ToString("0.000");
                string[] info = new string[viewData.NodesNum];
                for (int i = 0; i < viewData.NodesNum; i++) {
                    info[i] = $"Coord = {viewData.Raw.Coord[i].ToString("0.000")}\nVal = {viewData.Raw.Val[i].ToString("0.000")}";
                }
                RawData_info.ItemsSource = info;
                DrawPlot();
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

        private void DrawPlot() {
            PlotModel model = new PlotModel();
            model.Axes.Add(new LinearAxis() {
                Position = AxisPosition.Bottom,
                Minimum = viewData.LEnd,
                Maximum = viewData.REnd,
                Title = "Coord"
            });
            model.Axes.Add(new LinearAxis() {
                Position = AxisPosition.Left,
                Minimum = viewData.Spline.SplineList.Min(x => x.Val),
                Maximum = viewData.Spline.SplineList.Max(x => x.Val),
                Title = "Value"
            });

            LineSeries rawline = new LineSeries() {
                Title = "RawData",
                Color = OxyColors.Transparent,
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.Black
            };
            for (int i = 0; i < viewData.NodesNum; i++) {
                rawline.Points.Add(new DataPoint(viewData.Raw.Coord[i], viewData.Raw.Val[i]));
            }

            LineSeries splineline = new LineSeries() {
                Title = "SplineData",
                Color = OxyColors.Green,
            };
            for (int i = 0; i < viewData.SplineNodesNum; i++) {
                splineline.Points.Add(new DataPoint(
                    viewData.Spline.SplineList[i].Coord,
                    viewData.Spline.SplineList[i].Val
                    ));
            }

            model.Series.Add(splineline);
            model.Series.Add(rawline);
            plot.Model = model;
        }
    }
    public static class CustomCommands {
        public static RoutedCommand SaveCommand = new RoutedCommand("SaveCommand", typeof(CustomCommands));
        public static RoutedCommand LoadCommand = new RoutedCommand("LoadCommand", typeof(CustomCommands));
        public static RoutedCommand CreateCommand = new RoutedCommand("AddCommand", typeof(CustomCommands));
    }
}
