using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using KanburaLike.Models;
using Livet;
using Livet.Commands;
using Livet.EventListeners;
using Livet.Messaging;
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
		/// <summary>
		/// 艦隊情報
		/// </summary>
		public FleetViewModel[] Fleets { get; set; }
		/// <summary>
		/// キラ艦
		/// </summary>
		public ShipsViewModel Brilliant { get; } = new ShipsViewModel();
		/// <summary>
		/// 入渠待ち
		/// </summary>
		public ShipsViewModel RepairWaiting { get; } = new ShipsViewModel();

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

				listener.RegisterHandler(() => Kancolle.IsRegistered, (s, e) => Register());
				listener.RegisterHandler(() => Kancolle.Fleets, (s, e) => UpdateFleets());
				listener.RegisterHandler(() => Kancolle.Ships, (s, e) => UpdateShips());

				this.CompositeDisposable.Add(listener);
			}
		}

		/// <summary>
		/// 艦これが始まったときに呼ばれる
		/// </summary>
		private void Register()
		{
			Messenger.Raise(new TransitionMessage(typeof(Views.InformationWindow), this, TransitionMode.NewOrActive, "ShowMain"));
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
			RaisePropertyChanged(nameof(Fleets));
			KanColleModel.DebugWriteLine("ViewModel UpdateFleets");
		}

		private void UpdateShips()
		{
			var ships = this.Kancolle.Ships;

			if (ships == null) return;

			KanColleModel.DebugWriteLine("ViewModel UpdateShips");

			//キラキラ
			this.Brilliant.Update(ships.Where(s => s.ConditionType == ConditionType.Brilliant));
			RaisePropertyChanged(nameof(Brilliant));

			//入渠待ち
			this.RepairWaiting.Update(ships.Where(s => s.TimeToRepair > TimeSpan.Zero));
			RaisePropertyChanged(nameof(RepairWaiting));
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
