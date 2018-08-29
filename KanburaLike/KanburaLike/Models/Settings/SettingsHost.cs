using Livet;
using Livet.EventListeners;
using MetroRadiance.Interop.Win32;
using MetroRadiance.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace KanburaLike.Models.Settings
{
	public abstract class SettingsHost : INotifyPropertyChanged, IDisposable
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		/// <summary>
		/// 設定データ
		/// </summary>
		private static Dictionary<Type, SettingsHost> SettingData = new Dictionary<Type, SettingsHost>();

		/// <summary>
		/// ファイル書き込み用設定データ
		/// </summary>
		private static List<SettingsHost> SettingValue { get { return SettingData?.Values.ToList(); } }

		private readonly LivetCompositeDisposable compositeDisposable = new LivetCompositeDisposable();

		/// <summary>
		/// 設定ファイルのフルパス
		/// </summary>
		private static readonly string fullpath = Path.Combine(GetDllFolder(), "KanburaLike.xml");

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
			this[this.GetType()] = this;

			var listener = new PropertyChangedEventListener(this);
			listener.RegisterHandler((s, e) => Save());
			compositeDisposable.Add(listener);
		}

		/// <summary>
		/// 初期化（ファイルが読める場合はそれを読んで、読めない場合はデフォルト値を書く）
		/// </summary>
		public static void Init()
		{
			SettingData.Clear();
			if (System.IO.File.Exists(fullpath))
			{
				try
				{
					Load();
				}
				catch
				{
					Save();
				}
			}
			else
			{
				Save();
			}
		}

		/// <summary>
		/// Gets the <see cref="SettingsHost"/> with the specified t.
		/// </summary>
		/// <value>
		/// The <see cref="SettingsHost"/>.
		/// </value>
		/// <param name="t">t</param>
		/// <returns></returns>
		public SettingsHost this[Type t]
		{
			get
			{
				return SettingData[t];
			}
			private set
			{
				SettingData[t] = value;
			}
		}

		/// <summary>
		/// ファイルから読み込み
		/// </summary>
		public static void Load()
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
		public static void Save()
		{
			using (var outputStream = new FileStream(fullpath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
			using (var writer = new StreamWriter(outputStream, Encoding.UTF8))
			{
				serializer.Serialize(writer, SettingValue);

				writer.Flush();
				writer.Close();
				outputStream.Close();
			}
		}

		public void Dispose()
		{
			this.compositeDisposable.Dispose();
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
				RaisePropertyChanged(nameof(Placement));
			}
		}
		#endregion

		public void Reload()
		{
			throw new NotImplementedException();
		}

		public new void Save()
		{
			throw new NotImplementedException();
		}
		#endregion

	}
}
