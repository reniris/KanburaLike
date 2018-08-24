using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
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

namespace KanburaLike.Models
{
	class KanColleModel : Livet.NotificationObject, IDisposableHolder
	{

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


		private bool isRegistered;

		//private LivetCompositeDisposable organizationDisposables;
		private readonly LivetCompositeDisposable compositeDisposable = new LivetCompositeDisposable();
		public ICollection<IDisposable> CompositeDisposable => this.compositeDisposable;

		public KanColleModel()
		{
			KanColleClient.Current
				.Subscribe(nameof(KanColleClient.IsStarted), this.RegisterHomeportListener, false)
				.AddTo(this);
		}

		private void RegisterHomeportListener()
		{
			if (this.isRegistered) return;

			var client = KanColleClient.Current;

			client.Homeport.Organization
				.Subscribe(nameof(Organization.Fleets), () => this.UpdateFleets(client.Homeport.Organization))
				.AddTo(this);

			this.isRegistered = true;
		}
		
		/// <summary>
		/// 艦隊に変化があったときに呼ばれる
		/// </summary>
		/// <param name="organization">organization</param>
		private void UpdateFleets(Organization organization)
		{
			//this.organizationDisposables?.Dispose();
			//this.organizationDisposables = new LivetCompositeDisposable();

			var fleets = organization?.Fleets.Values;
			if (fleets != null)
			{
				Fleets = fleets;
			}

			var ships = organization?.Ships.Values;
			if(ships != null)
			{
				Ships = ships;
			}
		}

		public void Dispose()
		{
			this.compositeDisposable.Dispose();
			//this.dockyardDisposables?.Dispose();
			//this.repairyardDisposables?.Dispose();
			//this.organizationDisposables?.Dispose();
		}

		/// <summary>
		/// デバッグ用データ書き出し
		/// </summary>
		[Conditional("DEBUG")]
		public void DumpDebugData(object data, string filename)
		{
			var dir = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;

			// XAMLで書き出し
			var text = System.Windows.Markup.XamlWriter.Save(data);
			filename += ".xaml";
			System.IO.File.WriteAllText(Path.Combine(dir, filename), text);
		}
	}
}
