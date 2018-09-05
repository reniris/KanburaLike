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
		
		public int SumLv => (Ships != null) ? Ships.Sum(s => s.Lv) : 0;
		public int SumAirSuperiority => (Ships != null) ? Ships.Sum(s => s.AirSuperiority) : 0;

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
			_IsExpanded = true;

			Name = f.Name;
			Update(f.Ships);

			listener = new PropertyChangedEventListener(f);
			listener.RegisterHandler(() => f.Ships, (s, e) => Update(f.Ships));
			listener.RegisterHandler(() => f.Name, (s, e) => Name = f.Name);
			this.CompositeDisposable.Add(listener);
		}

		protected override void Update(IEnumerable<Ship> ships)
		{
			base.Update(ships);
			RaisePropertyChanged(nameof(SumLv));
			RaisePropertyChanged(nameof(SumAirSuperiority));
		}
	}
}