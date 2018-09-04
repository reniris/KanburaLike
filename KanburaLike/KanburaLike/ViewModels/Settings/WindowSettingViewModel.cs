using KanburaLike.Models.Settings;
using Livet;
using Livet.EventListeners;
using MetroRadiance.Interop.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.ViewModels.Settings
{
	public abstract class WindowSettingViewModel : Livet.ViewModel
	{
		public WindowSetting Setting { get; }

		public WindowSettingViewModel(string key)
		{
			Setting = SettingsHost.Cache<WindowSetting>(k => new WindowSetting(key), key);
		}
	}
}
