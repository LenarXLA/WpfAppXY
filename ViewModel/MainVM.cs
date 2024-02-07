using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CsvHelper;
using CsvHelper.Configuration;
using WpfAppXY.Model;
using WpfAppXY.Utilities;

namespace WpfAppXY.ViewModel
{
    public class MainVM : ViewModelBase
    {
        private string _oneDot;
        private string _twoDot;
        private string _threeDot;
        private string _fourDot;
        private string _fiveDot;
        private string _currentDot;
        private string _res;
        
        private int result;
        public ICommand GetDotsResult { get; set; }
        public ICommand GetDotsInCSV { get; set; } 
        
        public MainVM()
        {
            GetDotsResult = new RelayCommand(param => GetResult());
            GetDotsInCSV = new RelayCommand(param => GetDotsCSV());
        }

        public string Res
        {
            get { return _res; }
            set { _res = value; OnPropertyChanged(); }
        }
        
        public string OneDotValue
        {
            get { return _oneDot; }
            set { _oneDot = value; OnPropertyChanged(); }
        }
        
        public string TwoDotValue
        {
            get { return _twoDot; }
            set { _twoDot = value; OnPropertyChanged(); }
        }
        
        public string ThreeDotValue
        {
            get { return _threeDot; }
            set { _threeDot = value; OnPropertyChanged(); }
        }
        
        public string FourDotValue
        {
            get { return _fourDot; }
            set { _fourDot = value; OnPropertyChanged(); }
        }
        
        public string FiveDotValue
        {
            get { return _fiveDot; }
            set { _fiveDot = value; OnPropertyChanged(); }
        }
        
        public string CurrentDotValue
        {
            get { return _currentDot; }
            set { _currentDot = value; OnPropertyChanged(); }
        }
        
        private void GetResult()
        {
            if (string.IsNullOrEmpty(OneDotValue))
            {
                MessageBox.Show("Заполните координаты первой точки");
                return;
            }
            
            if (string.IsNullOrEmpty(TwoDotValue))
            {
                MessageBox.Show("Заполните координаты второй точки");
                return;
            }
            
            if (string.IsNullOrEmpty(ThreeDotValue))
            {
                MessageBox.Show("Заполните координаты третьей точки");
                return;
            }
            
            if (string.IsNullOrEmpty(FourDotValue))
            {
                MessageBox.Show("Заполните координаты четвертой точки");
                return;
            }
            
            if (string.IsNullOrEmpty(FiveDotValue))
            {
                MessageBox.Show("Заполните координаты пятой точки");
                return;
            }

            if (string.IsNullOrEmpty(CurrentDotValue))
            {
                MessageBox.Show("Заполните координаты проверяемой точки");
                return;
            }
            
            
            try
            {
                Dot dot1 = GetDot(OneDotValue);
                Dot dot2 = GetDot(TwoDotValue);
                Dot dot3 = GetDot(ThreeDotValue);
                Dot dot4 = GetDot(FourDotValue);
                Dot dot5 = GetDot(FiveDotValue);
                Dot dot6 = GetDot(CurrentDotValue);
                
                var xp = new int[] {dot1.DotX, dot2.DotX, dot3.DotX, dot4.DotX, dot5.DotX}; // Массив X-координат полигона
                var yp = new int[] {dot1.DotY, dot2.DotY, dot3.DotY, dot4.DotY, dot5.DotY}; // Массив Y-координат полигона

                result = inPoly(dot6.DotX, dot6.DotY, xp, yp);
                
            }
            catch (Exception ex)
            {
                // в логи
                MessageBox.Show(ex.Message.ToString());
            }

            if (result == 1)
            {
                Res = "точка внутри";
            }
            else
            {
                Res = "точка снаружи";
            }
            
            
        }
        
        // метод с вики
        private int inPoly( int x,int y, int[] xp, int[] yp){
            var npol = xp.Length;
            var j = npol - 1;
            var c = 0;
            for (var i = 0; i < npol;i++){
                if ((((yp[i]<=y) && (y<yp[j])) || ((yp[j]<=y) && (y<yp[i]))) &&
                    (x > (xp[j] - xp[i]) * (y - yp[i]) / (yp[j] - yp[i]) + xp[i]))
                {
                    c = 1 - c;
                }
                j = i;
            }
            return c;
        }

        
        // парсим вводимую строку на поинты
        private Dot GetDot(string val)
        {
            var arr = val.Split(",");
            return
                new Dot() { DotX = Convert.ToInt32(arr[0]), DotY = Convert.ToInt32(arr[1]) };
        }


        // Загружаем файлик CSV Test.csv в коренной директории приложения
        private void GetDotsCSV()
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            using (var reader = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}Test.csv"))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<Dot>();

                Dictionary<int, Dot> res = new Dictionary<int, Dot>();
                int key = 1;

                foreach (var record in records)
                {
                    res.Add(key, record);
                    key++;
                }
                
                OneDotValue = res[1].DotX + "," + res[1].DotY;
                TwoDotValue = res[2].DotX + "," + res[2].DotY;
                ThreeDotValue = res[3].DotX + "," + res[3].DotY;
                FourDotValue = res[4].DotX + "," + res[4].DotY;
                FiveDotValue = res[5].DotX + "," + res[5].DotY;
                CurrentDotValue = res[6].DotX + "," + res[6].DotY;
                    
            }
        }
    }
}