using App3.Interface;
using App3.Model;
using App3.CS;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entry = Microcharts.Entry;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using Microcharts;
using Microcharts.Forms;

namespace App3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class F101_actives_page : ContentPage
    {









        DatePicker _picker1 = new DatePicker
        {
            Format = "D",
            MaximumDate = DateTime.Parse("2018-12-31"),
            MinimumDate = DateTime.Now.AddMonths(-12),
            IsVisible = false,
        };
        

      
        static string d2 = "2018-12-31";
        string d1 = (DateTime.Parse(d2).AddMonths(-11)).ToString("yyyy-MM-dd");
        int topN = 30;


        IMyAPI myAPI = RestService.For<IMyAPI>("http://85.119.146.226/first/api");
        List<F101_actives_top_n> GetF101_actives_top_n_List = new List<F101_actives_top_n>();
        List<string> GetF101_actives_top_n_List_string = new List<string>();
        

        public async void GetF101_top_n(string d1, string d2, int n)
        {
            var ch = Grid_F101_actives_page.Children.ToList();

            foreach (var p in ch)
            {
                Grid_F101_actives_page.Children.RemoveAt(Grid_F101_actives_page.Children.IndexOf(p));
            }




            int i = 0;
            GetF101_actives_top_n_List = await myAPI.GetF101_actives_top_n(d2, d1, n);
            List<F101_actives_top_n> bank_names = GetF101_actives_top_n_List
                                              .GroupBy(a => a.ShortName)
                                              .Select(g => g.First())
                                              .ToList();

           // Grid_F101_actives_page.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
           // Grid_F101_actives_page.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });


            foreach (var x in bank_names)
            {

                List<F101_actives_top_n> L = GetF101_actives_top_n_List
                                                .Where(a => a.ShortName.Equals(x.ShortName))
                                                .ToList();
    
                    float prev_val = 0;
                    SKColor cl = SKColor.Empty;
                    var entries1 = new List<Microcharts.Entry>();

                        foreach (var p in L)
                        {
                            if (p.Val >= prev_val) { cl = SKColor.Parse("#A8E10C"); } else { cl = SKColor.Parse("#FF5765"); }
                            var entry = new Entry(p.Val)
                            {
                                //Label = p.Label,
                                //ValueLabel = p.Val.ToString(),
                   
                                    Color = cl,

                            };
                            prev_val = p.Val;
                            entries1.Add(entry);
                        }



                    var chart1 = new LineChart() { Entries = entries1, BackgroundColor = SKColor.Empty };

                    var chartView = new ChartView
                    {

                        Chart = chart1
                    };

                Grid_F101_actives_page.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });

                var _1 = new Label { Text = "№ "+(i+1).ToString() + (char)10 +x.ShortName + (char)10 + Math.Round(prev_val/1000000000,1).ToString()+" млрд.руб." };
                Grid_F101_actives_page.Children.Add(_1, 0, i);
                Grid_F101_actives_page.Children.Add(chartView, 1, i);
                   
                    i++;
               
            }




            

            Grid_F101_actives_page.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
            Grid_F101_actives_page.Children.Add(_picker1, 0, i+1);

        
            


        }
        








        public F101_actives_page()
	    {
		    InitializeComponent ();
            tbi_date_slice.Text = d2.ToString();
            GetF101_top_n(d1, d2, topN);

            _picker1.DateSelected += datePicker_DateSelected1;
          


        }


        private void Tbi_Clicked1(object sender, EventArgs e)
        {

            _picker1.Focus();

            
        }

        private void datePicker_DateSelected1(object sender, DateChangedEventArgs e)
        {
            d2=e.NewDate.ToString("yyyy-MM-dd");
            d1 = (DateTime.Parse(d2).AddMonths(-11)).ToString("yyyy-MM-dd");
            tbi_date_slice.Text = d2.ToString();
            GetF101_top_n(d1, d2, topN);
        }

        private void tbi_topN30_Clicked(object sender, EventArgs e)
        {
            topN = 30;
            GetF101_top_n(d1, d2, topN);
        }

        private void tbi_topN100_Clicked(object sender, EventArgs e)
        {
            topN = 100;
            GetF101_top_n(d1, d2, topN);
        }

        private void tbi_topN200_Clicked(object sender, EventArgs e)
        {
            topN = 200;
            GetF101_top_n(d1, d2, topN);
        }
    }
}