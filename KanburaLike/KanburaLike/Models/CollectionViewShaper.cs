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
	/// ファクトリ
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

	public class CollectionViewShaper<TSource>
	{
		private readonly ListCollectionView _view;
		private Predicate<object> _filter;
		private readonly List<SortDescription> _sortDescriptions;
		private readonly List<GroupDescription> _groupDescriptions;
		private readonly List<string> _filterDescriptions;
		private IComparer _customComparer;

		private static ConcurrentDictionary<Tuple<string, string>, Func<TSource, bool>> filter_cache = new ConcurrentDictionary<Tuple<string, string>, Func<TSource, bool>>();

		public ICollectionViewLiveShaping LiveView => _view;
		public IEnumerable View => _view;
		public int Count => (_view != null) ? _view.Count : 0;

		public CollectionViewShaper(ListCollectionView view)
		{
			_view = view ?? throw new ArgumentNullException("view");
			_filter = view.Filter;
			_sortDescriptions = view.SortDescriptions.ToList();
			_groupDescriptions = view.GroupDescriptions.ToList();

			if (LiveView.CanChangeLiveFiltering == true)
			{
				_filterDescriptions = LiveView.LiveFilteringProperties.ToList();
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
				if (LiveView.CanChangeLiveFiltering == true)
				{
					LiveView.LiveFilteringProperties.Clear();
					_filterDescriptions.ForEach(p => LiveView.LiveFilteringProperties.Add(p));
				}

				//ソート
				_view.SortDescriptions.Clear();
				LiveView.LiveSortingProperties.Clear();
				foreach (var s in _sortDescriptions)
				{
					_view.SortDescriptions.Add(s);

					if (LiveView.CanChangeLiveSorting == true)
						LiveView.LiveSortingProperties.Add(s.PropertyName);
				}

				//グルーピング
				_view.GroupDescriptions.Clear();
				LiveView.LiveGroupingProperties.Clear();
				foreach (var g in _groupDescriptions)
				{
					_view.GroupDescriptions.Add(g);

					if (LiveView.CanChangeLiveGrouping == true)
						LiveView.LiveGroupingProperties.Add((g as PropertyGroupDescription)?.PropertyName);
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

		public CollectionViewShaper<TSource> Where(Expression<Func<TSource, bool>> predicate)
		{
			var path = GetPath(predicate.Body);
			var key = string.Join(":", path);

			var f = filter_cache.GetOrAdd(Tuple.Create(nameof(TSource), key), predicate.Compile());

			_filter = o => f((TSource)o);
			path.ForEach(p => _filterDescriptions.Add(p));

			return this;
		}

		public CollectionViewShaper<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
		{
			return OrderBy(keySelector, true, ListSortDirection.Ascending);
		}

		public CollectionViewShaper<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector, IComparer Comparer)
		{
			return OrderBy(keySelector, true, ListSortDirection.Ascending, Comparer);
		}

		public CollectionViewShaper<TSource> OrderBy<TKey>(Expression<Func<TSource, TKey>> keySelector, Comparison<TSource> Comparer)
		{
			return OrderBy(keySelector, true, ListSortDirection.Ascending, Comparer<TSource>.Create(Comparer));
		}

		public CollectionViewShaper<TSource> OrderByDescending<TKey>(Expression<Func<TSource, TKey>> keySelector)
		{
			return OrderBy(keySelector, true, ListSortDirection.Descending);
		}

		public CollectionViewShaper<TSource> ThenBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
		{
			return OrderBy(keySelector, false, ListSortDirection.Ascending);
		}

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

		public CollectionViewShaper<TSource> GroupBy<TKey>(Expression<Func<TSource, TKey>> keySelector)
		{
			string path = GetPropertyPath(keySelector.Body);
			_groupDescriptions.Add(new PropertyGroupDescription(path));

			return this;
		}

		private static List<string> GetPath(Expression exp)
		{
			List<string> list = new List<string>();
			string path;
			switch (exp)
			{
				case BinaryExpression _:
					var binary = exp as BinaryExpression;
					var left = binary?.Left;
					var right = binary?.Right;
					list.AddRange(LeftPropertyPath(left));
					list.AddRange(LeftPropertyPath(right));
					break;

				case UnaryExpression _:
					var u = exp as UnaryExpression;
					path = GetPropertyPath(u.Operand);
					if (path != null)
						list.Add(path);
					break;

				case MethodCallExpression _:
					list.Add(MethodPropertyPath(exp));
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

		private static List<string> LeftPropertyPath(Expression left)
		{
			if (left == null)
				return null;

			var list = new List<string>();
			while (left is BinaryExpression l)
			{
				left = l.Left;
				list.AddRange(LeftPropertyPath(l.Right));
			}
			var path = GetPath(left);
			if (path != null)
				list.AddRange(path);

			return list;
		}

		private static string MethodPropertyPath(Expression method)
		{
			var m = method as MethodCallExpression;
			return GetPropertyPath(m.Object);
		}

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
