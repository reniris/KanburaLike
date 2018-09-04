using Microsoft.VisualStudio.TestTools.UnitTesting;
using KanburaLike.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KanburaLike.Models.Settings;

namespace KanburaLike.ViewModels.Tests
{
	[TestClass()]
	public class InformationWindowViewModelTests
	{
		[TestMethod()]
		public void InformationWindowViewModelTest()
		{
			string key = "Info";
			var vm = new InformationWindowViewModel(key);
			vm.Setting.TopMost = true;
			vm.Setting.Placement = new MetroRadiance.Interop.Win32.WINDOWPLACEMENT();
			SettingsHost.SaveFile();
		}
	}
}