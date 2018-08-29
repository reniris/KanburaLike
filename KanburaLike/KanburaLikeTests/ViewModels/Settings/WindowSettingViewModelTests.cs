using Microsoft.VisualStudio.TestTools.UnitTesting;
using KanburaLike.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KanburaLike.Models.Settings;

namespace KanburaLike.ViewModels.Settings.Tests
{
	[TestClass()]
	public class WindowSettingViewModelTests
	{
		[TestMethod()]
		public void ApplyTest()
		{
			var vm = new WindowSettingViewModel();

			Console.WriteLine(vm.Setting.Topmost);

			var target = new PrivateType(typeof(SettingsHost));
			var fullpath = (string)target.GetStaticField("fullpath");

			File.Exists(fullpath).Is(true);

			SettingsHost.Init();
			var wsetting = SettingsHost.Instance<WindowSetting>();
			Console.WriteLine(wsetting.Topmost);
		}
	}
}