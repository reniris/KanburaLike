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


		#region RepairWaiting変更通知プロパティ
		private RepairWaitingViewModel _RepairWaiting = new RepairWaitingViewModel();

		public RepairWaitingViewModel RepairWaiting
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
			var kira = ships.Where(s => s.ConditionType == ConditionType.Brilliant).Select((s, i) => new ShipViewModel(s, i + 1)).ToArray();
			Kancolle.DumpDebugData(kira, nameof(kira));

			//入渠待ち
			this.RepairWaiting.Ships = ships.Where(s => s.TimeToRepair > TimeSpan.Zero).Select((s, i) => new ShipViewModel(s, i + 1)).ToArray();
		}
	}
}
