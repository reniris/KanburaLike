using Grabacr07.KanColleWrapper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.Views.Converters
{
	class QuestStatusToString : MultiValueConverterBase<string, QuestState, QuestProgress>
	{
		protected override string Convert(QuestState value1, QuestProgress value2)
		{
			if (value1 == QuestState.Accomplished)
				return "完了";

			switch (value2)
			{
				case QuestProgress.Progress50:
					return "50%";
				case QuestProgress.Progress80:
					return "80%";
			}

			return string.Empty;
		}

		protected override Tuple<QuestState, QuestProgress> ConvertBack(string value)
		{
			return null;
		}
	}
}
