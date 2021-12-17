
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
	 * An answer has the option text 
	 * and the control variable if it is correct
	 */
	class Answer {

		public string Text { set; get; }

		public bool IsTrue { set; get; }

	}
}
