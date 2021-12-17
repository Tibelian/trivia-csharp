
// basic namespaces imported by default
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// included models to persist data
using Trivial.Model;

/**
 * Logic Layer
 */
namespace Trivial.Logic {

	/**
	 * Dispatcher is used to
	 * process the requests
	 */
	class Dispatcher {

		// constant variable - saves app name or logo
		private readonly string logo = "TRIVIAL - CARMELO & TIBERIU";

		/**
		 * Main menu
		 */
		public void ShowMenu() {
			// control the loop
			bool repeat = true;
			// {do-while} statement to execute at least 1 time the menu
			do {
				// clears and shows the options
				Console.Clear();
				Console.WriteLine("");
				Console.WriteLine(this.Render(this.logo));
				Console.WriteLine("");
				Console.WriteLine(this.Render("MAIN MENU"));
				Console.WriteLine("\ts. Start new game");
				Console.WriteLine("\tr. Show ranking");
				Console.WriteLine("\tq. Exit");
				Console.WriteLine("");
				Console.Write("\tEnter your option: ");
				// process the option selected by changing the inserted key to lowercase
				switch (Console.ReadLine().ToLower()) {
					case "s":
						this.LoadNewGame(); // show pre-game menu
					break;
					case "r":
						this.LoadRanking(); // show top 10 players
					break;
					case "q":
						repeat = false;
						Environment.Exit(1); // force exit
					break;
				}
			} while (repeat);
		}

		/**
		 * Ranking
		 */
		public void LoadRanking() {
			// clear the history of the console
			Console.Clear();
			Console.WriteLine("");
			Console.WriteLine(this.Render(this.logo));
			Console.WriteLine("");
			Console.WriteLine(this.Render("TOP 10 PLAYERS"));
			// load players form the csv file
			// through the data access static object
			List<Player> players = DataAccess.GetRanking();
			// check players quantity
			if (players.Count() > 0) {
				// start ranking from top 1
				int top = 1;
				// loop all the players
				foreach (Player player in players) {
					// singular and plural forms
					string tPoints = player.Points + " point";
					if (player.Points != 1) tPoints += "s";
					// show the current top
					Console.WriteLine("\t" + top + ". " + player.Nickname + " -- " + tPoints + " -- " + player.LastPlay);
					top++;
				}
			} else {
				// if no player is found the just show a simple message
				Console.WriteLine("\t-- Nothing here --");
			}
			// after the ranking show a message to
			// press any key to finish this method
			Console.WriteLine("");
			Console.Write("\tPress any key to go back...");
			Console.ReadLine();
		}

		/**
		 * New Game Category
		 */
		public void LoadNewGame() {
			// new game menu - loop controlled by this variable
			bool repeat = true;
			do {
				// show all the options
				Console.Clear();
				Console.WriteLine("");
				Console.WriteLine(this.Render(this.logo));
				Console.WriteLine("");
				Console.WriteLine(this.Render("START NEW GAME"));
				Console.WriteLine("\t1. History");
				Console.WriteLine("\t2. Maths");
				Console.WriteLine("\t3. Literature");
				Console.WriteLine("\t4. Sport");
				Console.WriteLine("\t5. Physics");
				Console.WriteLine("\t6. Biology");
				Console.WriteLine("\t7. Random");
				Console.WriteLine("");
				// request a key
				Console.Write("\tPlease select a category or press 'q' to quit: ");
				string keyPressed = Console.ReadLine().ToLower();
				// all possible options
				string[] availableKeys = { "1", "2", "3", "4", "5", "6", "7", "q" };
				// if key is not in the options list then the loop starts again
				int index = Array.IndexOf(availableKeys, keyPressed);
				if (index > -1) {
					// if key is available check if the user want to quit
					if (keyPressed == "q") {
						return;
					}
					// else then just get the category name
					string category = this.GetCategoryName(keyPressed);
					// and run the game
					this.StartGame(category);
					// after the game finishes, stop the loop
					repeat = false;
				}
			} while (repeat);
		}

		/**
		 * Run The Game
		 */
		public void StartGame(string category) {

			// if user doesn`t have inserted a nickname then we will request it
			if (Game.PLAYER == null) {
				this.RequestPlayerNickname();
			}

			// load questions from selected category
			Game.QUESTIONS = DataAccess.GetQuestions();

			// random questions order
			Game.RandomizeQuestions();

			// filter the questions by category and quantity
			Game.FilterCategory(category, 5);

			// show questions 1 by 1
			Game.INDEX = 0;
			foreach (Question question in Game.QUESTIONS) {
				this.LoadQuestion(question);
				Game.INDEX++;
			}

			// check if user has failed or approved the test
			this.ShowIfFailed();

			// save user's timestap and points
			Game.PLAYER.Points = Game.SUCCESS_COUNTER;
			Game.PLAYER.LastPlay = DateTime.Now;

			// updates the ranking
			DataAccess.UpdateRanking(Game.PLAYER);

			// clean the static object to prevent overrides
			// on starting another game
			Game.Clear();

		}

		/**
		 * Process all questions inside game
		 */
		private void LoadQuestion(Question question) {
			// again a {do-while} loop to
			// ask at least one time the answer
			// and repeat the question if the
			// pressed key is not allowed
			bool repeat = true;
			do {
				// every time question is showed
				// clear the console and append logo
				Console.Clear();
				Console.WriteLine("");
				Console.WriteLine(this.Render(this.logo));
				Console.WriteLine("");
				Console.WriteLine(this.Render(question.Text));
				// this counter is used to show the current question's number
				int aNum = 1;
				foreach (Answer answer in question.Answers) {
					// write all possible answers
					Console.WriteLine("\t" + aNum + ". " + answer.Text);
					aNum++;
				}
				// request an answer
				Console.WriteLine("");
				Console.Write("\tEnter your answer: ");
				// waits a key to be pressed
				string answerSelected = Console.ReadLine();

				try {
					// tries to parse the key to an integer
					int option = int.Parse(answerSelected) - 1;
					// check if answer is correct using the binding the option
					// selected with the number of the position in the list/array
					if (Game.QUESTIONS[Game.INDEX].Answers[option].IsTrue) {
						Console.WriteLine("\t -- Correct! --\n");
						// if ok then increment the current GAME points
						Game.SUCCESS_COUNTER++;
					} else {
						Console.WriteLine("\t** Incorrect! **\n");
					}
					// we stop the loop, this question is not going to be asked again
					repeat = false;
					// show a summary and wait for user to press a key to finish
					Console.WriteLine("\tCurrent question: " + (Game.INDEX + 1));
					Console.WriteLine("\tTotal questions: " + Game.QUESTIONS.Count());
					Console.WriteLine("");
					Console.Write("\tPress any key to continue...");
					Console.ReadLine();
				}
				catch (Exception) {}

			} while (repeat);
		}

		/**
		 * Game Over Message 
		 */
		private void ShowIfFailed() {
			
			// 50% or more to approve
			float minToApprove = Game.QUESTIONS.Count() - Game.SUCCESS_COUNTER;
			bool failed = Game.SUCCESS_COUNTER < minToApprove;

			// clear the window and show the logo
			Console.Clear();
			Console.WriteLine("");
			Console.WriteLine(this.Render(this.logo));
			Console.WriteLine("");
			// now show if player failed the test
			if (failed) {
				Console.WriteLine(this.Render("SORRY, YOU'VE FAILED THE TEST"));
			} else {
				Console.WriteLine(this.Render("CONGRATULATIONS, YOU'VE PASSED THE TEST"));
			}
			// final summary
			Console.WriteLine("\tCorrect: " + Game.SUCCESS_COUNTER);
			Console.WriteLine("\tIncorrect: " + (Game.QUESTIONS.Count() - Game.SUCCESS_COUNTER));
			Console.WriteLine("");
			// finish when user press any key
			Console.Write("\tPress any key to go to the main menu...");
			Console.ReadLine();

		}

		/**
		 * Menu Util.
		 * Generates a string inside hashtag symbols
		 */
		private string Render(string text) {
			// container variable to save all the generated string
			string renderd = "\t";
			// string lenght adds 3 pos more (3 left and 3 right)
			int totalLength = text.Length + 6;
			// adds first row #######
			for (int i = 0; i < totalLength; i++) {
				renderd += "#";
			}
			// adds 3 hashtags before the inserted text
			// and other 3 hashtags after the inserted text
			renderd += "\n\t## " + text + " ##\n\t";
			// now the last row
			for (int i = 0; i < totalLength; i++) {
				renderd += "#";
			}
			// return the result, example:
			// #####################
			// ## INSERTED STRING ##
			// #####################
			return renderd;
		}

		/**
		 * Associate numbers to categories
		 */
		private string GetCategoryName(string key) {
			// this binds numbers to categories names, like ids
			switch(key) {
				case "1": return "history";
				case "2": return "maths";
				case "3": return "literature";
				case "4": return "sport";
				case "5": return "physics";
				case "6": return "biology";
			}
			return "";
		}

		/**
		 * Save player's name
		 */
		public void RequestPlayerNickname() {
			// one time player name is requested
			Console.Clear();
			Console.WriteLine("");
			Console.WriteLine(this.Render(this.logo));
			Console.WriteLine("");
			Console.WriteLine(this.Render("INSERT YOUR NICKNAME"));
			Console.WriteLine("");
			Console.Write("\tWrite your name here: ");
			// save the name inside a string variable
			string nickname = Console.ReadLine();
			// and assign that nickname to one player
			// also that player is binded to
			// the current Game or Session if player is not
			// cleared after game over
			Game.PLAYER = new Player {
				Nickname = nickname
			};
		}

	}
}
