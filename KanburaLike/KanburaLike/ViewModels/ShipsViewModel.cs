using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using KanburaLike.Models;
using KanburaLike.Models.Settings;
using Livet;
using Livet.EventListeners;
using MetroTrilithon.Mvvm;
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
		public ShipsSetting Setting { get; }

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

		public ShipsViewModel(string key)
		{
			Setting = SettingsHost.Cache<ShipsSetting>(k => new ShipsSetting(key), key);
			Setting.Subscribe(nameof(Setting.IsAscending), () => ReverseSort(), false);
			Setting.Subscribe(nameof(Setting.IsAscending2), () => ReverseSort2(), false);
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

			this.FilteredShips.OrderBy(sortSelector, Setting.IsAscending).ThenBy(sortSelector2, Setting.IsAscending2).Where(filter).Apply();

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
		protected virtual void ReverseSort()
		{
			//実装はあとで
			RaisePropertyChanged(nameof(FilteredShips));
		}

		/// <summary>
		/// 昇順、降順を切り替え2
		/// </summary>
		protected virtual void ReverseSort2()
		{
			//実装はあとで
			RaisePropertyChanged(nameof(FilteredShips));
		}
	}
}
