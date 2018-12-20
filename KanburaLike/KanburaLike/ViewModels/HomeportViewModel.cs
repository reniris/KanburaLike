using KanburaLike.Models.Settings;
using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.ViewModels
{
	public class HomeportViewModel : Livet.ViewModel
	{
		public HomeportSetting Setting { get; }

		public AdmiralViewModel Admiral { get; } = new AdmiralViewModel();

		public SlotItemsViewModel SlotItems { get; } = new SlotItemsViewModel();

		public MaterialsViewModel Materials { get; } = new MaterialsViewModel();

		#region ShipsCount変更通知プロパティ
		private int _ShipsCount;

		public int ShipsCount
		{
			get
			{ return _ShipsCount; }
			set
			{ 
				if (_ShipsCount == value)
					return;
				_ShipsCount = value;
				RaisePropertyChanged(nameof(ShipsCount));
			}
		}
		#endregion

		public HomeportViewModel()
		{
			Setting = SettingsHost.Cache<HomeportSetting>(k => new HomeportSetting(), nameof(HomeportSetting));
		}
	}
}
