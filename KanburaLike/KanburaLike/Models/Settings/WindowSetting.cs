using MetroRadiance.Interop.Win32;
using MetroRadiance.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.Models.Settings
{
	/// <summary>
	/// ウインドウ設定用抽象クラス
	/// </summary>
	/// <seealso cref="KanburaLike.Models.Settings.SettingsHost" />
	/// <seealso cref="MetroRadiance.UI.Controls.IWindowSettings" />
	public abstract class WindowSetting : SettingsHost, IWindowSettings
	{

		#region Topmost変更通知プロパティ
		/// <summary>
		/// を保持するフィールド
		/// </summary>
		private bool _Topmost;

		/// <summary>
		/// の真偽値を保持するプロパティ
		/// </summary>
		public bool Topmost
		{
			get
			{ return _Topmost; }
			set
			{
				if (_Topmost == value)
					return;
				_Topmost = value;

				RaisePropertyChanged(nameof(Topmost));
			}
		}
		#endregion

		#region Placementプロパティ
		/// <summary>
		/// を保持するフィールド
		/// </summary>
		private WINDOWPLACEMENT? _Placement;

		/// <summary>
		/// を保持するプロパティ
		/// </summary>
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

		/// <summary>
		/// SettingsHost.SettingDataに入っているインスタンスをとる
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		protected T GetSettingData<T>() where T : WindowSetting
		{
			return SettingData.TryGetValue(typeof(T), out SettingsHost host) ? (T)host : null;
		}

		/// <summary>
		/// IWindowSettings.Reload
		/// </summary>
		public virtual void Reload()
		{

		}

		/// <summary>
		/// SettingsHost.SettingDataから読み出す
		/// </summary>
		/// <typeparam name="T"></typeparam>
		protected virtual void Reload<T>() where T : WindowSetting
		{
			KanColleModel.DebugWriteLine($"WindowSetting Load Topmost={Topmost} {Placement.Value.normalPosition.Left}");

			var data = GetSettingData<T>();
			this.Topmost = data.Topmost;

			if (data.Placement.HasValue)
				this.Placement = data.Placement;

			KanColleModel.DebugWriteLine($"SettingValue Topmost={data.Topmost} {data.Placement.Value.normalPosition.Left}");
		}

		/// <summary>
		/// IWindowSettings.Save
		/// </summary>
		public virtual void Save()
		{

		}

		/// <summary>
		/// SettingsHost.SettingDataへ書き出す
		/// </summary>
		/// <typeparam name="T"></typeparam>
		protected virtual void Save<T>() where T : WindowSetting
		{
			KanColleModel.DebugWriteLine($"WindowSetting Save Topmost={Topmost} {Placement.Value.normalPosition.Left}");

			var data = GetSettingData<T>();
			data.Topmost = this.Topmost;

			if (this.Placement.HasValue)
				data.Placement = this.Placement;

			KanColleModel.DebugWriteLine($"SettingValue Topmost={data.Topmost} {data.Placement.Value.normalPosition.Left}");
		}
	}

	/// <summary>
	/// 情報ウインドウ設定用クラス
	/// </summary>
	/// <seealso cref="KanburaLike.Models.Settings.WindowSetting" />
	public class InformationWindowSetting : WindowSetting
	{
		/// <summary>
		/// SettingsHost.SettingDataから読み出す
		/// </summary>
		public override void Reload()
		{
			base.Reload<InformationWindowSetting>();
		}

		/// <summary>
		/// SettingsHost.SettingDataへ書き出す
		/// </summary>
		public override void Save()
		{
			base.Save<InformationWindowSetting>();
		}
	}
}
