using App3.Interface;
using App3.Model;
using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entry = Microcharts.Entry;
using Microcharts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;

namespace App3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class F101Page_book_ap   : ContentPage 
	{
        Interface.IMyAPI myAPI = RestService.For<IMyAPI>("http://85.119.146.226/first/api");
        List<t_application_F101_allbanks> GetF101_data_List = new List<t_application_F101_allbanks>();
        List<t_application_F101_allbanks> GetF101_data_List_chart = new List<t_application_F101_allbanks>();
        List<BankInfo> bankinfos = new List<BankInfo>();

        List<t_dates> datesList = new List<t_dates>();
        List<string> DL = new List<string>();
        public string filtered_bankname = "0";
        public int tip = 4;
        public string str ="0" ;
        string dt_slice;
        string _pln;
        string _ap;
        


        public F101Page_book_ap(string regn_bank,string pln, string ap, string dt_sl)
        {

            InitializeComponent();

            Get_dates();



            Getregn_info("bankinfo", "anyvalue", regn_bank);
            F101_data(pln, ap, tip, regn_bank, dt_sl, dt_sl);

            chart(pln, ap, tip, regn_bank, "2018-12-31", "2019-11-30");

            SearchBar1.IsVisible = false;
            ListView_SearchBar1.IsVisible = false;
            ListView_slice.IsVisible = false;
            Header_fieds_change(Label1_tip, "Name_Part", "Исходящие остатки");
            _pln = pln;
            _ap = ap;
            //DisplayAlert("Уведомление", i_lw_ItemAppearing.ToString(), "ОK");


        }

        public class Header_fieds : INotifyPropertyChanged
        {
            private string name_part;
            public string Name_Part
            {
                get { return name_part; }
                set
                {
                    if (name_part != value)
                    {
                        name_part = value;
                        OnPropertyChanged("Name_Part");
                    }
                }
            }

          
            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged(string prop = "")
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }


        public void Header_fieds_change(Label field, string name_field,  string field_value)
        {
                var hf = new Header_fieds();
                hf.Name_Part = field_value;
                Binding bindingLabel1_tip = new Binding { Source = hf, Path = name_field };
                field.SetBinding(Label.TextProperty, bindingLabel1_tip);

                //filtered_bankname = field_value;
        }








        public async void Getregn_info(string MODE, string shortname, string n)
        {
            bankinfos.Clear();
            bankinfos = await myAPI.Getbankinfo( MODE,  shortname,  n);


            Header_fieds_change(Label2_bankname, "Name_Part",  bankinfos.First().ShortName.ToString());

        }


        public async void F101_data(string pln, string ap,int tip, string str, string dt_from, string dt_to)
        {
            
            GetF101_data_List.Clear();
            GetF101_data_List = await myAPI.GetF101_groups("group_to_Pln_Ap_IndCode"+ pln + ap, tip, str, dt_from, dt_to);
            GetF101_data_List = GetF101_data_List.OrderBy(order => order.IndCode).ToList();
            lw.ItemsSource = GetF101_data_List;

        }




        private void Clicked_getF101_data(object sender, EventArgs e)
        {

            ToolbarItem item =  ( ToolbarItem)sender;


            if (item.ClassId.ToString() == "Item1")
            {
                tip = 1;

            }
            else if (item.ClassId.ToString()== "Item2")
            {
                tip = 2;
            }
            else if (item.ClassId.ToString() == "Item3")
            {
                tip = 3;
            }
            else if (item.ClassId.ToString() == "Item4")
            {
                tip = 4;
            }

            // DisplayAlert("Уведомление", item.ClassId.ToString(), "ОK");
            // DisplayAlert("Уведомление", tip.ToString(), "ОK");
            


            F101_data(_pln, _ap, tip, str, dt_slice, dt_slice);
            Header_fieds_change(Label1_tip, "Name_Part", item.Text);
           
        }
      

        private void search_tap(object sender, EventArgs e)
        {


            SearchBar1.IsVisible = true;
            ListView_SearchBar1.IsVisible = true;
            SearchBar1.Text = "";
            ListView_SearchBar1.ItemsSource = null;
          
            Grid_f101_header.IsVisible = false;
            lw.IsVisible = false;


        }



        private  void ListView_SearchBar1_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            SearchBar1.IsVisible = false;
         
            ListView_SearchBar1.IsVisible = false;
            Grid_f101_header.IsVisible = true;
            lw.IsVisible = true;
            var b = bankinfos.Find(x => x.ShortName == e.Item.ToString());
            str = b.RegNumber.ToString();

            filtered_bankname = b.ShortName.ToString();
            Getregn_info("bankinfo", "anyvalue", str);
           
            F101_data(_pln, _ap, tip, str, dt_slice, dt_slice);
            Header_fieds_change(Label2_bankname, "Name_Part", b.ShortName);
            //DisplayAlert("Уведомление", _pln+"/"+ _ap + "/"+ tip.ToString() + "/"+ str + "/"+ dt_slice, "ОK");
        }




        private async void SearchBar1_OnTextChanged(object sender, TextChangedEventArgs e)
        {


            if (e.NewTextValue != "")
            {

                ListView_SearchBar1.BeginRefresh();
                bankinfos.Clear();
                //bankinfos = await myAPI.Get_allbanks(e.NewTextValue, 20);
                bankinfos = await myAPI.Getbankinfo("search_bar_withRF", e.NewTextValue, "20");

                List<string> BankInfo_list_ShortName = new List<string>();
                foreach (var x in bankinfos)
                    BankInfo_list_ShortName.Add(x.ShortName);

                ListView_SearchBar1.ItemsSource = BankInfo_list_ShortName;
                ListView_SearchBar1.EndRefresh();
            }

            else {
                ListView_SearchBar1.BeginRefresh();
                bankinfos.Clear();
                //bankinfos = await myAPI.Get_allbanks(e.NewTextValue, 20);
                bankinfos = await myAPI.Getbankinfo("bankinfo", "anyvalue", "0");

                List<string> BankInfo_list_ShortName = new List<string>();
                foreach (var x in bankinfos)
                    BankInfo_list_ShortName.Add(x.ShortName);

                ListView_SearchBar1.ItemsSource = BankInfo_list_ShortName;
                ListView_SearchBar1.EndRefresh();
            }

        }

       
   


        private async void lb_dynamic_clicked(object sender, EventArgs e)
        {
            await App.MasterDetail.Detail.Navigation.PushAsync(new F101Page_dynamic());
        }




       
        private async void Tbi_slice(object sender, EventArgs e)
        {

            ListView_slice.IsVisible = true;
            ListView_slice.ItemsSource = null;
            Grid_f101_header.IsVisible = false;
            lw.IsVisible = false;

            /*if (status_myAPI_t_dates== false)
            { 

                datesList = await myAPI.t_dates("t_Data101");
           
                foreach (var x in datesList)
                {
                    DL.Add( x.dt_mmmmYYYY);
                }

                
                status_myAPI_t_dates = true;
            }*/

            ListView_slice.ItemsSource = DL;

        }



        private void ListView_slice_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            ListView_slice.IsVisible = false;
            Grid_f101_header.IsVisible = true;
            lw.IsVisible = true;


             
            t_dates datesList_item = datesList.Find(x => x.dt_mmmmYYYY == e.Item.ToString());

            dt_slice = datesList_item.dt.ToString();
            slice.Text = datesList_item.dt_mmmmYYYY;



            F101_data(_pln, _ap, tip, str, dt_slice, dt_slice);




        }


        bool status_myAPI_t_dates = false;
        private async void Get_dates ()
        {
            if (status_myAPI_t_dates == false)
            {

                datesList = await myAPI.t_dates("t_Data101");

                foreach (var x in datesList)
                {
                    DL.Add(x.dt_mmmmYYYY);
                }


                status_myAPI_t_dates = true;
            }

            dt_slice = datesList.First().dt;
            slice.Text=datesList.Find(x => x.dt == dt_slice).dt_mmmmYYYY;



        }


        public async void grid_item_tapped(object sender, ItemTappedEventArgs e)
        {



            t_application_F101_allbanks selectedRow = e.Item as t_application_F101_allbanks;
            if (selectedRow != null)
                // await DisplayAlert(selectedRow.ap.ToString(), selectedRow.pln.ToString() , "OK");
                await App.MasterDetail.Detail.Navigation.PushAsync(new F101Page_byIndCode(tip, selectedRow.IndCode.ToString(),  datesList.First().dt, filtered_bankname));

            ((ListView)sender).SelectedItem = null;
        }


             public class cv_f101_dynamic_template
            {

                public string IndCode { get; set; }

                public float col_2 { get; set; }
                public Chart chart1 { get; set; }
                public string col_4 { get; set; }
                public Chart chart2 { get; set; }



            };


        public async void chart (string pln, string ap, int tip, string str, string dt_from, string dt_to)
        {
            List<cv_f101_dynamic_template> cv_f101_dynamic_template_list = new List<cv_f101_dynamic_template>();



            GetF101_data_List_chart.Clear();
            GetF101_data_List_chart = await myAPI.GetF101_groups("group_to_Pln_Ap_IndCode" + pln + ap, tip, str, dt_from, dt_to);
            //GetF101_data_List_chart = GetF101_data_List_chart.OrderBy(order => order.dt).ToList();


            List<t_application_F101_allbanks> L = GetF101_data_List_chart
                                                      .GroupBy(a => a.dt)
                                                      .Select(g=> new t_application_F101_allbanks
                                                      {
                                                          dt = g.First().dt,
                                                          regn =g.First().regn,
                                                          col_3 = g.Sum(c => c.col_3),
                                                      })
                                                     .OrderBy(a => a.dt)
                                                      .ToList();
       

   





              
                float first_val = L[0].col_3.Value;
                float prev_val = 0;
                string percent;
                SKColor cl = SKColor.Empty; SKColor cl_line = SKColor.Empty;
            var entries12 = new List<Microcharts.Entry>();
                var entries6 = new List<Microcharts.Entry>();
                var entries3 = new List<Microcharts.Entry>();

                var entries12_line = new List<Microcharts.Entry>();
                var entries6_line = new List<Microcharts.Entry>();
                var entries3_line = new List<Microcharts.Entry>();


            long col_3_Value_for_12_line=0;
            long col_3_Value_for_6_line=0;
            long col_3_Value_for_3_line = 0;


            int cnt_dt = 1;



            foreach (var p in L)
                {

                
                if (cnt_dt == 1) { col_3_Value_for_12_line = p.col_3.Value;  }
                else if (cnt_dt == 7) { col_3_Value_for_6_line = p.col_3.Value; }
                else if (cnt_dt == 10) { col_3_Value_for_3_line = p.col_3.Value; }

                   cl = SKColor.Parse("#00d9fe");
                    cl_line = SKColor.Parse("#2d96ff");


                var entry_chart1 = new Entry(p.col_3.Value)
                    {
                        //Label = p.Label,
                       // ValueLabel = p.col_3.ToString(),
                        Label = (DateTime.Parse(p.dt)).ToString("MM.yy"),
                        Color = cl,

                    };

                    entries12.Add(entry_chart1);



                    var entry_chart2_12_line = new Entry(col_3_Value_for_12_line)
                    {

                        Label = (DateTime.Parse(p.dt)).ToString("MM.yy"),
                        Color = cl_line,

                    };

                    entries12_line.Add(entry_chart2_12_line);


              

                    if (cnt_dt > 6) {

                        entries6.Add(entry_chart1);
                        var entry_chart2_6_line = new Entry(col_3_Value_for_6_line)
                            {

                                Label = (DateTime.Parse(p.dt)).ToString("MM.yy"),
                                Color = cl_line,

                            };

                        
                        entries6_line.Add(entry_chart2_6_line);

                    }
                    if (cnt_dt > 9) {
                        entries3.Add(entry_chart1);
                        var entry_chart2_3_line = new Entry(col_3_Value_for_3_line)
                            {

                                Label = (DateTime.Parse(p.dt)).ToString("MM.yy"),
                                Color = cl_line,

                            };
                       
                        entries3_line.Add(entry_chart2_3_line);
                     }


                 






                prev_val = p.col_3.Value;
                    cnt_dt++;




            }



                var chart12 = new BarChart()
                {
                    Entries = entries12,
                    BackgroundColor = SKColor.Empty,
                    PointMode = PointMode.None
                };

                var chart6= new BarChart()
                {
                    Entries = entries6,
                    BackgroundColor = SKColor.Empty,
                    PointMode = PointMode.None
                };

                var chart3 = new BarChart()
                {
                    Entries = entries3,
                    BackgroundColor = SKColor.Empty,
                    PointMode = PointMode.None
                };



            var chart12_line = new LineChart()
            {
                Entries = entries12_line,
                BackgroundColor = SKColor.Empty,
                LineMode = LineMode.Straight,
                MaxValue= chart12.MaxValue,
                LineAreaAlpha=0,
                PointMode = PointMode.None,
            };

            var chart6_line = new LineChart()
            {
                Entries = entries6_line,
                BackgroundColor = SKColor.Empty,
                LineMode = LineMode.Straight,
                MaxValue = chart6.MaxValue,
                LineAreaAlpha = 0,
                PointMode = PointMode.None,
            };

            var chart3_line = new LineChart()
            {
                Entries = entries3_line,
                BackgroundColor = SKColor.Empty,
                LineMode = LineMode.Straight,
                MaxValue = chart3.MaxValue,
                LineAreaAlpha = 0,
                PointMode = PointMode.None,
            };


            if (first_val != 0)
                { percent = Math.Round(prev_val * 100 / first_val - 100, 0).ToString() + "%"; }
            else { percent = "0%"; };

                cv_f101_dynamic_template_list.Add(new cv_f101_dynamic_template()
                    {
                        IndCode = str.ToString(),
                        col_2 = (float)Math.Round(first_val /1000,0),
                        chart1 = chart12,
                        col_4 = Math.Round(prev_val / 1000, 0).ToString() +" ("+ percent.ToString()+")",
                         chart2 = chart12_line,
                }   
                );

                cv_f101_dynamic_template_list.Add(new cv_f101_dynamic_template()
                {
                    IndCode = str.ToString(),
                    col_2 = (float)Math.Round(first_val / 1000, 0),
                    chart1 = chart6,
                    col_4 = Math.Round(prev_val / 1000, 0).ToString() + " (" + percent.ToString() + ")",
                     chart2 = chart6_line,
                }
                );

                cv_f101_dynamic_template_list.Add(new cv_f101_dynamic_template()
                {
                    IndCode = str.ToString(),
                    col_2 = (float)Math.Round(first_val / 1000, 0),
                    chart1 = chart3,
                    col_4 = Math.Round(prev_val / 1000, 0).ToString() + " (" + percent.ToString() + ")",
                     chart2 = chart3_line,
                }
                );




            cv_f101_dynamic.ItemsSource = cv_f101_dynamic_template_list;
            
            //  cv_f101_dynamic.HeightRequest = (int)Math.Round(cv_f101_dynamic.Height,0);



            /*
            await DisplayAlert( "Check",
                str.ToString()+ "/" + 
                (first_val / 1000).ToString()+ "/"+ 
                (prev_val / 1000).ToString()

                 
                
                , "OK"
                );*/
        }

  
        public double currentItemIndex;
        public double prevItemIndex =0;
        public bool scroll_UP;




   
        private  void lw_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {

            t_application_F101_allbanks item = e.Item as t_application_F101_allbanks;
            currentItemIndex = GetF101_data_List.IndexOf(item);

            if (currentItemIndex > prevItemIndex)
            {
                scroll_UP = true;
            }
            else
            {
                scroll_UP = false;
            }

            if (currentItemIndex > 20  & scroll_UP == true)
            {

                cv_f101_dynamic.IsVisible = false;

            }

            else if (currentItemIndex ==0 )
            {

                cv_f101_dynamic.IsVisible = true;
                      
               

            }

            
            /*DisplayAlert("Check",
                          Math.Round(Math.Pow(currentItemIndex, 1.8), 0).ToString() , "OK"
                           );*/

            prevItemIndex = currentItemIndex;





        }
    }
   


}