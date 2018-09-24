using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace KanburaLike.Views.Converters
{
	public abstract class MultiValueConverterBase<Target, Source1, Source2> : IMultiValueConverter
	{
		public virtual object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var v1 = values.OfType<Source1>().FirstOrDefault();
			var v2 = values.OfType<Source2>().FirstOrDefault();

			return Convert(v1, v2);
		}

		public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			var src = ConvertBack((Target)value);
			if (src == null) return null;

			return new object[] { src.Item1, src.Item2 };
		}

		protected abstract Target Convert(Source1 value1, Source2 value2);
		protected abstract Tuple<Source1, Source2> ConvertBack(Target value);
	}

	public abstract class MultiValueConverterBase<Target, Source> : IMultiValueConverter
	{
		public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var value = values.OfType<Source>().ToArray();

			return Convert(value);
		}

		public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			var src = ConvertBack((Target)value);
			if (src == null) return null;

			return src.Cast<object>().ToArray();
		}

		protected abstract Target Convert(Source[] value);
		protected abstract Source[] ConvertBack(Target value);
	}
}
