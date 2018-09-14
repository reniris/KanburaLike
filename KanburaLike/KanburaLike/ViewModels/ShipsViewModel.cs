using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using KanburaLike.Models;
using Livet;
using Livet.EventListeners;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace KanburaLike.ViewModels
{
	public class ShipsViewModel : Livet.ViewModel
	{
		#region IsExpanded変更通知プロパティ
		protected bool _IsExpanded = false;

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

		#region IsAscending変更通知プロパティ
		protected bool _IsAscending = true;

		public bool IsAscending
		{
			get
			{ return _IsAscending; }
			set
			{
				if (_IsAscending == value)
					return;
				_IsAscending = value;

				ReverseShips();
				RaisePropertyChanged(nameof(IsAscending));
			}
		}
		#endregion

		public ShipViewModel[] Ships { get; protected set; }
		public CollectionViewShaper<ShipViewModel> FilteredShips { get; protected set; }

		/// <summary>
		/// 隻数
		/// </summary>
		public int Count => (FilteredShips != null) ? FilteredShips.Count : 0;

		public ShipsViewModel()
		{

		}

		/// <summary>
		/// 更新
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <param name="ships">ships</param>
		/// <param name="filter">filter</param>
		/// <param name="sortSelector">sortSelector</param>
		public virtual void Update<TKey>(IEnumerable<Ship> ships, Expression<Func<ShipViewModel, bool>> filter, Expression<Func<ShipViewModel, TKey>> sortSelector)
		{
			this.Ships = ships.Select((s, i) => new ShipViewModel(s, i + 1)).ToArray();

			FilteredShips = CollectionViewShaper.Create<ShipViewModel>(this.Ships);

			this.FilteredShips.LiveView.IsLiveFiltering = true;
			this.FilteredShips.LiveView.IsLiveSorting = true;

			if (this.IsAscending == true)
				FilteredShips.OrderBy(sortSelector);
			else
				FilteredShips.OrderByDescending(sortSelector);

			FilteredShips.Where(filter);
			FilteredShips.Apply();

			RaisePropertyChanged(nameof(FilteredShips));
			RaisePropertyChanged(nameof(Count));
		}

		/// <summary>
		/// 昇順、降順を切り替え（あとまわし）
		/// </summary>
		protected virtual void ReverseShips()
		{
			/*if (this.IsAscending == true)
				FilteredShips.OrderBy(sortSelector);
			else
				FilteredShips.OrderByDescending(sortSelector);
			*/
			RaisePropertyChanged(nameof(FilteredShips));
		}
	}
}
