using Grabacr07.KanColleWrapper.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.ViewModels
{
	class ShipsViewModel : Livet.ViewModel
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

		public void Update(IEnumerable<Ship> ships)
		{
			this.Ships = ships.Select((s, i) => new ShipViewModel(s, i + 1)).ToArray();
		}
	}
}
