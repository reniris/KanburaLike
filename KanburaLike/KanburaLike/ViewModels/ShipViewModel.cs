using Grabacr07.KanColleWrapper.Models;
using KanburaLike.Models;
using Livet;
using Livet.EventListeners;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.ViewModels
{
	public class ShipViewModel : Livet.ViewModel
	{

		#region Name変更通知プロパティ
		private string _Name;

		/// <summary>
		/// 艦の名前
		/// </summary>
		public string Name
		{
			get
			{ return _Name; }
			set
			{
				if (_Name == value)
					return;
				_Name = value;
				RaisePropertyChanged(nameof(Name));
			}
		}
		#endregion

		#region Index変更通知プロパティ
		private int _Index;

		/// <summary>
		/// 艦隊のインデックス（１スタート）
		/// </summary>
		public int Index
		{
			get
			{ return _Index; }
			set
			{
				if (_Index == value)
					return;
				_Index = value;
				RaisePropertyChanged(nameof(Index));
			}
		}
		#endregion


		#region Lv変更通知プロパティ
		private int _Lv;

		/// <summary>
		/// レベル
		/// </summary>
		public int Lv
		{
			get
			{ return _Lv; }
			set
			{
				if (_Lv == value)
					return;
				_Lv = value;
				RaisePropertyChanged(nameof(Lv));
			}
		}
		#endregion

		#region Condition変更通知プロパティ
		private int _Condition;

		/// <summary>
		/// Cond値
		/// </summary>
		public int Condition
		{
			get
			{ return _Condition; }
			set
			{
				if (_Condition == value)
					return;
				_Condition = value;
				RaisePropertyChanged(nameof(Condition));
			}
		}
		#endregion

		#region ConditionType変更通知プロパティ
		private int _ConditionType;

		/// <summary>
		/// Cond値種類
		/// </summary>
		public int ConditionType
		{
			get
			{ return _ConditionType; }
			set
			{
				if (_ConditionType == value)
					return;
				_ConditionType = value;
				RaisePropertyChanged(nameof(ConditionType));
			}
		}
		#endregion


		#region ExpForNextLevel変更通知プロパティ
		private int _ExpForNextLevel;

		/// <summary>
		/// 次のレベルへの経験値
		/// </summary>
		public int ExpForNextLevel
		{
			get
			{ return _ExpForNextLevel; }
			set
			{
				if (_ExpForNextLevel == value)
					return;
				_ExpForNextLevel = value;
				RaisePropertyChanged(nameof(ExpForNextLevel));
			}
		}
		#endregion

		#region AirSuperiority変更通知プロパティ
		private int _AirSuperiority = 0;

		/// <summary>
		/// 制空
		/// </summary>
		public int AirSuperiority
		{
			get
			{ return _AirSuperiority; }
			set
			{
				if (_AirSuperiority == value)
					return;
				_AirSuperiority = value;
				RaisePropertyChanged(nameof(AirSuperiority));
			}
		}
		#endregion

		#region CurrentHP変更通知プロパティ
		private int _CurrentHP;

		/// <summary>
		/// 現在のHP
		/// </summary>
		public int CurrentHP
		{
			get
			{ return _CurrentHP; }
			set
			{
				if (_CurrentHP == value)
					return;
				_CurrentHP = value;
				RaisePropertyChanged(nameof(CurrentHP));
			}
		}
		#endregion

		#region MaxHP変更通知プロパティ
		private int _MaxHP;

		/// <summary>
		/// 最大HP
		/// </summary>
		public int MaxHP
		{
			get
			{ return _MaxHP; }
			set
			{
				if (_MaxHP == value)
					return;
				_MaxHP = value;
				RaisePropertyChanged(nameof(MaxHP));
			}
		}
		#endregion


		#region HPRateIndex変更通知プロパティ
		private int _HPRateIndex = 0;

		public int HPRateIndex
		{
			get
			{ return _HPRateIndex; }
			set
			{
				if (_HPRateIndex == value)
					return;
				_HPRateIndex = value;
				RaisePropertyChanged(nameof(HPRateIndex));
			}
		}
		#endregion

		#region FuelRateIndex変更通知プロパティ
		private int _FuelRateIndex;

		public int FuelRateIndex
		{
			get
			{ return _FuelRateIndex; }
			set
			{
				if (_FuelRateIndex == value)
					return;
				_FuelRateIndex = value;
				RaisePropertyChanged(nameof(FuelRateIndex));
			}
		}
		#endregion

		#region BullRateIndex変更通知プロパティ
		private int _BullRateIndex;

		public int BullRateIndex
		{
			get
			{ return _BullRateIndex; }
			set
			{
				if (_BullRateIndex == value)
					return;
				_BullRateIndex = value;
				RaisePropertyChanged(nameof(BullRateIndex));
			}
		}
		#endregion

		#region TimeToRepair変更通知プロパティ
		private TimeSpan _TimeToRepair;

		public TimeSpan TimeToRepair
		{
			get
			{ return _TimeToRepair; }
			set
			{
				if (_TimeToRepair == value)
					return;
				_TimeToRepair = value;
				RaisePropertyChanged(nameof(TimeToRepair));
			}
		}
		#endregion


		#region ShipTypeName変更通知プロパティ
		private string _ShipTypeName;

		public string ShipTypeName
		{
			get
			{ return _ShipTypeName; }
			set
			{
				if (_ShipTypeName == value)
					return;
				_ShipTypeName = value;
				RaisePropertyChanged(nameof(ShipTypeName));
			}
		}
		#endregion


		/// <summary>
		/// デザイナ用<see cref="ShipViewModel"/> class.
		/// </summary>
		public ShipViewModel()
		{
			
		}

		/// <summary>
		/// コードからはこっちを使う <see cref="ShipViewModel"/> class.
		/// </summary>
		/// <param name="s">s</param>
		public ShipViewModel(Ship s, int i)
		{
			this.Name = s.Info?.Name;
			this.Index = i;
			this.Lv = s.Level;
			this.Condition = s.Condition;
			this.ConditionType = (int)s.ConditionType;
			this.ExpForNextLevel = s.ExpForNextLevel;

			this.AirSuperiority = s.GetAirSuperiorityPotential();

			this.FuelRateIndex = GetRateIndex(s.Fuel.Current, s.Fuel.Maximum);
			this.BullRateIndex = GetRateIndex(s.Bull.Current, s.Bull.Maximum);

			this.CurrentHP = s.HP.Current;
			this.MaxHP = s.HP.Maximum;

			UpdateHP();

			this.TimeToRepair = s.TimeToRepair;

			this.ShipTypeName = s.Info?.ShipType.Name;
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

			KanColleModel.DebugWriteLine($"GetRateIndex err {rate}");
			return 4;
		}

		private void UpdateHP()
		{
			this.HPRateIndex = GetRateIndex(this.CurrentHP, this.MaxHP);
		}
	}
}
