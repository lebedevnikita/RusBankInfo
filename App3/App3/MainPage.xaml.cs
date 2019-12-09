using App3.Interface;
using Microcharts;
using SkiaSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Refit;

using App3.Model;
using App3.Views;

namespace App3
{
    public partial class MainPage : MasterDetailPage
    {
       
          public MainPage() 
          {
            InitializeComponent();

            this.Master = new Master();
            this.Detail = new NavigationPage(new Detail());
            App.MasterDetail = this;



          




          }

        



    }
}
