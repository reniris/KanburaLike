using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace KanburaLike.Views.Converters
{
	public abstract class ValueConverterBase<T, U> : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (value)
			{
				case T t_val:
					return Convert(t_val, targetType, parameter, culture);
				case IEnumerable<T> t_arr:
					return t_arr.Select(t => Convert(t, targetType, parameter, culture));
				default:
					return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (value)
			{
				case U u_val:
					return ConvertBack(u_val, targetType, parameter, culture);
				case IEnumerable<U> u_arr:
					return u_arr.Select(u => ConvertBack(u, targetType, parameter, culture));
				default:
					return null;
			}
		}

		public abstract U Convert(T value, Type targetType, object parameter, CultureInfo culture);
		public abstract T ConvertBack(U value, Type targetType, object parameter, CultureInfo culture);
	}

	public abstract class ValueConverterBase<T> : IValueConverter
	{
		public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			switch (value)
			{
				case T t_val:
					return Convert(t_val, targetType, parameter, culture);
				case IEnumerable<T> t_arr:
					return t_arr.Select(t => Convert(t, targetType, parameter, culture));
				default:
					return null;
			}
		}

		public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}

		public abstract object Convert(T value, Type targetType, object parameter, CultureInfo culture);
	}

	[ContentProperty("Values")]
	public abstract class AlternationConverterBase<T> : ValueConverterBase<T>
	{
		public IList Values { get; } = new List<object>();

		public override object Convert(T value, Type targetType, object parameter, CultureInfo culture)
		{
			int index = ValueToIndex(value, parameter);
			return this.Values[index];
		}

		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return IndexToValue(this.Values.IndexOf(value), parameter);
		}

		protected abstract int ValueToIndex(T value, object param);
		protected abstract T IndexToValue(int index, object param);
	}
}
