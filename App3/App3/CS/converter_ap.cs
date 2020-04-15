using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace App3.CS
{
    public class converter_ap : IValueConverter
    {

        public string val { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

         
            if (value.ToString() == "1")
            { return "Активы"; }
            else 
            if (value.ToString() == "2")
            { return "Пассивы"; }
            else 
            { return "-"; }


          
  

            //throw new NotImplementedException();
        }






        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
