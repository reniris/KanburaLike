using Livet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.Models.Settings
{
	public class QuestsSetting : SerializableSetting
	{
		/// <summary>
		/// 展開するかどうか
		/// </summary>
		#region IsExpanded変更通知プロパティ
		protected bool _IsExpanded = true;

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

		#region QuestsID変更通知プロパティ
		private int[] _QuestsID;

		public int[] QuestsID
		{
			get
			{ return _QuestsID; }
			set
			{ 
				if (_QuestsID == value)
					return;
				_QuestsID = value;
				RaisePropertyChanged(nameof(QuestsID));
			}
		}
		#endregion
	}
}
