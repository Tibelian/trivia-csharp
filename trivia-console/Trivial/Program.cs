
// basic namespaces imported by default
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// include logic layer
using Trivial.Logic;

/**
 * App main package/namespace
 */
namespace Trivial {

	/**
	 * App main class
	 */
	class Program {

		/**
		 * App main method
		 * Compiler calls this method to run
		 */
		static void Main(string[] args) {

			// here we init the app
			new Dispatcher().ShowMenu();

		}
	}
}
