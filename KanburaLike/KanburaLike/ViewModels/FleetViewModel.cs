using Grabacr07.KanColleWrapper.Models;
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
	public class FleetViewModel : Livet.ViewModel
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