using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KanburaLike.Models.Settings
{
	[SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
	[System.Xml.Serialization.XmlRootAttribute(ElementName = "KanburaLike", Namespace = "", IsNullable = false)]
	[System.Xml.Serialization.XmlInclude(typeof(WindowSetting))]
	public class SettingsRoot
	{
		[System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
		public string[] Keys { get; set; }

		[System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
		public SerializableSetting[] Values { get; set; }
	}
}
