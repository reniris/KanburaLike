using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.Models.Settings
{
	public abstract class SerializableSetting : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			KanColleModel.DebugWriteLine($"Changed {propertyName}");
		}

		public virtual string CategoryName => this.GetType().Name;

		public SerializableSetting()
		{

		}
	}
}
