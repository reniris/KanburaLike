using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.Models
{
	public static class DebugModel
	{
		static DebugModel()
		{
			Init();
		}

		[Conditional("DEBUG")]
		private static void Init()
		{
			//DefaultTraceListenerオブジェクトを取得
			DefaultTraceListener drl = (DefaultTraceListener)Trace.Listeners["Default"];
			//LogFileNameを変更する
			string dir = SettingPath.GetDllFolder();
			drl.LogFileName = Path.Combine(dir, "debug.txt");

			//デバッグログを見やすくするために空行を入れる
			Debug.WriteLine("");
		}

		[Conditional("DEBUG")]
		public static void WriteLine(string message)
		{
			var now = DateTime.Now;
			Debug.WriteLine($"{now}\t{message}");
		}

		[Conditional("DEBUG")]
		public static void WriteLine(Exception e)
		{
			WriteLine($"{e.GetType().ToString()} {e.TargetSite?.ToString()} {e.Message}");
			var inner = e.InnerException;
			if (inner != null)
				WriteLine($"{inner.GetType().ToString()} {inner.TargetSite?.ToString()} {inner.Message}");
		}
	}
}
