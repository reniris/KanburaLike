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

		#region IsAscending2変更通知プロパティ
		protected bool _IsAscending2 = true;

		public bool IsAscending2
		{
			get
			{ return _IsAscending2; }
			set
			{
				if (_IsAscending2 == value)
					return;
				_IsAscending2 = value;

				ReverseShips2();
				RaisePropertyChanged(nameof(IsAscending2));
			}
		}
		#endregion

		public ShipViewModel[] Ships { get; protected set; }
		public CollectionViewShaper<ShipViewModel> FilteredShips { get; protected set; }

		/// <summary>
		/// 隻数
		/// </summary>
		public int Count => (FilteredShips != null) ? FilteredShips.Count : 0;

		/// <summary>
		/// ソート用プロパティ名
		/// </summary>
		private string SortPropertyName = null;
		/// <summary>
		/// ソート用プロパティ名２
		/// </summary>
		private string SortPropertyName2 = null;

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
		public virtual void Update<TKey>(IEnumerable<Ship> ships, Expression<Func<ShipViewModel, bool>> filter
			, Expression<Func<ShipViewModel, TKey>> sortSelector, Expression<Func<ShipViewModel, TKey>> sortSelector2 = null)
		{
			this.Ships = ships.Select((s, i) => new ShipViewModel(s, i + 1)).ToArray();

			this.FilteredShips = CollectionViewShaper.Create<ShipViewModel>(this.Ships);

			this.FilteredShips.LiveShaping.IsLiveFiltering = true;
			this.FilteredShips.LiveShaping.IsLiveSorting = true;

			FilteredShips.OrderBy(sortSelector, IsAscending).ThenBy(sortSelector2, IsAscending2).Where(filter).Apply();

			//ソート用プロパティ名を保存
			var props = this.FilteredShips.LiveShaping.LiveSortingProperties;
			this.SortPropertyName = props.FirstOrDefault();
			this.SortPropertyName2 = props.Skip(1).FirstOrDefault();

			RaisePropertyChanged(nameof(FilteredShips));
			RaisePropertyChanged(nameof(Count));
		}

		/// <summary>
		/// 昇順、降順を切り替え
		/// </summary>
		protected virtual void ReverseShips()
		{
			//実装はあとで
			RaisePropertyChanged(nameof(FilteredShips));
		}

		/// <summary>
		/// 昇順、降順を切り替え2
		/// </summary>
		protected virtual void ReverseShips2()
		{
			//実装はあとで
			RaisePropertyChanged(nameof(FilteredShips));
		}
	}
}
