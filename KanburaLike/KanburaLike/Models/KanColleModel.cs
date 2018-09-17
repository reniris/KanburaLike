using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using KanburaLike.Models.Settings;
using Livet;
using MetroTrilithon.Lifetime;
using MetroTrilithon.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KanburaLike.Models
{
	public class KanColleModel : Livet.NotificationObject, IDisposableHolder
	{
		#region singleton

		public static KanColleModel Current { get; } = new KanColleModel();

		#endregion

		#region Fleets変更通知プロパティ
		private IEnumerable<Fleet> _Fleets;

		public IEnumerable<Fleet> Fleets
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

		#region Ships変更通知プロパティ
		private IEnumerable<Ship> _Ships;

		public IEnumerable<Ship> Ships
		{
			get
			{ return _Ships; }
			set
			{
				if (_Ships == value)
					return;
				_Ships = value;
				RaisePropertyChanged(nameof(Ships));
			}
		}
		#endregion

		#region RepairDocks変更通知プロパティ
		private IEnumerable<RepairingDock> _RepairDocks;

		public IEnumerable<RepairingDock> RepairDocks
		{
			get
			{ return _RepairDocks; }
			set
			{
				if (_RepairDocks == value)
					return;
				_RepairDocks = value;
				RaisePropertyChanged(nameof(RepairDocks));
			}
		}
		#endregion

		#region Repairyard変更通知プロパティ
		private Repairyard _Repairyard;

		public Repairyard Repairyard
		{
			get
			{ return _Repairyard; }
			set
			{ 
				if (_Repairyard == value)
					return;
				_Repairyard = value;
				RaisePropertyChanged(nameof(Repairyard));
			}
		}
		#endregion

		#region IsRegistered変更通知プロパティ
		private bool _IsRegistered = false;

		public bool IsRegistered
		{
			get
			{ return _IsRegistered; }
			set
			{
				if (_IsRegistered == value)
					return;
				_IsRegistered = value;
				RaisePropertyChanged(nameof(IsRegistered));
			}
		}
		#endregion

		//private LivetCompositeDisposable organizationDisposables;
		//private LivetCompositeDisposable repairyardDisposables;
		private readonly LivetCompositeDisposable compositeDisposable = new LivetCompositeDisposable();
		public ICollection<IDisposable> CompositeDisposable => this.compositeDisposable;

		private KanColleModel()
		{
			KanColleClient.Current
				.Subscribe(nameof(KanColleClient.IsStarted), this.RegisterHomeportListener, false)
				.AddTo(this);
		}

		private void RegisterHomeportListener()
		{
			if (this.IsRegistered) return;
			try
			{
				var client = KanColleClient.Current;
				client.Homeport.Repairyard
					.Subscribe(nameof(Repairyard.Docks), () => this.UpdateRepairyard(client.Homeport.Repairyard))
					.AddTo(this);

				client.Homeport.Organization
					.Subscribe(nameof(Organization.Fleets), () => this.UpdateFleets(client.Homeport.Organization))
					.Subscribe(nameof(Organization.Ships), () => this.UpdateShips(client.Homeport.Organization))
					.AddTo(this);

				this.IsRegistered = true;
			}
			catch (Exception e)
			{
				DebugModel.WriteLine(e);
			}
		}

		/// <summary>
		/// 入渠ドックに変化があったとき呼ばれる
		/// </summary>
		/// <param name="repairyard">repairyard</param>
		private void UpdateRepairyard(Repairyard repairyard)
		{
			this.RepairDocks = repairyard.Docks.Values;

			Repairyard = repairyard;
		}

		/// <summary>
		/// 艦娘に変化があったときに呼ばれる
		/// </summary>
		/// <param name="organization">organization</param>
		/// <returns></returns>
		private void UpdateShips(Organization organization)
		{
			this.Ships = organization.Ships.Values;
		}

		/// <summary>
		/// 艦隊に変化があったときに呼ばれる
		/// </summary>
		/// <param name="organization">organization</param>
		private void UpdateFleets(Organization organization)
		{
			this.Fleets = organization.Fleets.Values;
		}

		public void Dispose()
		{
			this.compositeDisposable.Dispose();
			//this.dockyardDisposables?.Dispose();
			//this.repairyardDisposables?.Dispose();
			//this.organizationDisposables?.Dispose();
		}
	}
}
