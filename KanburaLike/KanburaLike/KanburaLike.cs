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
	[ExportMetadata("Version", "0.4")]
	[ExportMetadata("Author", "reniris")]
	[Export(typeof(ISettings))]
	class KanburaLike : IPlugin, ISettings, IDisposable
	{
		private Views.InformationWindow info;
		private ViewModels.InformationWindowViewModel infovm;

		public KanburaLike()
		{
		}

		public object View => new Views.Settings { };

		public void Initialize()
		{
			DebugModel.WriteLine("KanburaLike Init");
			SettingsHost.LoadFile();
			infovm = new ViewModels.InformationWindowViewModel(nameof(Views.InformationWindow));

			info = new Views.InformationWindow
			{
				DataContext = infovm
			};
		}

		public void Dispose()
		{
			if (info.IsVisible == true)
				info.Close();

			SettingsHost.SaveFile();

			DebugModel.Dump(infovm.Info.Brilliant.Ships, nameof(infovm.Info.Brilliant));
			DebugModel.WriteLine("KanburaLike Dispose");
		}
	}
}
