using Grabacr07.KanColleWrapper.Models;
using KanburaLike.Models;
using Livet;
using Livet.EventListeners;
using MetroTrilithon.Lifetime;
using MetroTrilithon.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.ViewModels
{
	public class FleetViewModel : ShipsViewModel
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

		#region ViewRange変更通知プロパティ
		private double _ViewRange;

		public double ViewRange
		{
			get
			{ return _ViewRange; }
			set
			{ 
				if (_ViewRange == value)
					return;
				_ViewRange = value;
				RaisePropertyChanged(nameof(ViewRange));
			}
		}
		#endregion

		public int SumLv => (Ships != null) ? Ships.Sum(s => s.Ship.Level) : 0;
		public int SumAirSuperiority => (Ships != null) ? Ships.Sum(s => s.AirSuperiority) : 0;

		private Fleet Source { get; }

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
			_IsExpanded = true;
			Source = f;

			Source.Subscribe(nameof(Fleet.Name), () => this.Name = Source.Name).AddTo(this);
			Source.Subscribe(nameof(Fleet.State.ViewRange), () => this.ViewRange = Source.State.ViewRange).AddTo(this);
			Source.Subscribe(nameof(Fleet.Ships), () => Update(Source.Ships)).AddTo(this); ;
		}

		protected void Update(IEnumerable<Ship> ships)
		{
			this.Ships = ships.Select((s, i) => new ShipViewModel(s, i + 1)).ToArray();

			RaisePropertyChanged(nameof(Ships));
			RaisePropertyChanged(nameof(SumLv));
			RaisePropertyChanged(nameof(SumAirSuperiority));
		}
	}
}