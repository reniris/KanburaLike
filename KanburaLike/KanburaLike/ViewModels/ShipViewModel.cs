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

			this.Ship.Subscribe(nameof(Ship.Slots), () => this.AirSuperiority = s.GetAirSuperiorityPotential()).AddTo(this);
		}
	}
}
