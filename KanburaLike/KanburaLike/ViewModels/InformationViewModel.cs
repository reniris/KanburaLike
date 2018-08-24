using Grabacr07.KanColleWrapper.Models;
using KanburaLike.Models;
using Livet;
using Livet.EventListeners;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KanburaLike.ViewModels
{
	class InformationViewModel : ViewModel
	{

		#region Fleets変更通知プロパティ
		private IEnumerable<FleetViewModel> _Fleets;

		public IEnumerable<FleetViewModel> Fleets
		{
			get
			{ return _Fleets; }
			set
			{
				if (_Fleets == value)
					return;
				_Fleets = value;
				RaisePropertyChanged(nameof(Fleets));
			}
		}
		#endregion


		#region Brilliant変更通知プロパティ
		private ShipsViewModel _Brilliant = new ShipsViewModel();

		public ShipsViewModel Brilliant
		{
			get
			{ return _Brilliant; }
			set
			{ 
				if (_Brilliant == value)
					return;
				_Brilliant = value;
				RaisePropertyChanged(nameof(Brilliant));
			}
		}
		#endregion


		#region RepairWaiting変更通知プロパティ
		private ShipsViewModel _RepairWaiting = new ShipsViewModel();

		public ShipsViewModel RepairWaiting
		{
			get
			{ return _RepairWaiting; }
			set
			{
				if (_RepairWaiting == value)
					return;
				_RepairWaiting = value;
				RaisePropertyChanged(nameof(RepairWaiting));
			}
		}
		#endregion


		private KanColleModel Kancolle = new KanColleModel();
		private readonly PropertyChangedEventListener listener;

		public InformationViewModel()
		{
			/*listener.RegisterHandler(() => model.Value, (s, e) =>
				// Value プロパティが変更した時にだけ実行する処理
			});*/

			listener = new PropertyChangedEventListener(this.Kancolle);

			listener.RegisterHandler(() => Kancolle.Fleets, (s, e) => UpdateFleets());
			listener.RegisterHandler(() => Kancolle.Ships, (s, e) => UpdateShips());

			this.CompositeDisposable.Add(listener);
		}

		public void Initialize()
		{

		}

		private void UpdateFleets()
		{
			Fleets = this.Kancolle.Fleets.Select(f => new FleetViewModel(f)).ToArray();
			Kancolle.DumpDebugData(Fleets, nameof(Fleets));
		}

		private void UpdateShips()
		{
			var ships = this.Kancolle.Ships;

			//キラキラ
			this.Brilliant.Update(ships.Where(s => s.ConditionType == ConditionType.Brilliant));
			Kancolle.DumpDebugData(this.Brilliant.Ships, nameof(this.Brilliant.Ships));

			//入渠待ち
			this.RepairWaiting.Update(ships.Where(s => s.TimeToRepair > TimeSpan.Zero));
		}
	}
}
