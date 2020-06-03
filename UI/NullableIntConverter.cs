using System;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Globalization;

namespace UI
{
    // int? のようなNullable型は Textbox.Textが Empty の時に数値に変換できないため内部でエラーが発生し
    // 処理が中断されるため、空白をnullに変換して int? に null を代入できるようにする。

    class NullableIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
       {
            string str = (string)value;   
            
            Regex regex = new Regex(@"^\d+$");
            if (regex.IsMatch(str)) return value;
            else return null;
        }
    }
}
