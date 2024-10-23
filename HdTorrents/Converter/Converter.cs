using HdTorrents.Types.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HdTorrents.Converter
{
    public class CurrentPageFontAttributeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool currentPage)
            {
                return currentPage ? FontAttributes.Bold : FontAttributes.None;
            }
            return FontAttributes.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return FontAttributes.None;
        }    
    }

    public class CurrentPageFontColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                CurrentPage => Colors.Red,
                DummyPage => Colors.LightGray,
                _ => Microsoft.Maui.Graphics.Color.Parse(parameter.ToString())
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return FontAttributes.None;
        }
    }

    public class PageBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                DummyPage => Colors.Transparent,
                _ => Microsoft.Maui.Graphics.Color.Parse(parameter.ToString())
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SpanLayoutConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var layout = (LayoutMode)value;
            return layout switch
            {
                LayoutMode.Details => 1,
                LayoutMode.Poster => 2,
                LayoutMode.Card => 1,
                _ => 1
            };
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
