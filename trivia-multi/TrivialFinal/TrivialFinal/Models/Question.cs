using System;
using System.Collections.Generic;
using System.Text;

namespace TrivialFinal.Models {

	/**
	 * One question has a text,
	 * a list of possible answers and 
	 * a category to create different questions
	 */
	class Question {

		public string Text { set; get; }

		public List<Answer> Answers { set; get; }

		public string Category { set; get; }

	}
}
