using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
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
		public ListCollectionView FilteredShips { get; protected set; }

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
		/// <param name="ships">ships</param>
		/// <param name="filter">filter</param>
		/// <param name="filter_names">filter_names</param>
		/// <param name="sort_names">sort_names</param>
		public virtual void Update(IEnumerable<Ship> ships, Func<Ship, bool> filter, IEnumerable<string> filter_names, IEnumerable<string> sort_names)
		{
			this.Ships = ships.Select((s, i) => new ShipViewModel(s, i + 1)).ToArray();
			this.FilteredShips = new ListCollectionView(this.Ships)
			{
				Filter = obj => filter(((ShipViewModel)obj).Ship),
				IsLiveFiltering = true,
				IsLiveSorting = true,
			};

			foreach (var f in filter_names)
			{
				this.FilteredShips.LiveFilteringProperties.Add(f);
			}
			foreach (var s in sort_names)
			{
				if (this.IsAscending == true)
					this.FilteredShips.SortDescriptions.Add(new SortDescription(s, ListSortDirection.Ascending));
				else
					this.FilteredShips.SortDescriptions.Add(new SortDescription(s, ListSortDirection.Descending));

				this.FilteredShips.LiveSortingProperties.Add(s);
			}
			RaisePropertyChanged(nameof(FilteredShips));
			RaisePropertyChanged(nameof(Count));
		}

		/// <summary>
		/// 昇順、降順を切り替え（あとまわし）
		/// </summary>
		protected virtual void ReverseShips()
		{
			for (var i = 0; i < this.FilteredShips.SortDescriptions.Count; i++)
			{
				var s = FilteredShips.SortDescriptions[i];
				if (this.IsAscending == true)
					s.Direction = ListSortDirection.Ascending;
				else
					s.Direction = ListSortDirection.Descending;
			}
			RaisePropertyChanged(nameof(FilteredShips));
		}
	}
}
