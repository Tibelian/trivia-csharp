using System;
using System.Collections.Generic;
using System.Text;

namespace TrivialFinal.Models {

	/**
	 * The player with 3 editable and readable
	 * attributes: nickname, points and lastplay
	 */
	class Player {

		public string Nickname { set; get; }

		public int Points { set; get; }

		public DateTime LastPlay { set; get; }

	}
}
