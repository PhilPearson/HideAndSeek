using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideAndSeek
{
	class OutsideWithHidingPlace: Outside, IHidingPlace
	{
		private string hidingPlace;
		public string HidingPlace {
			get { return hidingPlace; }
		}

		public OutsideWithHidingPlace(string name, bool hot, string hidingPlace)
			: base (name,hot) {
			this.hidingPlace = hidingPlace;
		}
	}
}
