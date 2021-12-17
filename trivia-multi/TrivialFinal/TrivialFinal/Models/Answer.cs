using System;
using System.Collections.Generic;
using System.Text;

namespace TrivialFinal.Models {

	/**
	 * An answer has the option text 
	 * and the control variable if it is correct
	 */
	class Answer {

		public string Text { set; get; }

		public bool IsTrue { set; get; }

		public override string ToString() {
			return Text;
		}

	}
}
