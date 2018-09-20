using Grabacr07.KanColleWrapper.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.Views.Converters
{
	class LimitedValueToString : ValueConverterBase<LimitedValue, string>
	{
		public override string Convert(LimitedValue value, Type targetType, object parameter, CultureInfo culture)
		{
			string str = $"({value.Current}/{value.Maximum})";
			return str;
		}

		public override LimitedValue ConvertBack(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return default(LimitedValue);
		}
	}
}
