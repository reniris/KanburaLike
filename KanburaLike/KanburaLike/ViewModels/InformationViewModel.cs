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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;

namespace KanburaLike.ViewModels
{
	public class InformationViewModel : ViewModel
	{
		/// <summary>
		/// 母港情報
		/// </summary>
		public HomeportViewModel Homeport { get; private set; }

		/// <summary>
		/// 遂行中任務
		/// </summary>
		public QuestsViewModel Quests { get; private set; }

		/// <summary>
		/// 艦隊情報
		/// </summary>
		public FleetViewModel[] Fleets { get; private set; }

		/// <summary>
		/// キラ艦
		/// </summary>
		public ShipsViewModel Brilliant { get; } = new ShipsViewModel(nameof(Brilliant));

		/// <summary>
		/// 入渠待ち
		/// </summary>
		public ShipsViewModel RepairWaiting { get; } = new ShipsViewModel(nameof(RepairWaiting));

		private KanColleModel Kancolle { get; } = KanColleModel.Current;

		/// <summary>
		/// 艦これが始まったときに呼ばれる
		/// </summary>
		private void Register()
		{
			Messenger.Raise(new InteractionMessage("InfoShow"));

			//母港情報
			this.Homeport = new HomeportViewModel();
			RaisePropertyChanged(nameof(Homeport));

			//任務
			this.Quests = new QuestsViewModel();
			RaisePropertyChanged(nameof(Quests));

			DebugModel.WriteLine("艦これ Start");
		}

		private PropertyChangedEventListener listener;

		public InformationViewModel()
		{
			/*listener.RegisterHandler(
			 * () => model.Value, (s, e) =>
				// Value プロパティが変更した時にだけ実行する処理
			});*/

			listener = new PropertyChangedEventListener(Kancolle);

			listener.RegisterHandler(() => Kancolle.IsRegistered, (s, e) => Register());
			listener.RegisterHandler(() => Kancolle.Fleets, (s, e) => UpdateFleets());
			listener.RegisterHandler(() => Kancolle.Ships, (s, e) => UpdateShips());

			this.CompositeDisposable.Add(listener);
		}

		/// <summary>
		/// 艦隊情報が更新されたときに呼ばれる
		/// </summary>
		private void UpdateFleets()
		{
			var fleets = this.Kancolle.Fleets;

			if (fleets == null) return;

			Fleets = fleets.Select(f => new FleetViewModel(f)).ToArray();
			RaisePropertyChanged(nameof(Fleets));
		}

		/// <summary>
		/// 艦船情報が更新されたときに呼ばれる
		/// </summary>
		private void UpdateShips()
		{
			UpdateBrilliant();
			UpdateRepairWaiting();
			UpdateHomeportShips();
		}

		/// <summary>
		/// キラキラ（艦種ごとにソート）
		/// </summary>
		private void UpdateBrilliant()
		{
			var ships = this.Kancolle.Ships;
			if (ships == null) return;

			this.Brilliant.Update(ships
				, s => s.Ship.ConditionType == ConditionType.Brilliant
				, s => s.Ship.Info.ShipType.SortNumber
				, s => s.Ship.Condition);

			RaisePropertyChanged(nameof(Brilliant));
			//DebugModel.WriteLine("Update Brilliant");
		}

		/// <summary>
		/// 入渠待ち
		/// </summary>
		private void UpdateRepairWaiting()
		{
			var ships = this.Kancolle.Ships;
			if (ships == null) return;

			this.RepairWaiting.Update(ships
				, s => s.Ship.TimeToRepair > TimeSpan.Zero && s.IsRepairing == false
				, s => s.Ship.TimeToRepair);

			RaisePropertyChanged(nameof(RepairWaiting));
			//DebugModel.WriteLine("Update RepairWaiting");
		}

		/// <summary>
		/// 母港情報の船更新
		/// </summary>
		private void UpdateHomeportShips()
		{
			var ships = this.Kancolle.Ships;
			if (ships == null) return;

			if (Homeport == null) return;

			Homeport.ShipsCount = ships.Count();

			RaisePropertyChanged(nameof(Homeport));
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
			return false;
#endif
		}

		public void Test()
		{
		}
		#endregion

	}
}
