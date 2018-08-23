using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Grabacr07.KanColleWrapper;
using Grabacr07.KanColleWrapper.Models;
using StatefulModel;
using KanburaLike.Models;
using StatefulModel.EventListeners;
using System.ComponentModel;

namespace KanburaLike.ViewModels
{
	class InformationViewModel : ViewModel
	{

		#region Fleets変更通知プロパティ
		private IEnumerable<FleetViewModel> _Fleets;

		public IEnumerable<FleetViewModel> Fleets
		{
			get
			{ return _Fleets; }
			set
			{
				if (_Fleets == value)
					return;
				_Fleets = value;
				RaisePropertyChanged(nameof(Fleets));
			}
		}
		#endregion

		public KanColleModel kancolle { get; set; }

		public InformationViewModel()
		{


		}

		PropertyChangedEventListener listener;

		public void Initialize()
		{
			listener = new PropertyChangedEventListener(this.kancolle);

			listener.RegisterHandler(nameof(Fleets), (sender, e) => Fleets = this.kancolle.Fleets.Select(f => new FleetViewModel(f)));

			this.CompositeDisposable.Add(listener);
		}
	}
}
