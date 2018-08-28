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

		private KanColleModel()
		{
			InitDebug();

			KanColleClient.Current
				.Subscribe(nameof(KanColleClient.IsStarted), this.RegisterHomeportListener, false)
				.AddTo(this);
		}

		private void RegisterHomeportListener()
		{
			if (this.IsRegistered) return;
			try
			{
				DebugWriteLine("RegisterHomeportListener");

				var client = KanColleClient.Current;
				if (client.Homeport == null) return;

				client.Homeport.Organization
					.Subscribe(nameof(Organization.Fleets), () => this.UpdateFleets(client.Homeport.Organization))
					.Subscribe(nameof(Organization.Ships), () => this.UpdateShips(client.Homeport.Organization))
					.AddTo(this);

				DebugWriteLine("Registered HomeportListener");

				this.IsRegistered = true;
			}
			catch (Exception e)
			{
				DebugWriteLine($"{e.GetType().ToString()} {e.Message}");
			}
		}

		/// <summary>
		/// 艦娘に変化があったときに呼ばれる
		/// </summary>
		/// <param name="organization">organization</param>
		/// <returns></returns>
		private void UpdateShips(Organization organization)
		{
			DebugWriteLine("Model UpdateShips");
			var ships = organization?.Ships.Values;
			if (ships != null)
			{
				Ships = ships;
			}
		}

		/// <summary>
		/// 艦隊に変化があったときに呼ばれる
		/// </summary>
		/// <param name="organization">organization</param>
		private void UpdateFleets(Organization organization)
		{
			//this.organizationDisposables?.Dispose();
			//this.organizationDisposables = new LivetCompositeDisposable();
			try
			{
				DebugWriteLine("Model UpdateFleets");
				var fleets = organization.Fleets.Values;
				if (fleets != null)
				{
					Fleets = fleets;
				}
			}
			catch (Exception e)
			{
				DebugWriteLine(e);
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
		public static void DumpDebugData(object data, string filename)
		{
			string dir = GetDllFolder();
			var fullpath = Path.Combine(dir, filename);
			try
			{
				// XAMLで書き出し
				var text = System.Windows.Markup.XamlWriter.Save(data);
				DebugWriteLine(text);
				System.IO.File.WriteAllText(fullpath + ".xaml", text);

				//XMLで書き出し
				var xmls = new XmlSerializer(data.GetType());
				using (var writer = new StreamWriter(fullpath + ".xml", false, Encoding.UTF8))
				{
					xmls.Serialize(writer, data);
					writer.Flush();
				}
			}
			catch (Exception e)
			{
				DebugWriteLine(e);
			}
		}

		[Conditional("DEBUG")]
		public static void DebugWriteLine(string message)
		{
			var now = DateTime.Now;
			Debug.WriteLine($"{now}\t{message}");
		}

		[Conditional("DEBUG")]
		public static void DebugWriteLine(Exception e)
		{
			DebugWriteLine($"{e.GetType().ToString()} {e?.TargetSite.ToString()} {e.Message}");
		}

		[Conditional("DEBUG")]
		private static void InitDebug()
		{
			//DefaultTraceListenerオブジェクトを取得
			DefaultTraceListener drl = (DefaultTraceListener)Trace.Listeners["Default"];
			//LogFileNameを変更する
			string dir = GetDllFolder();
			drl.LogFileName = Path.Combine(dir, "debug.txt");

			//デバッグログを見やすくするために空行を入れる
			Debug.WriteLine("");
		}

		/// <summary>
		/// このプラグインのあるフォルダを取得
		/// </summary>
		/// <returns></returns>
		private static string GetDllFolder()
		{
			return Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
		}
	}
}
