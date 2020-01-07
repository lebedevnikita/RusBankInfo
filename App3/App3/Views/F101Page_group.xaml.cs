using App3.Interface;
using App3.Model;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class F101Page_group : ContentPage
    {
        
        Interface.IMyAPI myAPI = RestService.For<IMyAPI>("http://85.119.146.226/first/api");

        List<t_application_F101_allbanks> GetF101_data_List = new List<t_application_F101_allbanks>();
      
        List<BankInfo> bankinfos = new List<BankInfo>();

        List<t_dates> datesList = new List<t_dates>();
        List<string> DL = new List<string>();

        public int tip = 4;
        public string str = "0";
        string dt_slice;


        public ObservableCollection<Grouping<string, t_application_F101_allbanks>> F101_groups { get; set; }

        public F101Page_group(string regn_bank, string dt_sl)
        {
            InitializeComponent();

            
            Get_dates();



            Getregn_info("bankinfo", "anyvalue", "0");
            F101_data(tip, regn_bank, dt_sl, dt_sl);

            SearchBar1.IsVisible = false;
            ListView_SearchBar1.IsVisible = false;
            ListView_slice.IsVisible = false;
            Header_fieds_change(Label1_tip, "Name_Part", "Исходящие остатки тыс.р.");
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
            hf.Name_Part = field_value ;
            Binding bindingLabel1_tip = new Binding { Source = hf, Path = name_field };
            field.SetBinding(Label.TextProperty, bindingLabel1_tip);
        }








        public async void Getregn_info(string MODE, string shortname, string n)
        {
            bankinfos.Clear();
            bankinfos = await myAPI.Getbankinfo(MODE, shortname, n);
            Header_fieds_change(Label2_bankname, "Name_Part", bankinfos.First().ShortName.ToString());
        }


        public async void F101_data(int tip, string str, string dt_from, string dt_to)
        {
            GetF101_data_List.Clear();
            GetF101_data_List = await myAPI.GetF101_groups("group_to_Pln_Ap", tip, str, dt_from, dt_to);
            GetF101_data_List = GetF101_data_List

                                    .OrderBy(order => order.pln)
                                    .OrderBy(order => order.ap)
                                    .ToList();

            var groups = GetF101_data_List.GroupBy(p => p.pln).Select(g => new Grouping<string, t_application_F101_allbanks>(g.Key, g));
            F101_groups = new ObservableCollection<Grouping<string, t_application_F101_allbanks>>(groups);
            this.BindingContext = this;

            //lw.ItemsSource = GetF101_data_List;
            lw.ItemsSource = F101_groups;
           
        }

        


        private void Clicked_getF101_data(object sender, EventArgs e)
        {

            ToolbarItem item = (ToolbarItem)sender;


            if (item.ClassId.ToString() == "Item1")
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


            F101_data(tip, str, dt_slice, dt_slice);
            Header_fieds_change(Label1_tip , "Name_Part", item.Text +" тыс.р.");

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
            str = b.RegNumber.ToString();

            Getregn_info("bankinfo", "anyvalue", str);
            F101_data(tip, str, dt_slice, dt_slice);
            Header_fieds_change(Label2_bankname, "Name_Part", b.ShortName);

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



            F101_data(tip, str, dt_slice, dt_slice);




        }


        bool status_myAPI_t_dates = false;
        private async void Get_dates()
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
            slice.Text = datesList.Find(x => x.dt == dt_slice).dt_mmmmYYYY;



        }



        public class Grouping<K, T> : ObservableCollection<T>
        {
            public K Name { get; private set; }
            public Grouping(K name, IEnumerable<T> items)
            {
                Name = name;
                foreach (T item in items)
                    Items.Add(item);
            }
        }

        public async void grid_item_tapped(object sender, ItemTappedEventArgs e)
        {

           

            t_application_F101_allbanks selectedRow = e.Item as t_application_F101_allbanks;
                if (selectedRow != null)
                   // await DisplayAlert(selectedRow.ap.ToString(), selectedRow.pln.ToString() , "OK");
                await App.MasterDetail.Detail.Navigation.PushAsync(new F101Page_book_ap(str, selectedRow.pln.ToString(), selectedRow.ap.ToString(), datesList.First().dt));


            ((ListView)sender).SelectedItem = null;
        


        }
        
       /* private void grid_item_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
               // ((ListView)sender).BackgroundColor = Color.Red;
            ((ListView)sender).SelectedItem = null;
        }
        */
    }
}