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
		public void Test1()
		{
			var target = new PrivateType(typeof(SettingsHost));
			var fullpath = (string)target.GetStaticField("fullpath");

			if (File.Exists(fullpath) == true)
				File.Delete(fullpath);

			SettingsHost.Init();
			var vm = new InformationWindowViewModel();
			vm.Setting.Topmost = true;	//ここでプロパティを書き換え
			Console.WriteLine(vm.Setting.Topmost);

			SettingsHost.SaveFile();
			
			var wsetting = SettingsHost.Instance<InformationWindowSetting>();
			Console.WriteLine(wsetting.Topmost);
		}
	}
}