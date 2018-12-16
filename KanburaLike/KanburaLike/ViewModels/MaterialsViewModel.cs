using Grabacr07.KanColleWrapper;
using KanburaLike.Models.Settings;
using Livet;
using MetroTrilithon.Lifetime;
using MetroTrilithon.Mvvm;
using StatefulModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.ViewModels
{
	/// <summary>
	/// 資材ViewModel
	/// </summary>
	/// <seealso cref="Livet.ViewModel" />
	public class MaterialsViewModel : Livet.ViewModel
	{
		public MaterialsSetting Setting { get; }

		public IReadOnlyList<MaterialItemsViewModel> Items { get; }

		public MaterialsViewModel()
		{
			Setting = SettingsHost.Cache<MaterialsSetting>(k => new MaterialsSetting(), nameof(MaterialsSetting));

			Items = Enumerable.Repeat(new MaterialItemsViewModel(), Setting.CurrentMaterialCount).ToList().AsReadOnly();
			var defitems = MaterialItemsSource.Instance.Values.Reverse();
			for (int i = 0; i < Setting.CurrentMaterialCount; i++)
			{
				Items[i].SelectedItem = MaterialItemsSource.Instance.Values.FirstOrDefault(x => x.Key == Setting.CurrentSelectedItem[i]) ?? defitems.Skip(Setting.CurrentMaterialCount - i).First();

				Items[i].Subscribe(nameof(MaterialItemsViewModel.SelectedItem), () => Setting.CurrentSelectedItem[i] = Items[i].SelectedItem.Key).AddTo(this);
			}
		}
	}

	/// <summary>
	/// 資材コンボのItemsSource
	/// </summary>
	/// <seealso cref="Livet.ViewModel" />
	public class MaterialItemsSource : Livet.ViewModel
	{
		#region Singleton
		private static MaterialItemsSource _Instance = null;
		public static MaterialItemsSource Instance
		{
			get
			{
				if (_Instance == null)
					_Instance = new MaterialItemsSource();

				return _Instance;
			}
		}
		#endregion

		private Materials Model { get; set; }

		public ICollection<MaterialViewModel> Values { get; private set; }

		private MaterialItemsSource()
		{
			InitModel();
		}

		public void InitModel()
		{
			string[] names = new string[] { "燃料", "弾薬", "鋼鉄", "ボーキサイト", "開発資材", "高速修復材", "高速建造材", "改修資材" };

			Model = KanColleClient.Current.Homeport?.Materials;

			var fuel = new MaterialViewModel(nameof(Materials.Fuel), names[0]).AddTo(this);
			Model?.Subscribe(fuel.Key, () => fuel.Value = Model.Fuel).AddTo(this);

			var ammunition = new MaterialViewModel(nameof(Materials.Ammunition), names[1]).AddTo(this);
			Model?.Subscribe(ammunition.Key, () => ammunition.Value = Model.Ammunition).AddTo(this);

			var steel = new MaterialViewModel(nameof(Materials.Steel), names[2]).AddTo(this);
			Model?.Subscribe(steel.Key, () => steel.Value = Model.Steel).AddTo(this);

			var bauxite = new MaterialViewModel(nameof(Materials.Bauxite), names[3]).AddTo(this);
			Model?.Subscribe(bauxite.Key, () => bauxite.Value = Model.Bauxite).AddTo(this);

			var develop = new MaterialViewModel(nameof(Materials.DevelopmentMaterials), names[4]).AddTo(this);
			Model?.Subscribe(develop.Key, () => develop.Value = Model.DevelopmentMaterials).AddTo(this);

			var repair = new MaterialViewModel(nameof(Materials.InstantRepairMaterials), names[5]).AddTo(this);
			Model?.Subscribe(repair.Key, () => repair.Value = Model.InstantRepairMaterials).AddTo(this);

			var build = new MaterialViewModel(nameof(Materials.InstantBuildMaterials), names[6]).AddTo(this);
			Model?.Subscribe(build.Key, () => build.Value = Model.InstantBuildMaterials).AddTo(this);

			var improvement = new MaterialViewModel(nameof(Materials.ImprovementMaterials), names[7]).AddTo(this);
			Model?.Subscribe(improvement.Key, () => improvement.Value = Model.ImprovementMaterials).AddTo(this);

			Values = new List<MaterialViewModel>
			{
				fuel,
				ammunition,
				steel,
				bauxite,
				develop,
				repair,
				build,
				improvement,
			};
		}
	}

	/// <summary>
	/// 資材コンボのViewModel
	/// </summary>
	/// <seealso cref="Livet.ViewModel" />
	public class MaterialItemsViewModel : Livet.ViewModel
	{
		#region SelectedItem変更通知プロパティ
		private MaterialViewModel _SelectedItem = null;

		public MaterialViewModel SelectedItem
		{
			get
			{ return _SelectedItem; }
			set
			{
				if (_SelectedItem == value)
					return;
				_SelectedItem = value;

				RaisePropertyChanged(nameof(SelectedItem));
			}
		}
		#endregion

		public ICollection<MaterialViewModel> Values { get; } = MaterialItemsSource.Instance.Values;

		public MaterialItemsViewModel()
		{
		}
	}

	/// <summary>
	/// 資材コンボの中身
	/// </summary>
	/// <seealso cref="Livet.ViewModel" />
	public class MaterialViewModel : Livet.ViewModel
	{
		public string Key { get; }

		public string Display { get; }

		#region Value 変更通知プロパティ

		private int _Value;

		public int Value
		{
			get { return this._Value; }
			set
			{
				if (this._Value != value)
				{
					this._Value = value;
					this.RaisePropertyChanged();
				}
			}
		}

		#endregion

		public MaterialViewModel(string key, string display)
		{
			this.Key = key;
			this.Display = display;
		}
	}
}
