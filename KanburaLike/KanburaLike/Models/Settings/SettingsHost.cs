using MetroRadiance.Interop.Win32;
using MetroRadiance.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Serialization;

namespace KanburaLike.Models.Settings
{
	public abstract class SettingsHost : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			KanColleModel.DebugWriteLine($"Changed {propertyName}");
		}

		/// <summary>
		/// 設定データ
		/// </summary>
		protected static Dictionary<Type, SettingsHost> SettingData { get; set; } = new Dictionary<Type, SettingsHost>();

		/// <summary>
		/// ファイル書き込み用設定データ
		/// </summary>
		protected static List<SettingsHost> SettingValue { get { return SettingData?.Values.ToList(); } }

		/// <summary>
		/// 設定ファイルのフルパス
		/// </summary>
		protected static readonly string fullpath = Path.Combine(GetDllFolder(), "KanburaLike.xml");

		private static XmlSerializer serializer;

		static SettingsHost()
		{
			var root = new XmlRootAttribute
			{
				ElementName = "KanburaLike",
				Namespace = String.Empty,
				IsNullable = true
			};
			var types = new Type[] { typeof(WindowSetting) };
			serializer = new XmlSerializer(typeof(List<SettingsHost>), null, types, root, "");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SettingsHost"/> class.
		/// </summary>
		protected SettingsHost()
		{
			SettingData[this.GetType()] = this;
		}

		/// <summary>
		/// 初期化（ファイルが読める場合はそれを読んで、読めない場合は何もしない）
		/// </summary>
		public static void Init()
		{
			if (System.IO.File.Exists(fullpath))
			{
				try
				{
					SettingData.Clear();
					LoadFile();
				}
				catch (Exception e)
				{
					KanColleModel.DebugWriteLine("SettingHost Init Exception");
					KanColleModel.DebugWriteLine(e);
				}
			}
			else
			{
				KanColleModel.DebugWriteLine("SettingHost File Not Found");
			}
		}

		/// <summary>
		/// ファイルから読み込み
		/// </summary>
		public static void LoadFile()
		{
			// 読み込み
			using (var inputStream = new FileStream(fullpath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				var data = (List<SettingsHost>)serializer.Deserialize(inputStream);

				var dic = data.ToDictionary(d => d.GetType());
				SettingData = dic;

				inputStream.Close();
			}
		}

		/// <summary>
		/// ファイルへ書き込み
		/// </summary>
		public static void SaveFile()
		{
			using (var outputStream = new FileStream(fullpath, FileMode.OpenOrCreate, FileAccess.Write))
			using (var writer = new StreamWriter(outputStream, Encoding.UTF8))
			{
				serializer.Serialize(writer, SettingValue);

				writer.Flush();
				writer.Close();
				outputStream.Close();
			}
			KanColleModel.DebugWriteLine("SettingHost Save");
		}

		/// <summary>
		/// このプラグインのあるフォルダを取得
		/// </summary>
		/// <returns></returns>
		public static string GetDllFolder() => Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;

		/// <summary>
		/// <typeparamref name="T"/> 型の設定オブジェクトの唯一のインスタンスを取得します。
		/// </summary>
		public static T Instance<T>() where T : SettingsHost, new() => SettingData.TryGetValue(typeof(T), out SettingsHost host) ? (T)host : new T();
	}

	public class WindowSetting : SettingsHost, IWindowSettings
	{

		#region Topmost変更通知プロパティ
		private bool _Topmost;

		public bool Topmost
		{
			get
			{ return _Topmost; }
			set
			{
				if (_Topmost == value)
					return;
				_Topmost = value;

				RaisePropertyChanged(nameof(Topmost));
			}
		}

		#region Placement変更通知プロパティ
		private WINDOWPLACEMENT? _Placement;

		public WINDOWPLACEMENT? Placement
		{
			get
			{ return _Placement; }
			set
			{
				if (_Placement.Equals(value))
					return;
				_Placement = value;
			}
		}
		#endregion

		protected WindowSetting data
		{
			get { return (WindowSetting)SettingData[GetType()]; }
		}

		public virtual void Reload()
		{
			KanColleModel.DebugWriteLine($"WindowSetting Load Topmost={Topmost} {Placement.Value.normalPosition.Left}");

			this.Topmost = data.Topmost;
			this.Placement = data.Placement;

			KanColleModel.DebugWriteLine($"SettingValue Topmost={data.Topmost} {data.Placement.Value.normalPosition.Left}");
		}

		public virtual void Save()
		{
			KanColleModel.DebugWriteLine($"WindowSetting Save Topmost={Topmost} {Placement.Value.normalPosition.Left}");

			data.Topmost = this.Topmost;
			data.Placement = this.Placement;

			KanColleModel.DebugWriteLine($"SettingValue Topmost={data.Topmost} {data.Placement.Value.normalPosition.Left}");
		}
		#endregion
	}
}
