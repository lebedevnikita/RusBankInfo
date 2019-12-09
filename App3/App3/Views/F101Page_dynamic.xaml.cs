using App3.Interface;
using App3.Model;
using Refit;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entry = Microcharts.Entry;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;
using Microcharts.Forms;

namespace App3.Views
{







    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class F101Page_dynamic : ContentPage
    {
        public class lw_f101_dynamic_template
        {

            public string IndCode { get; set; }
            
            public float col_2 { get; set; }
            public LineChart linechart { get; set; }
            public float col_4 { get; set; }
            public string col_5 { get; set; }

        }

        Interface.IMyAPI myAPI = RestService.For<IMyAPI>("http://85.119.146.226/first/api");
        List<t_application_F101_allbanks> GetF101_List_dynamic = new List<t_application_F101_allbanks>();
       
        List<lw_f101_dynamic_template> lw_f101_dynamic_template_list = new List<lw_f101_dynamic_template>();








        public async void test()
        {List<lw_f101_dynamic_template> lw_f101_dynamic_template_list = new List<lw_f101_dynamic_template>();


            GetF101_List_dynamic = await myAPI.GetF101_data("regn",1,"0","2018-01-31", "2019-01-31");
            GetF101_List_dynamic = GetF101_List_dynamic.OrderBy(order => order.IndCode).ToList();

            List<t_application_F101_allbanks> L = GetF101_List_dynamic
                                                      .GroupBy(a => a.IndCode)
                                                      .Select(g => g.First())
                                                      .ToList();
            int n = 0;
            foreach (var x in L)
            {

                List<t_application_F101_allbanks> data = GetF101_List_dynamic
                                                        .Where(a => a.IndCode.Equals(x.IndCode))
                                                        .OrderBy(order => order.dt)
                                                        .ToList();

                
                float prev_val = 0;
                float first_val= data[0].col_3.Value ;
                string percent;
                SKColor cl = SKColor.Empty;
                var entries1 = new List<Microcharts.Entry>();

                foreach (var p in data)
                {
                   

                    if (p.col_3 >= prev_val) { cl = SKColor.Parse("#A8E10C"); } else { cl = SKColor.Parse("#FF5765"); }
                    var entry = new Entry(p.col_3.Value)
                    {
                        //Label = p.Label,
                        //ValueLabel = p.Val.ToString(),

                        Color = cl,

                    };
                    prev_val = p.col_3.Value;
                    entries1.Add(entry);
                }



                var chart1 = new LineChart() {
                                                Entries = entries1,
                                                BackgroundColor = SKColor.Empty ,
                                                PointMode =PointMode.None,
                                                LineMode=LineMode.Straight
                                                

                                             };

                if (first_val != 0)
                { percent = Math.Round(prev_val * 100 / first_val -100, 0).ToString() + "%"; }
                else { percent = "0%"; };
               
                lw_f101_dynamic_template_list.Add(new lw_f101_dynamic_template() { IndCode = x.IndCode.ToString(),
                                                                                   col_2 = first_val/1000,
                                                                                   linechart = chart1,
                                                                                   col_4 = prev_val/1000,
                                                                                   col_5= percent
                }
                );
              
            }


            lw_f101_dynamic.ItemsSource = lw_f101_dynamic_template_list;
            lw_f101_dynamic.RowHeight = 70;

        }







        /*

        public async void GetF101_allbanks_td()
        {
            GetF101_allbanks_incoming_balance_List_dynamic = await myAPI.GetF101_allbanks_turnover_deb("2018-01-31", "2019-01-31");
            GetF101_allbanks_incoming_balance_List_dynamic = GetF101_allbanks_incoming_balance_List_dynamic.OrderBy(order => order.IndCode).ToList();
            lw.ItemsSource = GetF101_allbanks_incoming_balance_List_dynamic;

        }

        public async void GetF101_allbanks_tc()
        {
            GetF101_allbanks_incoming_balance_List_dynamic = await myAPI.GetF101_allbanks_turnover_cred("2018-01-31", "2019-01-31");
            GetF101_allbanks_incoming_balance_List_dynamic = GetF101_allbanks_incoming_balance_List_dynamic.OrderBy(order => order.IndCode).ToList();
            lw.ItemsSource = GetF101_allbanks_incoming_balance_List_dynamic;

        }

        public async void GetF101_allbanks_ob()
        {
            GetF101_allbanks_incoming_balance_List_dynamic = await myAPI.GetF101_allbanks_outcoming_balance("2018-01-31", "2019-01-31");
            GetF101_allbanks_incoming_balance_List_dynamic = GetF101_allbanks_incoming_balance_List_dynamic.OrderBy(order => order.IndCode).ToList();
            lw.ItemsSource = GetF101_allbanks_incoming_balance_List_dynamic;

        }
        */



        public F101Page_dynamic()
		{

     
            InitializeComponent ();
           test();



        }



    }
}