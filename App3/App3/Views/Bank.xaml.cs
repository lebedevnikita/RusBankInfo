using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entry = Microcharts.Entry;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Refit;
using App3.Interface;
using App3.Model;
using Microcharts;
using SkiaSharp;

namespace App3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Bank : ContentPage
    {


        Interface.IMyAPI myAPI = RestService.For<IMyAPI>("http://85.119.146.226/first/api");
        List<F101_actives> GetF101_actives_List = new List<F101_actives>();

        public async void GetF101()
        {

            GetF101_actives_List = await myAPI.GetF101_actives("2018-05-01");
            var entries = new List<Microcharts.Entry>();

            GetF101_actives_List = GetF101_actives_List.OrderBy(order => order.Label).ToList();

            float prev_val = 0;
            SKColor cl = SKColor.Empty;
            foreach (var p in GetF101_actives_List)
            {
                if (p.Val >= prev_val) { cl = SKColor.Parse("#A8E10C"); } else { cl = SKColor.Parse("#FF5765"); }
                var entry = new Entry(p.Val)
                {
                    Label = (DateTime.Parse(p.Label).AddMonths(-11)).ToString("MMM-yyyy"),
                    //ValueLabel = p.Val.ToString(),
                    Color = cl
                };
                entries.Add(entry);
            }
            
            var chart = new LineChart() { Entries = entries };

            this.chartView.Chart = chart;


        }



        




        public Bank(string label_ShortName, string label_RegNum_date, string label_IntCode, string label_RegNumber)
        {
            InitializeComponent();

            Label_ShortName.Text = label_ShortName;
            RegNum_date.Text = label_RegNum_date;
            IntCode.Text = label_IntCode;
            RegNumber.Text = label_RegNumber;

            GetF101();
            

        }


     

    }

}






    




