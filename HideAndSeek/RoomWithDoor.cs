using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
	class RoomWithDoor : RoomWithHidingPlace, IHasExteriorDoor
	{
		private string doorDescription;
		public string DoorDescription {
			get { return doorDescription; }
		}

		public RoomWithDoor(string name, string decoration, string doorDescription, string hidingPlace)
			: base(name, decoration, hidingPlace) {
			this.doorDescription = doorDescription;
		}

		private Location doorLocation;
		public Location DoorLocation {
			get { return doorLocation; }
			set { doorLocation = value; }
		}
	}
}
