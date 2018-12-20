using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.Models.Settings
{
	public class MaterialsSetting : SerializableSetting
	{
		/// <summary>
		/// 最小表示資材の種類
		/// </summary>
		public const int MinMaterialCount = 1;
		/// <summary>
		/// 最大表示資材の種類
		/// </summary>
		public const int MaxMaterialCount = 8;
		/// <summary>
		/// デフォルト表示資材の種類
		/// </summary>
		public const int DefaultMaterialCount = 4;

		#region CurrentMaterialCount変更通知プロパティ
		private int _CurrentMaterialCount = DefaultMaterialCount;

		/// <summary>
		/// 表示資材の種類
		/// </summary>
		public int CurrentMaterialCount
		{
			get
			{ return _CurrentMaterialCount; }
			set
			{
				//値チェック
				if (value < MinMaterialCount || MaxMaterialCount < value)
					return;
				if (_CurrentMaterialCount == value)
					return;
				_CurrentMaterialCount = value;

				ResizeSelectedItem();
				RaisePropertyChanged(nameof(CurrentMaterialCount));
			}
		}
		#endregion

		public string[] CurrentSelectedItem { get; set; } = null;

		private void ResizeSelectedItem()
		{
			if (CurrentSelectedItem == null)
			{
				CurrentSelectedItem = new string[CurrentMaterialCount];
			}
			else
			{
				string[] old = CurrentSelectedItem.ToArray();
				CurrentSelectedItem = new string[CurrentMaterialCount];
				for (int i = 0; i < CurrentMaterialCount; i++)
				{
					if (i < old.Length)
						CurrentSelectedItem[i] = old[i];
					else
						CurrentSelectedItem[i] = null;
				}
			}
		}

		public MaterialsSetting()
		{
			ResizeSelectedItem();
		}
	}
}
