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
    public partial class F1022Page : ContentPage
    {
        Interface.IMyAPI myAPI = RestService.For<IMyAPI>("http://85.119.146.226/first/api");
        List<Dataset_F102> GetF102_data_List = new List<Dataset_F102>();
        List<Dataset_F102> GetF102_data_List_search_bar_filterd = new List<Dataset_F102>();
        List<BankInfo> bankinfos = new List<BankInfo>();

        List<t_dates> datesList = new List<t_dates>();
        List<string> DL = new List<string>();

        public int field_id;
        //public string str = "0";
        public string regn = "0";
        public string slice = "0";
        public string filtered_bankname = "0";
        string dt_slice;

        public F102Page( string _slice, int _field_id, string _regn, string dt_sl, string _filtered_bankname)
        {
          // NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            // DisplayAlert("Уведомление", _filtered_bankname, "ОK");
            Get_dates();
            slice = _slice;
            field_id = _field_id;
            regn = _regn;
            filtered_bankname = _filtered_bankname;
            Getregn_info("bankinfo", "anyvalue", regn);
           // this.Title = slice;
            this.ResDic["page_slice"] = slice;
            F102_data(slice, field_id, regn, dt_sl, dt_sl, filtered_bankname);
            
            chart(slice, field_id, regn, (DateTime.Parse(dt_sl).AddMonths(-11)).ToString("yyyy-MM-dd"), dt_sl, filtered_bankname); 

            



            SearchBar1.IsVisible = false;
            ListView_SearchBar1.IsVisible = false;
            ListView_slice.IsVisible = false;
            Header_fieds_change(Label1_tip, "Name_Part", "Исходящие остатки");
            Header_fieds_change(Label2_bankname, "Name_Part", filtered_bankname);

        }



        public async void F102_data(string _slice, int _field_id, string _regn, string _dt_from, string _dt_to, string _filtered_bankname)
        {
            GetF102_data_List.Clear();
            GetF102_data_List_search_bar_filterd.Clear();

            GetF102_data_List = await myAPI.GetF102_data(_slice, _field_id, _regn, _dt_from, _dt_to);
            GetF102_data_List = GetF102_data_List.OrderBy(order => order.FieldName).ToList();


            /*if (filtered_bankname != "0" & filtered_bankname != "БАНКОВСКАЯ СИСТЕМА РФ")
            {
                //DisplayAlert("Уведомление", filtered_bankname, "ОK");
                GetF102_data_List_search_bar_filterd = GetF102_data_List.Where(x => x.regn == _filtered_bankname).ToList();
                lw.ItemsSource = GetF102_data_List_search_bar_filterd;
            }
            else
            {
                lw.ItemsSource = GetF102_data_List.Where(x => x.regn == "0");
            }
            */


            lw.ItemsSource = GetF102_data_List;
            //await DisplayAlert("Уведомление", filtered_bankname, "ОK");


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


        public void Header_fieds_change(Label field, string name_field, string field_value)
        {
            var hf = new Header_fieds();
            hf.Name_Part = field_value;
            Binding bindingLabel1_tip = new Binding { Source = hf, Path = name_field };
            field.SetBinding(Label.TextProperty, bindingLabel1_tip);
        }








        public async void Getregn_info(string MODE, string shortname, string n)
        {
            bankinfos.Clear();
            bankinfos = await myAPI.Getbankinfo(MODE, shortname, n);


            Header_fieds_change(Label2_bankname, "Name_Part", bankinfos.First().ShortName.ToString());

        }


        




        private void Clicked_getF102_data(object sender, EventArgs e)
        {

            ToolbarItem item = (ToolbarItem)sender;


          /*  if (item.ClassId.ToString() == "Item1")
            {
                tip = 1;

            }
            else if (item.ClassId.ToString() == "Item2")
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
            */
        }







        private void search_tap(object sender, EventArgs e)
        {


            SearchBar1.IsVisible = true;
            ListView_SearchBar1.IsVisible = true;
            SearchBar1.Text = "";
            ListView_SearchBar1.ItemsSource = null;

            Grid_f102_header.IsVisible = false;
            lw.IsVisible = false;


        }



        private void ListView_SearchBar1_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            SearchBar1.IsVisible = false;
            ListView_SearchBar1.IsVisible = false;
            Grid_f102_header.IsVisible = true;
            lw.IsVisible = true;

            var b = bankinfos.Find(x => x.ShortName == e.Item.ToString());
            regn = b.RegNumber.ToString();
            filtered_bankname = b.ShortName;
            Getregn_info("bankinfo", "anyvalue", regn);
            F102_data(slice, field_id, regn, dt_slice, dt_slice, filtered_bankname);
            Header_fieds_change(Label2_bankname, "Name_Part", filtered_bankname);


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

            else
            {
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
            Grid_f102_header.IsVisible = false;
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
            Grid_f102_header.IsVisible = true;
            lw.IsVisible = true;



            t_dates datesList_item = datesList.Find(x => x.dt_mmmmYYYY == e.Item.ToString());

            dt_slice = datesList_item.dt.ToString();
            date_slice.Text = datesList_item.dt_mmmmYYYY;


            F102_data(slice, field_id, regn, dt_slice, dt_slice, filtered_bankname);
            /*
            if (tip == 4) { chart(tip, indCode, (DateTime.Parse(dt_slice).AddMonths(-12)).ToString("yyyy-MM-dd"), dt_slice, filtered_bankname); }
            else { chart(tip, indCode, (DateTime.Parse(dt_slice).AddMonths(-11)).ToString("yyyy-MM-dd"), dt_slice, filtered_bankname); }
            */
        }


        bool status_myAPI_t_dates = false;
        private async void Get_dates()
        {
            if (status_myAPI_t_dates == false)
            {

                datesList = await myAPI.t_dates("t_Data102");

                foreach (var x in datesList)
                {
                    DL.Add(x.dt_mmmmYYYY);
                }


                status_myAPI_t_dates = true;
            }

            dt_slice = datesList.First().dt;
            date_slice.Text = datesList.Find(x => x.dt == dt_slice).dt_mmmmYYYY;



        }


        

        public async void grid_item_tapped(object sender, ItemTappedEventArgs e)
        {
            Dataset_F102 selectedRow = e.Item as Dataset_F102;
            // if (selectedRow != null)

            // await App.MasterDetail.Detail.Navigation.PushAsync(new F101Page_byIndCode(tip, selectedRow.IndCode.ToString(), datesList.First().dt, selectedRow.regn));

            SearchBar1.IsVisible = false;
            ListView_SearchBar1.IsVisible = false;
            Grid_f102_header.IsVisible = true;
            lw.IsVisible = true;
            slice = (string)ResDic["page_slice"];
            if (slice == "Part")
            { slice = "Section_by_Part" + selectedRow.Field_id.ToString(); }
            else if (slice.Contains("Section_by_Part"))
            { slice = "Article_by_Section" + selectedRow.Field_id.ToString(); }
            else if (slice.Contains("Article_by_Section"))
            { slice = "SymbCode_by_Article" + selectedRow.Field_id.ToString(); }
            else if (slice.Contains("SymbCode_by_Article"))
            { slice = "Banks_by_symbol" + selectedRow.Field_id.ToString(); }
            else if (slice.Contains("Banks_by_symbol"))
            { slice = "Exit_"; }

            


            if (!slice.Contains("Exit_"))
            { await App.MasterDetail.Detail.Navigation.PushAsync(new F102Page(slice, -1, regn, dt_slice, filtered_bankname)); }
            
            
            
            
            
            
            // DisplayAlert("Уведомление", selectedRow.Field_id.ToString(), "ОK");


            /*if (filtered_bankname != "0" & filtered_bankname != "БАНКОВСКАЯ СИСТЕМА РФ")
            {
                //DisplayAlert("Уведомление", filtered_bankname, "ОK");
                GetF102_data_List_search_bar_filterd = GetF102_data_List.Where(x => x.regn == filtered_bankname).ToList();
                lw.ItemsSource = GetF102_data_List_search_bar_filterd;
            }
            else
            {
                lw.ItemsSource = GetF102_data_List;
            }*/

            /*Header_fieds_change(Label2_bankname, "Name_Part", filtered_bankname);
            if (tip == 4) { chart(tip, indCode, (DateTime.Parse(dt_slice).AddMonths(-12)).ToString("yyyy-MM-dd"), dt_slice, filtered_bankname); }
            else { chart(tip, indCode, (DateTime.Parse(dt_slice).AddMonths(-11)).ToString("yyyy-MM-dd"), dt_slice, filtered_bankname); }
            */










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





        List<Dataset_F102> GetF102_data_List_chart = new List<Dataset_F102>();
        //List<Charts_dataset> dataset_for_charts = new List<Charts_dataset>();


        public async void chart(string _slice, int _field_id, string _regn, string _dt_from, string _dt_to, string _filtered_bankname)
        {

            GetF102_data_List_chart.Clear();
            GetF102_data_List_chart = await myAPI.GetF102_data(_slice, _field_id, _regn, _dt_from, _dt_to);
            GetF102_data_List_chart = GetF102_data_List_chart.OrderBy(order => order.dt).Reverse()
                                                             .ToList();
            List<Charts_dataset> dataset_for_charts = GetF102_data_List_chart.Select(i=> new Charts_dataset { dt=i.dt.ToString(), val=i.val }).ToList();
                                                                           



            Graf102 gr = new Graf102();
            gr.Charts(dataset_for_charts, 1, _dt_from, _dt_to);
            cv_f102_dynamic.ItemsSource = gr.cv_f101_dynamic_template_list;

        }


        public double currentItemIndex;
        public double prevItemIndex = 0;
        public bool scroll_UP;

        private void lw_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {

            Dataset_F102 item = e.Item as Dataset_F102;
            currentItemIndex = GetF102_data_List.IndexOf(item);

            if (currentItemIndex > prevItemIndex)
            {
                scroll_UP = true;
            }
            else
            {
                scroll_UP = false;
            }

            if (currentItemIndex > 20 & scroll_UP == true)
            {

                cv_f102_dynamic.IsVisible = false;

            }

            else if (currentItemIndex == 0)
            {

                cv_f102_dynamic.IsVisible = true;



            }


            /*DisplayAlert("Check",
                          Math.Round(Math.Pow(currentItemIndex, 1.8), 0).ToString() , "OK"
                           );*/

            prevItemIndex = currentItemIndex;





        }

    }



}