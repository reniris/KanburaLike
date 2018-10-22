using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KanburaLike.ViewModels
{
	public static class Extensions
	{
		public static bool IsInDesignMode(this Livet.ViewModel vm) => (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue;
	}
}
