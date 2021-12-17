
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
	 * One question has his a text,
	 * a list of possible answers and 
	 * a category to create different questions
	 */
	class Question {

		public string Text { set; get; }

		public List<Answer> Answers { set; get; }

		public string Category { set; get; }

	}
}
