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
	public enum LimitedValueOption
	{
		HP,
		FuelBull
	}

	class LimitedValueToListObject : AlternationConverterBase<LimitedValue>
	{
		public LimitedValueOption Option { get; set; }

		protected override int ValueToIndex(LimitedValue value, object param)
		{
			int notch;
			bool andover;

			switch (Option)
			{
				case LimitedValueOption.HP:
					notch = 25;
					andover = false;
					break;
				case LimitedValueOption.FuelBull:
					notch = 10;
					andover = false;
					break;
				default:
					throw new ArgumentOutOfRangeException($"{nameof(LimitedValueOption)}");
			}
			return GetRateIndex(value.Current, value.Maximum, notch, andover);
		}

		protected override LimitedValue IndexToValue(int index, object param)
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

		private int GetRateIndex(decimal current, decimal max, int notch, bool andover)
		{
			var rate = GetRate(current, max);

			//100以上
			if (rate >= 100)
				return 0;

			int ret = 0;
			for (int i = 100; i > 0; i -= notch)
			{
				if (andover == true)
				{
					if (rate >= i)
						return ret;
				}
				else
				{
					if (rate > i)
						return ret;
				}

				ret++;
			}
			return ret;
		}
	}
}
