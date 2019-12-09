using App3.Interface;
using App3.Model;
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

namespace App3.CS
{
    public class Graf
    {


        IMyAPI myAPI = RestService.For<IMyAPI>("http://85.119.146.226/first/api");

        ChartView chartView = new ChartView();
        public async void ChartV(string bank_name)
        {
            
            List<F101_actives_top_n> L = new List<F101_actives_top_n>();
            L = await myAPI.GetF101_bank_actives("2018-05-01", bank_name);
            var entries1 = new List<Microcharts.Entry>();

            foreach (var p in L)
            {

                var entry = new Entry(p.Val)
                {
                    //Label = p.Label,
                    //ValueLabel = p.Val.ToString(),
                    Color = SKColor.Parse("#77d065"),

                };
                entries1.Add(entry);
            }

            LineChart chart1 = new LineChart() { Entries = entries1, BackgroundColor = SKColor.Empty };
           
            chartView.Chart = chart1;

        }

        public ChartView Cv(string s)
        {
            ChartV(s);
            return chartView;
        }

           

    }
}
