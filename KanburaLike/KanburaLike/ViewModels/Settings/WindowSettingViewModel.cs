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
	public class WindowSettingViewModel : Livet.ViewModel
	{
		#region Setting変更通知プロパティ
		private WindowSetting _Setting;

		public WindowSetting Setting
		{
			get
			{ return _Setting; }
			set
			{ 
				if (_Setting == value)
					return;
				_Setting = value;
				RaisePropertyChanged(nameof(Setting));
			}
		}
		#endregion

		public WindowSettingViewModel()
		{
			this.Setting = Models.Settings.SettingsHost.Instance<WindowSetting>();
		}
	}
}
