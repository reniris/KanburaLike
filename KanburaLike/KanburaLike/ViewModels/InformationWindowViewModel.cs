using KanburaLike.Models.Settings;
using KanburaLike.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.ViewModels
{
	public class InformationWindowViewModel : WindowSettingViewModel
	{
		public InformationViewModel Info { get; } = new InformationViewModel();

		public InformationWindowViewModel()
		{
			this.Setting = SettingsHost.Instance<InformationWindowSetting>();
		}
	}
}
