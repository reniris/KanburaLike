using Grabacr07.KanColleWrapper.Models;
using Grabacr07.KanColleWrapper.Internal;
using Grabacr07.KanColleWrapper.Models.Raw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.Models
{
	public static class KanColleExtensions
	{
		/// <summary>大破しているか</summary>
		public static bool IsHeavilyDamage(this LimitedValue hp) => (hp.Current / (decimal)hp.Maximum) <= (decimal)0.25;

		/// <summary>TP計算</summary>
		public static decimal CalcTP(this Ship s)
		{
			decimal tp = 0;
			//揚陸ポイントに入った際に大破または撤退していた場合、その艦と装備のTPは計算に含まれません
			if (s.HP.IsHeavilyDamage() == true)
				return 0;

			if (s.Situation.HasFlag(ShipSituation.Evacuation) == true || s.Situation.HasFlag(ShipSituation.Tow) == true)
				return 0;

			//艦種
			var shiptype_table = new Dictionary<string, decimal>
			{
				{"潜水空母",   1 },
				{"駆逐艦",     5},
				{"軽巡洋艦",   2},
				{"航空巡洋艦",  4   },
				{"航空戦艦" ,   7   },
				{"補給艦",    15  },
				{"揚陸艦",    12  },
				{"水上機母艦",  9   },
				{"潜水母艦",   7   },
				{"練習巡洋艦",  6},
			};
			tp += shiptype_table.TryGetValue(s.Info.ShipType.Name, out decimal tp1) ? tp1 : 0;

			//装備
			var item_table = new Dictionary<string, decimal>
			{
				{"ドラム缶(輸送用)",  5   },
				{"大発動艇", 8   },
				{"特大発動艇",   8},
				{"大発動艇(八九式中戦車＆陸戦隊)",    8},
				{"特大発動艇+戦車第11連隊",    8},
				{"特二式内火艇",  2   },
				{"戦闘糧食",    1   },
				{"秋刀魚の缶詰",  1},
			};
			tp += s.EquippedItems.Select(y => item_table.TryGetValue(y.Item.Info.Name, out decimal tp2) ? tp2 : 0).Sum();

			//鬼怒改二のみ特例があり、大発一個分のTPが上乗せ
			if (s.Info.Name == "鬼怒改二")
				tp += item_table["大発動艇"];

			return tp;
		}
	}
}
