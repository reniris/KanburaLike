using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using Livet;
using Livet.EventListeners;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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

		#region IsAscending変更通知プロパティ
		private bool _IsAscending = true;

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

		public DispatcherCollection<ShipViewModel> Ships { get; } = new DispatcherCollection<ShipViewModel>(DispatcherHelper.UIDispatcher);
		
		/// <summary>
		/// 隻数
		/// </summary>
		public int Count => (Ships != null) ? Ships.Count : 0;

		public ShipsViewModel()
		{

		}

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

		/// <summary>
		/// ソートして更新
		/// </summary>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <param name="ships">ships</param>
		/// <param name="sort">sort</param>
		public void SortedUpdate<TKey>(IEnumerable<Ship> ships, Func<Ship, TKey> sort)
		{
			if (this.IsAscending == true)
				Update(ships.OrderBy(sort));
			else
				Update(ships.OrderByDescending(sort));
		}

		/// <summary>
		/// 昇順、降順を切り替え（SortedUpdateでソートされてるはずなのでここでは逆順にするだけ）
		/// </summary>
		public void ReverseShips()
		{
			this.Ships.Clear();
			foreach (var s in this.Ships.Reverse())
			{
				this.Ships.Add(s);	//めんどいのでIndexは変えない
			}
			RaisePropertyChanged(nameof(Ships));
		}
	}
}
