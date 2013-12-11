using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ContosoFinancialServices.Common
{
    public class TextToSliderValueConverter : IValueConverter
    {
        public double MaximumSliderValue { get; set; }
        public double MinimumSliderValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double sliderValue;

            if (double.TryParse(value as string, out sliderValue)
                && sliderValue <= MaximumSliderValue && sliderValue >= MinimumSliderValue)
            {
                return sliderValue;
            }
            else
            {
                return 0.0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
