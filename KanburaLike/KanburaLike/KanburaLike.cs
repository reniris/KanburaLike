using Grabacr07.KanColleViewer.Composition;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
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
	[ExportMetadata("Version", "0.1")]
	[ExportMetadata("Author", "reniris")]
	class KanburaLike : IPlugin
	{
		private Views.InformationWindow info;
		private ViewModels.InformationViewModel infovm = new ViewModels.InformationViewModel();
		private Models.KanColleModel kancolle;

		public KanburaLike()
		{
		}

		public void Initialize()
		{
			kancolle = new Models.KanColleModel();
			info = new Views.InformationWindow
			{
				DataContext = infovm
			};
			info.Show();
		}

		/// <summary>
		/// デバッグ用データ書き出し
		/// </summary>
		[Conditional("DEBUG")]
		private void DumpDebugData(object data, string filename)
		{
			var dir = Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName;

			// XAMLで書き出し
			var text = System.Windows.Markup.XamlWriter.Save(data);
			filename += ".xaml";
			System.IO.File.WriteAllText(Path.Combine(dir, filename), text);
		}
	}
}
