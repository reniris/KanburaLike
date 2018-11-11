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
		public AdmiralViewModel Admiral { get; private set; } = new AdmiralViewModel();

		public SlotItemsViewModel SlotItems { get; private set; } = new SlotItemsViewModel();


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

	}
}
