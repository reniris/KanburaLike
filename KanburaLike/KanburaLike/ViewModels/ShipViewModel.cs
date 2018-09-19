using Grabacr07.KanColleWrapper.Models;
using KanburaLike.Models;
using Livet;
using Livet.EventListeners;
using MetroTrilithon.Mvvm;
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

		#region IsRepairing変更通知プロパティ
		private bool _IsRepairing;

		public bool IsRepairing
		{
			get
			{ return _IsRepairing; }
			set
			{
				if (_IsRepairing == value)
					return;
				_IsRepairing = value;
				RaisePropertyChanged(nameof(IsRepairing));
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

		public Ship Ship { get; set; }

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
			this.Ship = s;
			this.Index = i;
			var kmodel = KanColleModel.Current;
			kmodel.Subscribe(nameof(kmodel.RepairState), () => this.IsRepairing = kmodel.Repairyard.CheckRepairing(Ship.Id)).AddTo(this);

			this.ConditionType = (int)s.ConditionType;

			this.Ship.Subscribe(nameof(Ship.Slots), () => this.AirSuperiority = s.GetAirSuperiorityPotential()).AddTo(this);

			this.FuelRateIndex = GetRateIndex(s.Fuel.Current, s.Fuel.Maximum);
			this.BullRateIndex = GetRateIndex(s.Bull.Current, s.Bull.Maximum);

			this.CurrentHP = s.HP.Current;
			this.MaxHP = s.HP.Maximum;

			UpdateHP();
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

			return 4;
		}

		private void UpdateHP()
		{
			this.HPRateIndex = GetRateIndex(this.CurrentHP, this.MaxHP);
		}
	}
}
