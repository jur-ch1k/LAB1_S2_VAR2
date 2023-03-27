using CLASS_LIB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Documents;

namespace LAB1_S2_VAR2 {
    public class ViewData : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        //для RawData
        private int nodesnum;
        public int NodesNum { 
            get { return nodesnum;  }
            set {
                nodesnum = value;
                OnPropertyChanged("NodesNum");
            } 
        }
        private double lend;
        public double LEnd { 
            get { return lend;  } 
            set {
                lend = value;
                OnPropertyChanged("LEnd");
            } 
        }
        private double rend;
        public double REnd {
            get { return rend; }
            set {
                rend = value;
                OnPropertyChanged("REnd");
            }
        }

        private bool isuniform;
        public bool IsUnuform {
            get { return isuniform; }
            set {
                isuniform = value;
                OnPropertyChanged("IsUnuform");
            } 
        }
        private FRawEnum functype;
        public FRawEnum FuncType {
            get { return functype; }
            set {
                functype = value;
                OnPropertyChanged("FuncType");
            }
        }
        public RawData? Raw { get; set; }

        //для SplineData
        public int SplineNodesNum { get; set; }
        public double LeftSndDer { get; set; }
        public double RightSndDer { get; set; }
        private SplineData? spline;
        public SplineData? Spline {
            get { return spline; }
            set {
                spline = value;
                OnPropertyChanged("Spline");
            }
        }
        public void Save(string filename) {
            double[] Ends = new double[] { LEnd, REnd };
            RawData rawData =  new RawData(Ends, NodesNum, IsUnuform, FuncType);
            rawData.Save(filename);
        }
        public void Load(string filename) {
            RawData rawData = new RawData(filename);
            try {
                NodesNum = rawData.NumOfNodes;
                LEnd = rawData.Ends[0];
                REnd = rawData.Ends[1];
                IsUnuform = rawData.IsUnuform;
                FuncType = rawData.Func;
            }
            catch (Exception ex) {
                string messageBoxText = ex.ToString();
                string caption = "Ошибочка";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
            Raw = rawData;
        }
        public ViewData() {
            NodesNum = 6;
            LEnd = 0;
            REnd = 5;
            IsUnuform = true;
            FuncType = FRawEnum.Linear;

            SplineNodesNum = 11;
            LeftSndDer = 0;
            RightSndDer = 0;
            Raw = null;
            Spline = null;
        }

        public void CreateRawData() {
            double[] Ends = new double[] { LEnd, REnd };
            Raw = new RawData(Ends, NodesNum, IsUnuform, FuncType);
        }

        public void ExiquteSpline() {
            CreateRawData();
            SplineData splineData = new SplineData(Raw, LeftSndDer, RightSndDer, SplineNodesNum);
            splineData.Spline();
            Spline = splineData;
        }
        protected void OnPropertyChanged([CallerMemberName] string name = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    
}
