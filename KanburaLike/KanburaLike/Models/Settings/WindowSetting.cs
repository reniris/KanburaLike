using MetroRadiance.Interop.Win32;
using MetroRadiance.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.Models.Settings
{
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
