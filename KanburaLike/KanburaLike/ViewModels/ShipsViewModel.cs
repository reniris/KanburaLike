using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using Livet;
using Livet.EventListeners;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.ViewModels
{
	public class ShipsViewModel : Livet.ViewModel
	{
		#region IsExpanded変更通知プロパティ
		private bool _IsExpanded = false;

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

		public DispatcherCollection<ShipViewModel> Ships { get; } = new DispatcherCollection<ShipViewModel>(DispatcherHelper.UIDispatcher);

		/// <summary>
		/// 隻数
		/// </summary>
		public int Count => (Ships != null) ? Ships.Count : 0;

		/// <summary>
		/// 更新
		/// </summary>
		/// <param name="ships">ships</param>
		public void Update(IEnumerable<Ship> ships)
		{
			this.Ships.Clear();
			foreach (var s in ships.Select((s, i) => new ShipViewModel(s, i + 1)))
			{
				this.Ships.Add(s);
			}

			RaisePropertyChanged(nameof(Ships));
			RaisePropertyChanged(nameof(Count));
		}

		public ShipsViewModel()
		{

		}
	}
}
