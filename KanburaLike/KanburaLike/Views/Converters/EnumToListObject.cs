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
	class EnumToListObject : AlternationConverterBase<Enum>
	{
		protected override int ValueToIndex(Enum value, object param)
		{
			return System.Convert.ToInt32(value);
		}

		protected override Enum IndexToValue(int index, object param)
		{
			return null;
		}
	}
}
