using KanburaLike.Models;
using Livet;
using Livet.EventListeners;
using System.Collections.Generic;
using System.Linq;

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

		private KanColleModel Kancolle = new KanColleModel();
		private readonly PropertyChangedEventListener listener;

		public InformationViewModel()
		{
			listener = new PropertyChangedEventListener(this.Kancolle);

			listener.RegisterHandler(() => Kancolle.IsRegistered, (s, e) =>
			{
				listener.Add(nameof(Fleets), (_,__) => UpdateFleets());
			});

			this.CompositeDisposable.Add(listener);
		}

		public void Initialize()
		{
			
		}

		private void UpdateFleets()
		{
			Fleets = this.Kancolle.Fleets.Select(f => new FleetViewModel(f)).ToArray();
			//Kancolle.DumpDebugData(Fleets, nameof(Fleets));
		}
	}
}
