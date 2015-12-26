using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
	class RoomWithHidingPlace : Room, IHidingPlace
	{
		private string hidingPlace;
		public string HidingPlace {
			get { return hidingPlace; }
		}

		public RoomWithHidingPlace(string name, string decoration, string hidingPlace)
			: base(name, decoration) {
			this.hidingPlace = hidingPlace;
		}

		public override string Description {
			get {
				return base.Description + " Someone could hide " + hidingPlace + ".";
			}
		}
	}
}
