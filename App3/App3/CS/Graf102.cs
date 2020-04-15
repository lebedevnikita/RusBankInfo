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
    public partial class Graf102
    {
        

        public class cv_dynamic_template
        {

           // public string IndCode { get; set; }
            public Chart chart1 { get; set; }
            public string col_4 { get; set; }
            public Chart chart2 { get; set; }
            public Chart chart3 { get; set; }
        };




        public double div(double a, double b)
        {

            if (b != 0) { return a / b; }
            else { return 1; }
        }




        public List<cv_dynamic_template> cv_f101_dynamic_template_list
        {
            get { return _cv_f101_dynamic_template_list; }
            set { _cv_f101_dynamic_template_list = value; }
        } 





        private List<cv_dynamic_template> _cv_f101_dynamic_template_list = new List<cv_dynamic_template>();



        public  void Charts(List<Charts_dataset> _get_data_List_chart, int tip, string _dt_from, string _dt_to)
        {


         



            for (int i_zero_item = 0; i_zero_item <= 12; i_zero_item++)
            {
                var zero_item = new Charts_dataset
                {
                    val = 0,
                    dt = (DateTime.Parse(_dt_to).AddMonths(-i_zero_item)).ToString(),
       
                };

                _get_data_List_chart.Add(zero_item);
            }


            

            List<Charts_dataset> L = _get_data_List_chart
                                                          .GroupBy(g => DateTime.Parse(g.dt).ToString("MM.yy"))
                                                          .Select(g => new Charts_dataset
                                                          {
                                                              dt = DateTime.Parse(g.First().dt).ToString("MM.yy"),
                                                             
                                                              val = g.Sum(c => c.val),
                                                          })
                                                          .OrderBy(g => DateTime.Parse("01." + g.dt))
                                                          .ToList();

            
            
            float first_val = L[0].val.Value;
            float prev_val = 0;

            SKColor cl = SKColor.Empty; SKColor cl_line = SKColor.Empty;

            var entries12 = new List<Microcharts.Entry>();
            var entries6 = new List<Microcharts.Entry>();
            var entries3 = new List<Microcharts.Entry>();

            var entries12_line = new List<Microcharts.Entry>();
            var entries6_line = new List<Microcharts.Entry>();
            var entries3_line = new List<Microcharts.Entry>();


            long col_3_Value_for_12_line = 0;
            long col_3_Value_for_6_line = 0;
            long col_3_Value_for_3_line = 0;

            var entries12_for_labels = new List<Microcharts.Entry>();
            var entries6_for_labels = new List<Microcharts.Entry>();
            var entries3_for_labels = new List<Microcharts.Entry>();





            int cnt_dt = 1;


            
            foreach (var p in L)
            {
                //await DisplayAlert("123", p.dt.ToString(), "OK");

                if (cnt_dt == 1) { col_3_Value_for_12_line = p.val.Value; }
                else if (cnt_dt == 7) { col_3_Value_for_6_line = p.val.Value; }
                else if (cnt_dt == 10) { col_3_Value_for_3_line = p.val.Value; }

                cl = SKColor.Parse("#00d9fe");
                cl_line = SKColor.Parse("#2d96ff");


                var entry_chart1 = new Entry(p.val.Value)
                {
                    //Label = p.Label,
                    // ValueLabel = p.col_3.ToString(),
                    Label = p.dt,//(DateTime.Parse(p.dt)).ToString("MM.yy"),
                    Color = cl,

                };

                entries12.Add(entry_chart1);



                var entry_chart2_12_line = new Entry(col_3_Value_for_12_line)
                {

                    Label = p.dt,//(DateTime.Parse(p.dt)).ToString("MM.yy"),
                    Color = cl_line,

                };

                entries12_line.Add(entry_chart2_12_line);


                var entry_chart3_12 = new Entry(0)
                {


                    Color = SKColor.Parse("#ffffff"),
                    ValueLabel = p.val.Value.ToString(),

                };

                entries12_for_labels.Add(entry_chart3_12);





                if (cnt_dt > 6)
                {

                    entries6.Add(entry_chart1);
                    var entry_chart2_6_line = new Entry(col_3_Value_for_6_line)
                    {

                        Label = p.dt,//(DateTime.Parse(p.dt)).ToString("MM.yy"),
                        Color = cl_line,

                    };


                    entries6_line.Add(entry_chart2_6_line);


                    var entry_chart3_6 = new Entry(0)
                    {


                        Color = SKColor.Parse("#ffffff"),
                        ValueLabel = p.val.Value.ToString(),

                    };

                    entries6_for_labels.Add(entry_chart3_6);

                }




                if (cnt_dt > 9)
                {
                    entries3.Add(entry_chart1);
                    var entry_chart2_3_line = new Entry(col_3_Value_for_3_line)
                    {

                        Label = p.dt,//(DateTime.Parse(p.dt)).ToString("MM.yy"),
                        Color = cl_line,

                    };

                    entries3_line.Add(entry_chart2_3_line);


                    var entry_chart3_3 = new Entry(0)
                    {


                        Color = SKColor.Parse("#ffffff"),
                        ValueLabel = p.val.Value.ToString(),

                    };

                    entries3_for_labels.Add(entry_chart3_3);



                }


                prev_val = p.val.Value;
                cnt_dt++;

            }
            

            
            if (tip == 4)
            {

                entries12.RemoveAt(0);
                entries6.RemoveAt(0);
                entries3.RemoveAt(0);

                entries12_line.RemoveAt(0);
                entries6_line.RemoveAt(0);
                entries3_line.RemoveAt(0);

                entries12_for_labels.RemoveAt(0);
                entries6_for_labels.RemoveAt(0);
                entries3_for_labels.RemoveAt(0);

            }





            










            var chart12 = new BarChart()
            {
                Entries = entries12,
                BackgroundColor = SKColor.Empty,
                PointMode = PointMode.None
            };

            var chart6 = new BarChart()
            {
                Entries = entries6,
                BackgroundColor = SKColor.Empty,
                PointMode = PointMode.None,
                MaxValue = chart12.MaxValue
            };

            var chart3 = new BarChart()
            {
                Entries = entries3,
                BackgroundColor = SKColor.Empty,
                PointMode = PointMode.None,
                MaxValue = chart6.MaxValue

            };



            var chart12_line = new LineChart()
            {
                Entries = entries12_line,
                BackgroundColor = SKColor.Empty,
                LineMode = LineMode.Straight,
                MaxValue = chart12.MaxValue,
                LineAreaAlpha = 0,
                PointMode = PointMode.None,
            };

            var chart6_line = new LineChart()
            {
                Entries = entries6_line,
                BackgroundColor = SKColor.Empty,
                LineMode = LineMode.Straight,
                MaxValue = chart12.MaxValue,
                LineAreaAlpha = 0,
                PointMode = PointMode.None,
            };

            var chart3_line = new LineChart()
            {
                Entries = entries3_line,
                BackgroundColor = SKColor.Empty,
                LineMode = LineMode.Straight,
                MaxValue = chart6.MaxValue,
                LineAreaAlpha = 0,
                PointMode = PointMode.None,
            };





            



            var chart12_labels = new BarChart()
            {
                Entries = entries12_for_labels,
                BackgroundColor = SKColor.Empty,
                PointMode = PointMode.None,
                LabelTextSize = 20
            };

            var chart6_labels = new BarChart()
            {
                Entries = entries6_for_labels,
                BackgroundColor = SKColor.Empty,
                PointMode = PointMode.None,
                LabelTextSize = 30
            };

            var chart3_labels = new BarChart()
            {
                Entries = entries3_for_labels,
                BackgroundColor = SKColor.Empty,
                PointMode = PointMode.None,
                LabelTextSize = 40

            };


            


            int cnt_entries = entries12.Count - 1;
            string delta = Math.Round((entries12[cnt_entries].Value - entries12[0].Value) / 1000.0, 0).ToString();
            string percent = Math.Round((double)div(entries12[cnt_entries].Value, (double)entries12[0].Value) * 100 - 100, 0).ToString();
            //await DisplayAlert("123", "sdf", "OK");



             


            _cv_f101_dynamic_template_list.Add(new cv_dynamic_template()
           {
               //IndCode = str.ToString(),

               chart1 = chart12,
               col_4 = "изменение за 12 мес:" + delta + " (" + percent + "%)",
               chart2 = chart12_line,
               chart3 = chart12_labels,
           }
               );


           cnt_entries = entries6.Count - 1;
           delta = Math.Round((entries6[cnt_entries].Value - entries6[0].Value) / 1000.0, 0).ToString();
           percent = Math.Round((double)div(entries6[cnt_entries].Value, (double)entries6[0].Value) * 100 - 100, 0).ToString();



           _cv_f101_dynamic_template_list.Add(new cv_dynamic_template()
           {
              // IndCode = str.ToString(),

               chart1 = chart6,
               col_4 = "изменение за 6 мес:" + delta + " (" + percent + "%)",
               chart2 = chart6_line,
               chart3 = chart6_labels,
           }
            );







           cnt_entries = entries3.Count - 1;
           delta = Math.Round((entries3[cnt_entries].Value - entries3[0].Value) / 1000.0, 0).ToString();
           percent = Math.Round((double)div(entries3[cnt_entries].Value, (double)entries3[0].Value) * 100 - 100, 0).ToString();




           _cv_f101_dynamic_template_list.Add(new cv_dynamic_template()
           {
              // IndCode = str.ToString(),

               chart1 = chart3,
               col_4 = "изменение за 3 мес:" + delta + " (" + percent + "%)",
               chart2 = chart3_line,
               chart3 = chart3_labels,
           }
            );




          // cv_f101_dynamic.ItemsSource = cv_f101_dynamic_template_list;

           
        }





    }
}
