using Grabacr07.KanColleWrapper.Models;
using Livet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.ViewModels
{
	public class ShipsViewModel : Livet.ViewModel
	{
		#region IsExpanded変更通知プロパティ
		private bool _IsExpanded = true;

		public bool IsExpanded
		{
			get
			{ return _IsExpanded; }
			set
			{
				if (_IsExpanded == value)
					return;
				_IsExpanded = value;
				RaisePropertyChanged(nameof(IsExpanded));
			}
		}
		#endregion

		#region Ships変更通知プロパティ
		private IList<ShipViewModel> _Ships;

		public IList<ShipViewModel> Ships
		{
			get
			{ return _Ships; }
			set
			{
				if (_Ships == value)
					return;
				_Ships = value;
				RaisePropertyChanged(nameof(Ships));
			}
		}
		#endregion

		/// <summary>
		/// 隻数
		/// </summary>
		public int Count => (Ships != null) ? Ships.Count : 0;

		public void Update(IEnumerable<Ship> ships)
		{
			this.Ships = ships.Select((s, i) => new ShipViewModel(s, i + 1)).ToArray();
			RaisePropertyChanged(nameof(Count));
		}

		public ShipsViewModel()
		{
			RaisePropertyChanged();
		}
	}
}
