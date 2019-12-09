using App3.Interface;
using App3.Model;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App3.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchPage2 : ContentPage
	{
        List<BankInfo> bankinfos = new List<BankInfo>();

        IMyAPI myAPI = RestService.For<IMyAPI>("http://85.119.146.226/first/api");


        /*public async void Search_TextChanged(object sender, TextChangedEventArgs e)
        {

            bankinfos.Clear();

            bankinfos = await myAPI.Get_allbanks(Entry_Search.Text, 20);
            List<string> BankInfo_list_ShortName = new List<string>();

            foreach (var x in bankinfos)
                BankInfo_list_ShortName.Add(x.ShortName);

            ListView_banks.ItemsSource = BankInfo_list_ShortName;




        }*/



        public async void ListView_banks_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var b = bankinfos.Find(x => x.ShortName == e.Item.ToString());
            await Navigation.PushModalAsync(new Bank(b.ShortName, b.RegNum_date, b.IntCode.ToString(), b.RegNum));
        }



        public SearchPage2()
        {
            InitializeComponent();

        }

        private async void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ListView_banks.BeginRefresh();

            bankinfos.Clear();

            bankinfos = await myAPI.Get_allbanks(SearchBar1.Text, 20);
            List<string> BankInfo_list_ShortName = new List<string>();

            foreach (var x in bankinfos)
                BankInfo_list_ShortName.Add(x.ShortName);

            ListView_banks.ItemsSource = BankInfo_list_ShortName;
            ListView_banks.EndRefresh();
        }
    }
}