using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using KanburaLike.Models;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using MetroTrilithon.Lifetime;
using MetroTrilithon.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace KanburaLike.ViewModels
{
	public class InformationViewModel : ViewModel
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

		//public KanColleModel Kancolle { get; set; }

		#region KanColle
		private KanColleModel _Kancolle;

		public KanColleModel Kancolle
		{
			get
			{ return _Kancolle; }
			set
			{
				if (_Kancolle == value)
					return;

				_Kancolle = value;

				/*listener.RegisterHandler(() => model.Value, (s, e) =>
				// Value プロパティが変更した時にだけ実行する処理
				});*/

				listener = new PropertyChangedEventListener(this.Kancolle);

				listener.RegisterHandler(() => Kancolle.Fleets, (s, e) => UpdateFleets());
				listener.RegisterHandler(() => Kancolle.Ships, (s, e) => UpdateShips());

				this.CompositeDisposable.Add(listener);
			}
		}
		#endregion


		private PropertyChangedEventListener listener;

		public InformationViewModel()
		{

		}

		public void Initialize()
		{
			KanColleModel.DebugWriteLine("Initialize");
		}

		private void UpdateFleets()
		{
			Fleets = this.Kancolle.Fleets.Select(f => new FleetViewModel(f)).ToArray();

		}

		private void UpdateShips()
		{
			var ships = this.Kancolle.Ships;

			if (ships == null) return;

			//キラキラ
			this.Brilliant.Update(ships.Where(s => s.ConditionType == ConditionType.Brilliant));

			//入渠待ち
			this.RepairWaiting.Update(ships.Where(s => s.TimeToRepair > TimeSpan.Zero));
		}


		#region TestCommand
		private Livet.Commands.ViewModelCommand _TestCommand;

		public Livet.Commands.ViewModelCommand TestCommand
		{
			get
			{
				if (_TestCommand == null)
				{
					_TestCommand = new Livet.Commands.ViewModelCommand(Test, CanTest);
				}
				return _TestCommand;
			}
		}

		public bool CanTest()
		{
#if DEBUG
			return true;
#else
			return false
#endif
		}

		public void Test()
		{
			KanColleModel.DebugWriteLine("TestCommand");

			KanColleModel.DumpDebugData(Fleets, nameof(Fleets));
			KanColleModel.DumpDebugData(this.Brilliant.Ships, nameof(this.Brilliant.Ships));

			KanColleModel.DebugWriteLine("TestCommanded");
		}
		#endregion

	}
}
