using Livet;
using MetroRadiance.Interop.Win32;
using MetroRadiance.UI.Controls;
using MetroTrilithon.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KanburaLike.Models.Settings
{
	/// <summary>
	/// ウインドウ設定用クラス
	/// </summary>
	/// <seealso cref="KanburaLike.Models.Settings.SettingsHost" />
	/// <seealso cref="MetroRadiance.UI.Controls.IWindowSettings" />
	public class WindowSetting : SerializableSetting, IWindowSettings
	{
		/// <summary>
		/// ウィンドウを常に最前面に表示するかどうかを示す設定値を取得します。
		/// </summary>
	
		#region TopMost変更通知プロパティ
		private bool _TopMost;

		public bool TopMost
		{
			get
			{ return _TopMost; }
			set
			{ 
				if (_TopMost == value)
					return;
				_TopMost = value;
				RaisePropertyChanged(nameof(TopMost));
			}
		}
		#endregion

		#region Placementプロパティ
		private WINDOWPLACEMENT? _Placement;
		public WINDOWPLACEMENT? Placement
		{
			get
			{ return _Placement; }
			set
			{
				if (_Placement.Equals(value))
					return;
				_Placement = value;
			}
		}
		#endregion

		public override string CategoryName { get; }

		public WindowSetting() : this(null) { }

		public WindowSetting(string key)
		{
			this.CategoryName = key ?? this.GetType().Name;
		}

		/// <summary>
		/// IWindowSettings.Reload
		/// </summary>
		public virtual void Reload()
		{

		}

		/// <summary>
		/// IWindowSettings.Save
		/// </summary>
		public virtual void Save()
		{

		}
	}
}
