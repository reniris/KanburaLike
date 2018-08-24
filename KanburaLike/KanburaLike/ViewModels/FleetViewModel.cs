using Grabacr07.KanColleWrapper.Models;
using Livet;
using Livet.EventListeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.ViewModels
{
	class FleetViewModel : Livet.ViewModel
	{

		#region Name変更通知プロパティ
		private string _Name;

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

		/*		#region IsExpanded変更通知プロパティ
				private bool _IsExpanded = true;

				public bool IsExpanded
				{
					get
					{ return _IsExpanded; }
					set
					{
						if (_IsExpanded == value)
							return;
						_IsExpanded = value;
						RaisePropertyChanged(nameof(IsExpanded));
					}
				}
				#endregion

				#region Ships変更通知プロパティ
				private IEnumerable<ShipViewModel> _Ships;

				public IEnumerable<ShipViewModel> Ships
				{
					get
					{ return _Ships; }
					set
					{
						if (_Ships == value)
							return;
						_Ships = value;
						RaisePropertyChanged(nameof(Ships));
					}
				}
				#endregion
				*/

		public ShipsViewModel Ships { get; set; } = new ShipsViewModel();

		#region SumLv変更通知プロパティ
		private int _SumLv;

		public int SumLv
		{
			get
			{ return _SumLv; }
			set
			{
				if (_SumLv == value)
					return;
				_SumLv = value;
				RaisePropertyChanged(nameof(SumLv));
			}
		}
		#endregion


		#region SumAirSuperiority変更通知プロパティ
		private int _SumAirSuperiority;

		public int SumAirSuperiority
		{
			get
			{ return _SumAirSuperiority; }
			set
			{
				if (_SumAirSuperiority == value)
					return;
				_SumAirSuperiority = value;
				RaisePropertyChanged(nameof(SumAirSuperiority));
			}
		}
		#endregion


		/// <summary>
		/// デザイナ用 <see cref="FleetViewModel"/> class.
		/// </summary>
		public FleetViewModel()
		{
			//listener.RegisterHandler(() => model.Value, (s, e) =>
			// Value プロパティが変更した時にだけ実行する処理
			//});
		}

		/// <summary>
		/// コードからはこっちを使う <see cref="FleetViewModel"/> class.
		/// </summary>
		/// <param name="f">f</param>
		public FleetViewModel(Fleet f)
		{
			Name = f.Name;
			Ships.Update(f.Ships);
			SumLv = Ships.Ships.Sum(s => s.Lv);
			SumAirSuperiority = Ships.Ships.Sum(s => s.AirSuperiority);
		}
	}
}