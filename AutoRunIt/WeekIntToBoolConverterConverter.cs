using System;
using System.Globalization;
using System.Windows.Data;

namespace AutoRunIt
{
    public class WeekIntToBoolConverterConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (int.TryParse(value?.ToString(), out int weekInt) && int.TryParse(parameter?.ToString(), out int position))
                {
                    int testInt = 0b0000_0000_0000_0001 << position - 1;
                    if ((weekInt & testInt) == testInt)
                    {
                        return true;
                    }
                    return false;
                }
                return false;

            }
            catch (Exception exception)
            {
                LogHelper.LogError("WeekUIntToBoolConverterConverter Convert!", exception);
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (bool.TryParse(value?.ToString(),out bool isChecked)&& int.TryParse(parameter?.ToString(), out int position))
                {
                    if (isChecked)
                    {
                        return position;
                    }
                    return -position;
                }
                LogHelper.LogError("WeekUIntToBoolConverterConverter ConvertBack! return 0 !");
                return 0;
            }
            catch (Exception exception)
            {
                LogHelper.LogError("WeekUIntToBoolConverterConverter ConvertBack!", exception);
                return 0;
            }
        }

        #endregion
    }
}