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
	public partial class Master : ContentPage
	{
       
        Interface.IMyAPI myAPI = RestService.For<IMyAPI>("http://85.119.146.226/first/api");

        public Master ()
		{
			InitializeComponent ();










            bt_101f.Clicked += async (sender, e) => {

                App.MasterDetail.IsPresented = false;

                List<t_dates> datesList = new List<t_dates>();
                List<string> DL = new List<string>();

                datesList = await myAPI.t_dates("t_Data101");

                await App.MasterDetail.Detail.Navigation.PushAsync(new F101Page("0", datesList.First().dt));


            };
            bt_102f.Clicked += async (sender, e) =>
            {

                App.MasterDetail.IsPresented = false;
                List<t_dates> datesList = new List<t_dates>();
                List<string> DL = new List<string>();

                datesList = await myAPI.t_dates("t_Data102");
                await App.MasterDetail.Detail.Navigation.PushAsync(new F102Page("Part",-1,"0", datesList.First().dt, "0"));
            };

            bt_search.Clicked += async (sender, e) =>
            {

                App.MasterDetail.IsPresented = false;
                ///await App.MasterDetail.Detail.Navigation.PushAsync(new SearchPage());
                List<t_dates> datesList = new List<t_dates>();
                List<string> DL = new List<string>();

                datesList = await myAPI.t_dates("t_Data101");
                await App.MasterDetail.Detail.Navigation.PushAsync(new F101Page_group("0", datesList.First().dt));




            };



            Charts_JS.Clicked += async (sender, e) =>
            {

                App.MasterDetail.IsPresented = false;
               
                await App.MasterDetail.Detail.Navigation.PushAsync(new Page12());




            };



        }
	}
}