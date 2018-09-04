using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.Models
{
	public static class SettingPath
	{
		/// <summary>
		/// このプラグインのあるフォルダを取得
		/// </summary>
		/// <returns></returns>
		public static string GetDllFolder() => Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;

		/// <summary>
		/// アセンブリ名
		/// </summary>
		public static readonly string DLLName = Assembly.GetExecutingAssembly().GetName().Name;

		/// <summary>
		/// 設定ファイル名
		/// </summary>
		private static readonly string SettingFileName = $"{DLLName}.xml";

		/// <summary>
		/// ローカルApplication Dataフォルダに 
		/// \grabacr.net\KanColleViewer\Plugins\KanburaLike\KanburaLike.xaml
		/// を繋げたもの（コンパイルオプションで使用可能）
		/// </summary>
		private static string LocalAppFilePath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
			"grabacr.net", "KanColleViewer", "Plugins", DLLName, SettingFileName);

		/// <summary>
		/// このプラグインがあるパスに
		/// \KanburaLike.xaml
		/// を繋げたもの（デフォルトはこれ）
		/// </summary>
		private static string PluginPath { get; } = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName,
			 SettingFileName);

#if SETTING_LOCALAPP
		public static string Current { get; } = LocalAppFilePath;
#else
		public static string Current { get; } = PluginPath;
#endif
	}
}
