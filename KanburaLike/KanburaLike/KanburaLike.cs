using Grabacr07.KanColleViewer.Composition;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using KanburaLike.Models;
using KanburaLike.Models.Settings;
using Livet;
using MetroTrilithon.Lifetime;
using MetroTrilithon.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike
{
	[Export(typeof(IPlugin))]
	[ExportMetadata("Guid", "9143C600-3D98-4014-A8E9-406C68252611")]
	[ExportMetadata("Title", "KanburaLike")]
	[ExportMetadata("Description", "艦ぶらっぽいの")]
	[ExportMetadata("Version", "0.2")]
	[ExportMetadata("Author", "reniris")]
	[Export(typeof(ISettings))]
	class KanburaLike : IPlugin, ISettings
	{
		private Views.InformationWindow info;
		private ViewModels.InformationWindowViewModel infovm;

		public KanburaLike()
		{
		}

		public object View => new Views.Settings { };

		public void Initialize()
		{
			SettingsHost.Load();
			infovm = new ViewModels.InformationWindowViewModel();

			info = new Views.InformationWindow
			{
				DataContext = infovm
			};
			info.Show();
		}
	}
}
