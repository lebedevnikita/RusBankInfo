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
using App3.CS;

namespace App3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class F101Page_byIndCode : ContentPage 
	{
        Interface.IMyAPI myAPI = RestService.For<IMyAPI>("http://85.119.146.226/first/api");
        List<t_application_F101_allbanks> GetF101_data_List = new List<t_application_F101_allbanks>();
        List<t_application_F101_allbanks> GetF101_data_List_search_bar_filterd = new List<t_application_F101_allbanks>();
        List<BankInfo> bankinfos = new List<BankInfo>();

        List<t_dates> datesList = new List<t_dates>();
        List<string> DL = new List<string>();

        public int tip;
        public string str ="0" ;
        public string indCode = "0";
        public string filtered_bankname = "0";
        string dt_slice;

        public F101Page_byIndCode(int _tip, string _indCode, string dt_sl,string _filtered_bankname)
        {

            InitializeComponent();
           // DisplayAlert("Уведомление", _filtered_bankname, "ОK");
            Get_dates();
            tip = _tip;
            indCode = _indCode;
            filtered_bankname = _filtered_bankname;
            Getregn_info("bankinfo", "anyvalue", "0");


            F101_data(tip, indCode, dt_sl, dt_sl, filtered_bankname);


            if (tip == 4) { chart( tip, indCode, (DateTime.Parse(dt_sl).AddMonths(-12)).ToString("yyyy-MM-dd"), dt_sl, filtered_bankname); }
            else { chart( tip, indCode, (DateTime.Parse(dt_sl).AddMonths(-11)).ToString("yyyy-MM-dd"), dt_sl, filtered_bankname); }




            SearchBar1.IsVisible = false;
            ListView_SearchBar1.IsVisible = false;
            ListView_slice.IsVisible = false;
            Header_fieds_change(Label1_tip, "Name_Part", "Исходящие остатки");
            Header_fieds_change(Label2_bankname, "Name_Part", filtered_bankname);

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
        }








        public async void Getregn_info(string MODE, string shortname, string n)
        {
            bankinfos.Clear();
            bankinfos = await myAPI.Getbankinfo( MODE,  shortname,  n);


            Header_fieds_change(Label2_bankname, "Name_Part",  bankinfos.First().ShortName.ToString());

        }


        public async void F101_data(int tip, string str, string dt_from, string dt_to , string filtered_bankname)
        {
            GetF101_data_List.Clear();
            GetF101_data_List_search_bar_filterd.Clear();

            GetF101_data_List = await myAPI.GetF101_data("IndCode", tip, indCode,  dt_from,  dt_to);
            GetF101_data_List = GetF101_data_List.OrderBy(order => order.col_3).Reverse().ToList();

            if (filtered_bankname != "0" & filtered_bankname != "БАНКОВСКАЯ СИСТЕМА РФ")
            {
                //DisplayAlert("Уведомление", filtered_bankname, "ОK");
                GetF101_data_List_search_bar_filterd = GetF101_data_List.Where(x => x.regn == filtered_bankname).ToList();
                lw.ItemsSource = GetF101_data_List_search_bar_filterd;
            }
            else {
                lw.ItemsSource = GetF101_data_List.Where(x => x.regn!="0");
            }
 


           // lw.ItemsSource = GetF101_data_List;

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

            
            F101_data(tip, indCode, dt_slice, dt_slice, filtered_bankname);
            Header_fieds_change(Label1_tip, "Name_Part", item.Text);

            if (tip == 4) { chart(tip, indCode, (DateTime.Parse(dt_slice).AddMonths(-12)).ToString("yyyy-MM-dd"), dt_slice, filtered_bankname); }
            else { chart(tip, indCode, (DateTime.Parse(dt_slice).AddMonths(-11)).ToString("yyyy-MM-dd"), dt_slice, filtered_bankname); }


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



        private void ListView_SearchBar1_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            SearchBar1.IsVisible = false;
            ListView_SearchBar1.IsVisible = false;
            Grid_f101_header.IsVisible = true;
            lw.IsVisible = true;

            var b = bankinfos.Find(x => x.ShortName == e.Item.ToString());
            filtered_bankname  = b.ShortName.ToString();

            // Getregn_info("bankinfo", "anyvalue", str);

            //GetF101_data_List_search_bar_filterd.Clear();
            //GetF101_data_List_search_bar_filterd =GetF101_data_List.Where(x => x.regn == filtered_bankname).ToList();

            
          //  lw.ItemsSource = GetF101_data_List_search_bar_filterd;
            // F101_data(tip, str, dt_slice, dt_slice);
           

            if(filtered_bankname != "0" & filtered_bankname != "БАНКОВСКАЯ СИСТЕМА РФ")
            {
                //DisplayAlert("Уведомление", filtered_bankname, "ОK");
                GetF101_data_List_search_bar_filterd = GetF101_data_List.Where(x => x.regn == filtered_bankname).ToList();
                lw.ItemsSource = GetF101_data_List_search_bar_filterd;
            }
            else {
                lw.ItemsSource = GetF101_data_List;
            }

            Header_fieds_change(Label2_bankname, "Name_Part", b.ShortName);
            if (tip == 4) { chart(tip, indCode, (DateTime.Parse(dt_slice).AddMonths(-12)).ToString("yyyy-MM-dd"), dt_slice, b.ShortName); }
            else { chart(tip, indCode, (DateTime.Parse(dt_slice).AddMonths(-11)).ToString("yyyy-MM-dd"), dt_slice, b.ShortName); }



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



            F101_data(tip, indCode, dt_slice, dt_slice, filtered_bankname);
            if (tip == 4) { chart(tip, indCode, (DateTime.Parse(dt_slice).AddMonths(-12)).ToString("yyyy-MM-dd"), dt_slice, filtered_bankname); }
            else { chart(tip, indCode, (DateTime.Parse(dt_slice).AddMonths(-11)).ToString("yyyy-MM-dd"), dt_slice, filtered_bankname); }

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
           ((ListView)sender).SelectedItem = null;


            

        }






        public class cv_f101_dynamic_template
        {

            public string IndCode { get; set; }


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





        List<t_application_F101_allbanks> GetF101_data_List_chart = new List<t_application_F101_allbanks>();



        public async void chart( int tip, string indCode, string dt_from, string dt_to, string filtered_bankname)
        {
            
            GetF101_data_List_chart.Clear();
            GetF101_data_List_chart = await myAPI.GetF101_data("IndCode", tip, indCode, dt_from, dt_to);
            GetF101_data_List_chart = GetF101_data_List_chart.Where(x => x.regn == filtered_bankname)
                                                             .OrderBy(order => order.col_3).Reverse()
                                                             .ToList();

            Graf gr = new Graf();
            gr.Charts(GetF101_data_List_chart, tip, dt_from, dt_to);
            cv_f101_dynamic.ItemsSource = gr.cv_f101_dynamic_template_list;

        }




    }
    


}