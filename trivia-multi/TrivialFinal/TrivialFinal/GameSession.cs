using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrivialFinal.Models;

namespace TrivialFinal {

	/**
	 * Game options
	 */
	static class GameSession {

		// All questions to show
		public static List<Question> QUESTIONS;

		// The loggedin user
		public static Player PLAYER;

		// Number of correct answers
		public static int SUCCESS_COUNTER = 0;

		// Number of the current question
		public static int INDEX = 0;

		// Library used to order randomly the questions
		private static Random rng = new Random();

		/**
		 * Updates questions positions to order randomly
		 */
		public static void RandomizeQuestions() {
			// OrderBy generates the new list in memory so 
			// we have to assign it to the static variable again
			QUESTIONS = QUESTIONS.OrderBy(

				// "random object" generates new pos
				q => rng.Next()

			).ToList();
		}

		/**
		 * Change variables to default
		 * Used before any game 
		 * to prevent duplicated data
		 */
		public static void Clear() {
			QUESTIONS = null;
			//PLAYER = null; // optional
			INDEX = 0;
			SUCCESS_COUNTER = 0;
		}

		/**
		 * Filter the list to the requested category
		 */
		public static void FilterCategory(string category, int limit = 5) {

			// here we save the questions that meet the requirements
			List<Question> filteredQuestions = new List<Question>();

			// each question is verified - one by one
			foreach (Question q in QUESTIONS) {

				// check the questions count
				if (filteredQuestions.Count() == limit) {
					break;
				}

				// if question has requested category then saves it
				if (q.Category.ToLower() == category.ToLower() || category == "") {
					filteredQuestions.Add(q);
				}

			}

			// when loop ends "apply" the new filtered questions
			QUESTIONS = filteredQuestions;

		}


	}

}
