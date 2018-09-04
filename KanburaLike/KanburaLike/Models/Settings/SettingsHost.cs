using MetroRadiance.Interop.Win32;
using MetroRadiance.UI.Controls;
using MetroTrilithon.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Xml.Serialization;

namespace KanburaLike.Models.Settings
{
	public static class SettingsHost
	{
		private static Dictionary<string, SerializableSetting> cached_instances = new Dictionary<string, SerializableSetting>();
		private static readonly object _sync = new object();

		/// <summary>
		/// 現在のインスタンスにキャッシュされている <see cref="{T}"/>
		/// を取得します。 キャッシュがない場合は <see cref="create"/> に従って生成します。
		/// </summary>
		/// <returns></returns>
		public static T Cache<T>(Func<string, T> create, string key) where T : SerializableSetting
		{
			if (cached_instances.TryGetValue(key, out SerializableSetting obj) && obj is T) return (T)obj;

			var property = create(key);
			cached_instances[key] = property;

			return property;
		}

		#region Load / Save

		/// <summary>
		/// ファイルからロード
		/// </summary>
		public static void LoadFile()
		{
			try
			{
				if (File.Exists(SettingPath.Current))
				{
					using (var stream = new FileStream(SettingPath.Current, FileMode.Open, FileAccess.Read))
					{
						lock (_sync)
						{
							// 読み込み
							var serializer = new XmlSerializer(typeof(SettingsRoot));
							var root = (SettingsRoot)serializer.Deserialize(stream);
							var dic = root.Keys.Zip(root.Values, (k, v) => new KeyValuePair<string, SerializableSetting>(k, v))
								.ToDictionary(kv => kv.Key, kv => kv.Value);

							cached_instances = dic;
						}
					}
				}
				else
				{
					lock (_sync)
					{
						cached_instances = new Dictionary<string, SerializableSetting>();
					}
				}
			}
			catch (Exception)
			{
				File.Delete(SettingPath.Current);
				cached_instances = new Dictionary<string, SerializableSetting>();
			}
		}

		/// <summary>
		/// ファイルにセーブ
		/// </summary>
		public static void SaveFile()
		{
			try
			{
				if (cached_instances.Count == 0) return;

				lock (_sync)
				{
					using (var stream = new FileStream(SettingPath.Current, FileMode.Create, FileAccess.ReadWrite))
					using (var writer = new StreamWriter(stream, Encoding.UTF8))
					{
						SettingsRoot root = new SettingsRoot
						{
							Keys = cached_instances.Keys.ToArray(),
							Values = cached_instances.Values.ToArray()
						};
						var serializer = new XmlSerializer(typeof(SettingsRoot));
						serializer.Serialize(writer, root);

						writer.Flush();
					}
				}
			}
			catch (Exception ex)
			{
				DebugModel.WriteLine(ex);
			}
		}

		#endregion
	}
}
