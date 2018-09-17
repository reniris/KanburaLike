using Livet;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace KanburaLike.Models
{
	//https://www.thomaslevesque.com/2011/11/30/wpf-using-linq-to-shape-data-in-a-collectionview/
	//をもとに作成しました

	/// <summary>
	/// CollectionViewShaperファクトリ
	/// </summary>
	public static class CollectionViewShaper
	{
		public static CollectionViewShaper<TSource> Create<TSource>(this IList source)
		{
			var view = new ListCollectionView(source);
			return new CollectionViewShaper<TSource>(view);
		}

		public static CollectionViewShaper<TSource> Create<TSource>(this ListCollectionView view)
		{
			return new CollectionViewShaper<TSource>(view);
		}
	}

	/// <summary>
	/// ListCollectionViewのフィルター、グルーピング、ソートをラムダ式で設定できるようにするクラス
	/// Live Shapingにも対応しています
	/// </summary>
	/// <typeparam name="TSource">The type of the source.</typeparam>
	public class CollectionViewShaper<TSource>
	{
		private readonly ListCollectionView _view;
		private Predicate<object> _filter;
		private readonly List<SortDescription> _sortDescriptions;
		private readonly List<GroupDescription> _groupDescriptions;
		private readonly List<string> _filterDescriptions;
		private IComparer _customComparer;

		private static ConcurrentDictionary<Tuple<string, string>, Func<TSource, bool>> filter_cache = new ConcurrentDictionary<Tuple<string, string>, Func<TSource, bool>>();

		/// <summary>
		/// Live Shaping用インターフェース
		/// </summary>
		public ICollectionViewLiveShaping LiveShaping => _view;
		/// <summary>
		/// バインド用
		/// </summary>
		public IEnumerable View => _view;
		/// <summary>
		/// フィルター済みの件数
		/// </summary>
		public int Count => (_view != null) ? _view.Count : 0;

		internal CollectionViewShaper(ListCollectionView view)
		{
			_view = view ?? throw new ArgumentNullException("view");
			_filter = view.Filter;
			_sortDescriptions = view.SortDescriptions.ToList();
			_groupDescriptions = view.GroupDescriptions.ToList();

			if (LiveShaping.CanChangeLiveFiltering == true)
			{
				_filterDescriptions = LiveShaping.LiveFilteringProperties.ToList();
			}

			_customComparer = view.CustomSort;
		}

		/// <summary>
		/// フィルター、グルーピング、ソートを適用する
		/// 一回はこれを呼ばないと何も起こらないので注意
		/// </summary>
		public void Apply()
		{
			using (_view.DeferRefresh())
			{
				//フィルター
				_view.Filter = _filter;
				if (LiveShaping.CanChangeLiveFiltering == true)
				{
					LiveShaping.LiveFilteringProperties.Clear();
					_filterDescriptions.ForEach(p => LiveShaping.LiveFilteringProperties.Add(p));
				}

				//ソート
				_view.SortDescriptions.Clear();
				LiveShaping.LiveSortingProperties.Clear();
				foreach (var s in _sortDescriptions)
				{
					_view.SortDescriptions.Add(s);

					if (LiveShaping.CanChangeLiveSorting == true)
						LiveShaping.LiveSortingProperties.Add(s.PropertyName);
				}

				//グルーピング
				_view.GroupDescriptions.Clear();
				LiveShaping.LiveGroupingProperties.Clear();
				foreach (var g in _groupDescriptions)
				{
					_view.GroupDescriptions.Add(g);

					if (LiveShaping.CanChangeLiveGrouping == true)
						LiveShaping.LiveGroupingProperties.Add((g as PropertyGroupDescription)?.PropertyName);
				}

				//カスタムソート
				_view.CustomSort = _customComparer;
			}
		}

		/// <summary>
		/// ソート反転
		/// </summary>
		public void ReverseSort()
		{
			_view.SortDescriptions.Clear();
			for (int i = 0; i < _sortDescriptions.Count(); i++)
			{
				var s = _sortDescriptions[i];
				var rev = new SortDescription
				{
					PropertyName = s.PropertyName
				};
				if (s.Direction == ListSortDirection.Ascending)
					rev.Direction = ListSortDirection.Descending;
				else
					rev.Direction = ListSortDirection.Ascending;

				_view.SortDescriptions.Add(rev);

				_sortDescriptions[i] = rev;
			}
		}

		public CollectionViewShaper<TSource> ClearGrouping()
		{
			_groupDescriptions.Clear();

			return this;
		}

		public CollectionViewShaper<TSource> ClearSort()
		{
			_sortDescriptions.Clear();

			return this;
		}

		public CollectionViewShaper<TSource> ClearFilter()
		{
			_filter = null;
			_filterDescriptions.Clear();

			return this;
		}

		public CollectionViewShaper<TSource> ClearAll()
		{
			ClearFilter();
			ClearSort();
			ClearGrouping();

			return this;
		}

		/// <summary>
		/// フィルターをセットする
		/// 適用するためにはApply()を呼ぶ必要がある
		/// </summary>
		/// <param name="predicate">フィルター式</param>
		/// <returns></returns>
		public CollectionViewShaper<TSource> Where(Expression<Func<TSource, bool>> predicate)
		{
			var path = GetSwitchPropertyPath(predicate.Body);
			var key = string.Join(":", path);

			var f = filter_cache.GetOrAdd(Tuple.Create(nameof(TSource), key), predicate.Compile());

			_filter = o => f((TSource)o);
			path.ForEach(p => _filterDescriptions.Add(p));

			return this;
		}

		/// <summary>
		/// 昇順ソート
		/// 適用するためにはApply()を呼ぶ必要がある
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="keySelector">ソート用キーセレクタ</param>
		/// <returns></returns>
		public CollectionViewShaper<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
		{
			return OrderBy(keySelector, true, ListSortDirection.Ascending);
		}

		/// <summary>
		/// 昇順ソート
		/// 適用するためにはApply()を呼ぶ必要がある
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="keySelector">ソート用キーセレクタ</param>
		/// <param name="Comparer">ソート用比較インターフェース</param>
		/// <returns></returns>
		public CollectionViewShaper<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector, IComparer Comparer)
		{
			return OrderBy(keySelector, true, ListSortDirection.Ascending, Comparer);
		}

		/// <summary>
		/// 昇順ソート
		/// 適用するためにはApply()を呼ぶ必要がある
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="keySelector">ソート用キーセレクタ</param>
		/// <param name="Comparer">ソート用比較ラムダ式</param>
		/// <returns></returns>
		public CollectionViewShaper<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector, Comparison<TSource> Comparer)
		{
			return OrderBy(keySelector, true, ListSortDirection.Ascending, Comparer<TSource>.Create(Comparer));
		}

		/// <summary>
		/// 降順ソート
		/// 適用するためにはApply()を呼ぶ必要がある
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="keySelector">ソート用キーセレクタ</param>
		/// <returns></returns>
		public CollectionViewShaper<TSource> OrderByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector)
		{
			return OrderBy(keySelector, true, ListSortDirection.Descending);
		}

		/// <summary>
		/// 後続の要素を昇順ソート
		/// 適用するためにはApply()を呼ぶ必要がある
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="keySelector">ソート用キーセレクタ</param>
		/// <returns></returns>
		public CollectionViewShaper<TSource> ThenBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
		{
			return OrderBy(keySelector, false, ListSortDirection.Ascending);
		}

		/// <summary>
		/// 後続の要素を降順ソート
		/// 適用するためにはApply()を呼ぶ必要がある
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="keySelector">ソート用キーセレクタ</param>
		/// <returns></returns>
		public CollectionViewShaper<TSource> ThenByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector)
		{
			return OrderBy(keySelector, false, ListSortDirection.Descending);
		}

		private CollectionViewShaper<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector, bool clear, ListSortDirection direction)
		{
			string path = GetPropertyPath(keySelector.Body);
			if (clear)
				ClearSort();

			_sortDescriptions.Add(new SortDescription(path, direction));

			return this;
		}

		private CollectionViewShaper<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector, bool clear, ListSortDirection direction, IComparer Comparer)
		{
			string path = GetPropertyPath(keySelector.Body);
			_customComparer = Comparer;

			if (clear)
				_sortDescriptions.Clear();

			_sortDescriptions.Add(new SortDescription(path, direction));
			return this;
		}

		/// <summary>
		/// グルーピング
		/// </summary>
		/// <typeparam name="TKey"></typeparam>
		/// <param name="keySelector">グルーピング用キーセレクタ</param>
		/// <returns></returns>
		public CollectionViewShaper<TSource> GroupBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
		{
			string path = GetPropertyPath(keySelector.Body);
			_groupDescriptions.Add(new PropertyGroupDescription(path));

			return this;
		}

		/// <summary>
		/// 式からプロパティ名を取得する
		/// </summary>
		/// <param name="exp">式</param>
		/// <returns></returns>
		private static List<string> GetSwitchPropertyPath(Expression exp)
		{
			List<string> list = new List<string>();
			string path;
			switch (exp)
			{
				case BinaryExpression _:
					var binary = exp as BinaryExpression;
					var left = binary?.Left;
					var right = binary?.Right;
					list.AddRange(DecomposePropertyPath(left));
					list.AddRange(DecomposePropertyPath(right));
					break;

				case UnaryExpression _:
					var u = exp as UnaryExpression;
					path = GetPropertyPath(u.Operand);
					if (path != null)
						list.Add(path);
					break;

				case MethodCallExpression _:
					var m = exp as MethodCallExpression;
					path = GetPropertyPath(m.Object);
					if (path != null)
						list.Add(path);
					break;

				case MemberExpression _:
					path = GetPropertyPath(exp);
					if (path != null)
						list.Add(path);
					break;

				default:
					DebugModel.WriteLine(exp.Type.Name);
					return null;
			}

			return list;
		}

		/// <summary>
		/// 式を分解してプロパティ名を取得する
		/// </summary>
		/// <param name="left">式</param>
		/// <returns></returns>
		private static List<string> DecomposePropertyPath(Expression left)
		{
			if (left == null)
				return null;

			var list = new List<string>();
			while (left is BinaryExpression l)
			{
				left = l.Left;
				list.AddRange(DecomposePropertyPath(l.Right));
			}
			var path = GetSwitchPropertyPath(left);
			if (path != null)
				list.AddRange(path);

			return list;
		}

		/// <summary>
		/// 式からプロパティ名を取得する
		/// </summary>
		/// <param name="expression">式</param>
		/// <returns></returns>
		/// <exception cref="ArgumentException">The selector body must contain only property or field access expressions</exception>
		private static string GetPropertyPath(Expression expression)
		{
			var names = new Stack<string>();
			var expr = expression;
			while (expr != null && !(expr is ParameterExpression) && !(expr is ConstantExpression))
			{
				if (!(expr is MemberExpression memberExpr))
					throw new ArgumentException("The selector body must contain only property or field access expressions");

				if ((memberExpr.Member as FieldInfo)?.IsInitOnly == true)
					return null;

				names.Push(memberExpr.Member.Name);
				expr = memberExpr.Expression;
			}
			return String.Join(".", names.ToArray());
		}
	}
}
