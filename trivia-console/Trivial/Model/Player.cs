
// basic namespaces imported by default
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * Model Layer 
 */
namespace Trivial.Model {

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
