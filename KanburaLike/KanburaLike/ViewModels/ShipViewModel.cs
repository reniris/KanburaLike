using Grabacr07.KanColleWrapper.Models;
using Livet;
using Livet.EventListeners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.ViewModels
{
	class ShipViewModel : Livet.ViewModel
	{

		#region Name変更通知プロパティ
		private string _Name;

		public string Name
		{
			get
			{ return _Name; }
			set
			{
				if (_Name == value)
					return;
				_Name = value;
				RaisePropertyChanged(nameof(Name));
			}
		}
		#endregion

		#region Index変更通知プロパティ
		private int _Index;

		public int Index
		{
			get
			{ return _Index; }
			set
			{
				if (_Index == value)
					return;
				_Index = value;
				RaisePropertyChanged(nameof(Index));
			}
		}
		#endregion


		#region Lv変更通知プロパティ
		private int _Lv;

		public int Lv
		{
			get
			{ return _Lv; }
			set
			{
				if (_Lv == value)
					return;
				_Lv = value;
				RaisePropertyChanged(nameof(Lv));
			}
		}
		#endregion


		#region ExpForNextLevel変更通知プロパティ
		private int _ExpForNextLevel;

		public int ExpForNextLevel
		{
			get
			{ return _ExpForNextLevel; }
			set
			{
				if (_ExpForNextLevel == value)
					return;
				_ExpForNextLevel = value;
				RaisePropertyChanged(nameof(ExpForNextLevel));
			}
		}
		#endregion

		/// <summary>
		/// デザイナ用<see cref="ShipViewModel"/> class.
		/// </summary>
		public ShipViewModel() { }

		/// <summary>
		/// コードからはこっちを使う <see cref="ShipViewModel"/> class.
		/// </summary>
		/// <param name="s">s</param>
		public ShipViewModel(Ship s, int i)
		{
			this.Name = s.Info.Name;
			this.Index = i;
			this.Lv = s.Level;
			this.ExpForNextLevel = s.ExpForNextLevel;
		}
	}
}
