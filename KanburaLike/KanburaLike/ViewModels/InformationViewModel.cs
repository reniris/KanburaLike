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
using System.Diagnostics;
using System.Linq;

namespace KanburaLike.ViewModels
{
	public class InformationViewModel : ViewModel
	{
		/// <summary>
		/// 艦隊情報
		/// </summary>
		public FleetViewModel[] Fleets { get; private set; }

		/// <summary>
		/// キラ艦
		/// </summary>
		public ShipsViewModel Brilliant { get; } = new ShipsViewModel();
		/// <summary>

		/// 入渠待ち
		/// </summary>
		public ShipsViewModel RepairWaiting { get; } = new ShipsViewModel();

		private KanColleModel Kancolle { get; } = KanColleModel.Current;

		/// <summary>
		/// 艦これが始まったときに呼ばれる
		/// </summary>
		private void Register()
		{
			Messenger.Raise(new InteractionMessage("InfoShow"));
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
			try
			{
				var fleets = this.Kancolle.Fleets;

				if (fleets == null) return;

				Fleets = fleets.Select(f => new FleetViewModel(f)).ToArray();
				RaisePropertyChanged(nameof(Fleets));
			}
			catch (Exception e)
			{
				DebugModel.WriteLine(e);
			}
		}

		/// <summary>
		/// 艦船情報が更新されたときに呼ばれる
		/// </summary>
		private void UpdateShips()
		{
			UpdateBrilliant();
			UpdateRepairWaiting();
		}

		/// <summary>
		/// キラキラ（艦種ごとにソート）
		/// </summary>
		private void UpdateBrilliant()
		{
			var ships = this.Kancolle.Ships;
			if (ships == null) return;

			this.Brilliant.SortedUpdate(ships.Where(s => s.ConditionType == ConditionType.Brilliant), Ship => Ship.Info.ShipType.SortNumber);
			RaisePropertyChanged(nameof(Brilliant));
		}

		/// <summary>
		/// 入渠待ち
		/// </summary>
		private void UpdateRepairWaiting()
		{
			var ships = this.Kancolle.Ships;
			if (ships == null) return;

			this.RepairWaiting.SortedUpdate(ships
				.Where(s => s.TimeToRepair > TimeSpan.Zero)
				.Where(s => s.Situation.HasFlag(ShipSituation.Repair) == false)
				, Ship => Ship.TimeToRepair);
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
			return false;
#endif
		}

		public void Test()
		{
		}
		#endregion

	}
}
