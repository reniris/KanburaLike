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
			if (this.IsRegistered) return;

			var client = KanColleClient.Current;

			client.Homeport.Organization
				.Subscribe(nameof(Organization.Fleets), () => this.UpdateFleets(client.Homeport.Organization))
				.AddTo(this);

			this.IsRegistered = true;
		}

		private void UpdateFleets(Organization organization)
		{
			//this.organizationDisposables?.Dispose();
			//this.organizationDisposables = new LivetCompositeDisposable();

			var fleets = organization?.Fleets?.Values;
			if (fleets != null)
			{
				Fleets = fleets;
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
