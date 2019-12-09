using App3.Interface;
using App3.Model;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace App3
{
    public partial class App : Application
    {


        

        public static MasterDetailPage MasterDetail { get; set; }

        public async Task NavigateMasterDetail(Page page)
        {
            await App.MasterDetail.Detail.Navigation.PushAsync( page);
           
        }

        public App()
        {
            

        InitializeComponent();
          
           
            MainPage = new MainPage();

            
           
           



        }

        protected override void OnStart()
        {
           

          
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }



        




    }
}
