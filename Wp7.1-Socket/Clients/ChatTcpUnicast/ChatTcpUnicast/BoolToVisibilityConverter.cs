using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace ChatTcpUnicast {
    public class BoolToVisibilityConverter : IValueConverter {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            bool boolValue;
            if (Boolean.TryParse(value.ToString(), out boolValue)) {
                bool isReverse = System.Convert.ToBoolean(parameter);
                return boolValue ^ isReverse ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            bool isReverse = (bool)parameter;
            Visibility visibility = (Visibility)value;
            return ((visibility == Visibility.Visible) == isReverse) ;
        }
    }
}
