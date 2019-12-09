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

namespace App3.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Detail : ContentPage
	{



        Interface.IMyAPI myAPI = RestService.For<IMyAPI>("http://85.119.146.226/first/api");
       


        float prev_val = 0;
        public List<F101_actives> GetF101_actives_List = new List<F101_actives>();
        public async void GetF101_chart()
        {

            GetF101_actives_List = await myAPI.GetF101_actives("2018-05-01");
            var entries1 = new List<Microcharts.Entry>();

            GetF101_actives_List= GetF101_actives_List.OrderBy(order=>order.Label).ToList();

            SKColor cl = SKColor.Empty;
            foreach (var p in GetF101_actives_List)
            {
                if (p.Val >= prev_val) { cl = SKColor.Parse("#A8E10C"); } else { cl = SKColor.Parse("#FF5765"); }
                var entry = new Entry(p.Val)
                {
                    Label = (DateTime.Parse(p.Label).AddMonths(-11)).ToString("MMM-yyyy"),
                    
                //ValueLabel = p.Val.ToString(),
                Color = cl,
                    
                };

                entries1.Add(entry);
            }

            var chart1 = new LineChart() { Entries = entries1 , BackgroundColor = SKColor.Empty };

          


            this.ChartView_chart1.Chart = chart1;

        }




        List<F101_actives_top_n> GetF101_actives_top_n_List = new List<F101_actives_top_n>();
        List<string> GetF101_actives_top_n_List_string = new List<string>();

        List<F101_passives_top_n> GetF101_passives_top_n_List = new List<F101_passives_top_n>();
        List<string> GetF101_passives_top_n_List_string = new List<string>();
        public async void GetF101_top_n()
        {

            GetF101_actives_top_n_List = await myAPI.GetF101_actives_top_n("2019-01-31", "2019-01-31",20);
            foreach (var x in GetF101_actives_top_n_List)
                GetF101_actives_top_n_List_string.Add(x.ShortName);

            ListView_top30_actives.ItemsSource = GetF101_actives_top_n_List_string;

            GetF101_passives_top_n_List = await myAPI.GetF101_passives_top_n("2019-01-31", "2019-01-31", 20);
            foreach (var x in GetF101_passives_top_n_List)
                GetF101_passives_top_n_List_string.Add(x.ShortName);

            ListView_top30_passives.ItemsSource = GetF101_passives_top_n_List_string;

        }


        public async void lbltop30_actives_clicked(object sender, EventArgs args)
        {
            await App.MasterDetail.Detail.Navigation.PushAsync(new F101_actives_page());
        }
        public async void lbltop30_passives_clicked(object sender, EventArgs args)
        {
            await App.MasterDetail.Detail.Navigation.PushAsync(new SearchPage2());
        }






        public async void ListView_top30_actives_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var b = GetF101_actives_top_n_List.Find(x => x.ShortName == e.Item.ToString());
            //await Navigation.PushModalAsync(new Bank(b.ShortName, b.RegNum_date, b.IntCode.ToString(), b.RegNum));
        }

        

        public async void ListView_top30_passives_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var b = GetF101_actives_top_n_List.Find(x => x.ShortName == e.Item.ToString());
            //await Navigation.PushModalAsync(new Bank(b.ShortName, b.RegNum_date, b.IntCode.ToString(), b.RegNum));
        }



        public Detail()
		{
			InitializeComponent();
            GetF101_chart();
            GetF101_top_n();
        }

        private  async void Tbi_search(object sender, EventArgs e)
        {
            await App.MasterDetail.Detail.Navigation.PushAsync(new SearchPage2());
        }
    }
}