using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace App3.CS
{
    public class converter_numbers : IValueConverter
    {



        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            long longValue;
            long.TryParse(value.ToString(), out longValue);

            long longParameter;
            long.TryParse(parameter.ToString(), out longParameter);
         

            


            return longValue/ longParameter;




            //throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
