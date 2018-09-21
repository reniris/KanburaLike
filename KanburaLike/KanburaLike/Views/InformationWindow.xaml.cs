
using MetroRadiance.UI.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KanburaLike.Views
{
	/// <summary>
	/// InformationWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class InformationWindow : MetroWindow
	{
		public InformationWindow()
		{
			InitializeComponent();

		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (this.WindowState == WindowState.Minimized)
				this.WindowState = WindowState.Normal;

			base.OnClosing(e);
		}
	}
}
