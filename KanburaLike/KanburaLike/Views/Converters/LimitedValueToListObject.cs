using Grabacr07.KanColleWrapper.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace KanburaLike.Views.Converters
{
	class LimitedValueToListObject : AlternationConverterBase<LimitedValue>
	{
		protected override int ValueToIndex(LimitedValue value)
		{
			return GetRateIndex(value.Current, value.Maximum);
		}

		protected override LimitedValue IndexToValue(int index)
		{
			//使わないんで適当に実装
			return default(LimitedValue);
		}

		private decimal GetRate(decimal current, decimal max)
		{
			if (max == decimal.Zero)
				return decimal.MinusOne;

			var rate = (current / max) * 100;
			return rate;
		}

		private int GetRateIndex(decimal current, decimal max)
		{
			var rate = GetRate(current, max);

			//100以上
			if (rate >= 100)
				return 0;

			//100未満 75超
			if (rate < 100 && 75 < rate)
				return 1;

			//75以下 50超
			if (rate <= 75 && 50 < rate)
				return 2;

			//50以下 25超
			if (rate <= 50 && 25 < rate)
				return 3;

			//25以下
			if (rate <= 25)
				return 4;

			throw new ArgumentException();
		}
	}
}
