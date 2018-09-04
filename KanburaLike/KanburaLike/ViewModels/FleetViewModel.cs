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

		#region IsExpanded変更通知プロパティ
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

		public ShipViewModel[] Ships { get; private set; }

		public int SumLv => (Ships != null) ? Ships.Sum(s => s.Lv) : 0;
		public int SumAirSuperiority => (Ships != null) ? Ships.Sum(s => s.AirSuperiority) : 0;

		/// <summary>
		/// 艦これの艦隊データ
		/// </summary>
		public Fleet Source { get; }

		private PropertyChangedEventListener listener;

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
			Source = f;

			Name = f.Name;
			UpdateShips();

			listener = new PropertyChangedEventListener(f);
			listener.RegisterHandler(() => f.Ships, (s, e) => UpdateShips());
			listener.RegisterHandler(() => f.Name, (s, e) => Name = f.Name);
			this.CompositeDisposable.Add(listener);
		}

		private void UpdateShips()
		{
			this.Ships = this.Source.Ships.Select((x, i) => new ShipViewModel(x, i + 1)).ToArray();

			RaisePropertyChanged(nameof(Ships));
			RaisePropertyChanged(nameof(SumLv));
			RaisePropertyChanged(nameof(SumAirSuperiority));
		}
	}
}