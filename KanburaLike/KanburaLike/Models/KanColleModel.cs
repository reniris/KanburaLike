﻿using Grabacr07.KanColleWrapper;
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


		private bool isRegistered = false;

		//private LivetCompositeDisposable organizationDisposables;
		private readonly LivetCompositeDisposable compositeDisposable = new LivetCompositeDisposable();
		public ICollection<IDisposable> CompositeDisposable => this.compositeDisposable;

		public KanColleModel()
		{
			KanColleClient.Current
				.Subscribe(nameof(KanColleClient.IsStarted), this.RegisterHomeportListener, false)
				.AddTo(this);

			InitDebug();
		}

		public void UpdateHomeport()
		{
			RegisterHomeportListener();
		}

		private void RegisterHomeportListener()
		{
			if (this.isRegistered) return;

			DebugWriteLine("RegisterHomeportListener");

			var client = KanColleClient.Current;
			if (client.Homeport == null) return;

			client.Homeport.Organization
				.Subscribe(nameof(Organization.Fleets), () => this.UpdateFleets(client.Homeport.Organization))
				.Subscribe(nameof(Organization.Ships), () => this.UpdateShips(client.Homeport.Organization))
				.AddTo(this);

			DebugWriteLine("Registered HomeportListener");

			this.isRegistered = true;
		}

		/// <summary>
		/// 艦娘に変化があったときに呼ばれる
		/// </summary>
		/// <param name="organization">organization</param>
		/// <returns></returns>
		private void UpdateShips(Organization organization)
		{
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

			var fleets = organization?.Fleets.Values;
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
		public static void DumpDebugData(object data, string filename)
		{
			string dir = GetDllFolder();
			var fullpath = Path.Combine(dir, filename);

			// XAMLで書き出し
			var text = System.Windows.Markup.XamlWriter.Save(data);
			System.IO.File.WriteAllText(fullpath + ".xaml", text);

			// シリアライズする
			/*var xmlSerializer1 = new XmlSerializer(data.GetType());
			using (var streamWriter = new StreamWriter(fullpath + ".xml", false, Encoding.UTF8))
			{
				xmlSerializer1.Serialize(streamWriter, data);
				streamWriter.Flush();
			}*/
		}

		[Conditional("DEBUG")]
		public static void DebugWriteLine(string format, params object[] args)
		{
			Debug.WriteLine(format, args);
		}

		[Conditional("DEBUG")]
		public static void DebugWriteLine(string message)
		{
			Debug.WriteLine(message);
		}

		[Conditional("DEBUG")]
		private static void InitDebug()
		{
			//DefaultTraceListenerオブジェクトを取得
			DefaultTraceListener drl = (DefaultTraceListener)Trace.Listeners["Default"];
			//LogFileNameを変更する
			string dir = GetDllFolder();

			drl.LogFileName = Path.Combine(dir, "debug.txt");
		}

		private static string GetDllFolder()
		{
			return Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;
		}
	}
}
