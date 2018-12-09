using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace KanburaLike.Models.Settings
{
	[SerializableAttribute()]
	[XmlTypeAttribute(AnonymousType = true)]
	[XmlRoot(ElementName = "KanburaLike", Namespace = "", IsNullable = false)]
	[XmlInclude(typeof(WindowSetting))]
	[XmlInclude(typeof(ShipsSetting))]
	[XmlInclude(typeof(QuestsSetting))]
	[XmlInclude(typeof(HomeportSetting))]
	public class SettingsRoot
	{
		[XmlArrayItem(IsNullable = false)]
		public string[] Keys { get; set; }

		[XmlArrayItem(IsNullable = false)]
		public SerializableSetting[] Values { get; set; }
	}
}
