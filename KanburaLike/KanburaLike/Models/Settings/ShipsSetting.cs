using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.Models.Settings
{
	/// <summary>
	/// 船団の設定を表すクラス
	/// </summary>
	/// <seealso cref="KanburaLike.Models.Settings.SerializableSetting" />
	public class ShipsSetting : SerializableSetting
	{
		/// <summary>
		/// 表示するかどうか
		/// </summary>
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

		/// <summary>
		/// ソートが昇順かどうか
		/// </summary>
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

				RaisePropertyChanged(nameof(IsAscending));
			}
		}
		#endregion
		/// <summary>
		/// ２番目のソートが昇順かどうか
		/// </summary>
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

				RaisePropertyChanged(nameof(IsAscending2));
			}
		}
		#endregion

		public override string CategoryName { get; }

		public ShipsSetting() : this(null) { }

		public ShipsSetting(string key)
		{
			this.CategoryName = key ?? this.GetType().Name;
		}
	}
}
