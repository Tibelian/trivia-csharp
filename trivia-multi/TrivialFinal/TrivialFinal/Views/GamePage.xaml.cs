using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrivialFinal.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GamePage : ContentPage {

		// audio files inside project
		private const string SOUND_OK = "TrivialFinal.Data.sound_success.mp3";
		private const string SOUND_KO = "TrivialFinal.Data.sound_fail.mp3";
		private const string SOUND_FINISH = "TrivialFinal.Data.sound_game_over.mp3";

		/**
		 * init game
		 */
		public GamePage(string category) {

			// default page initializer
			InitializeComponent();

			// reset index to start from 0 always
			GameSession.Clear();

			// load all questions
			GameSession.QUESTIONS = DataAccess.GetQuestions();

			// random questions order
			GameSession.RandomizeQuestions();

			// save only questions with this category
			GameSession.FilterCategory(category);

			// bind data
			this.UpdateQuestion();
		}

		/**
		 * modify the labels and lists from the XAML views
		 * with the question after the Game Index refreshed
		 */
		private void UpdateQuestion() {
			AnswersList.ItemsSource = GameSession.QUESTIONS[GameSession.INDEX].Answers;
			Title = "Question " + (GameSession.INDEX + 1) + "/" + GameSession.QUESTIONS.Count;
			QuestionLabel.Text = GameSession.QUESTIONS[GameSession.INDEX].Text;
		}

		/**
		 * manage event on click answers option
		 */
		async void Handle_ItemTapped(object sender, ItemTappedEventArgs e) {
			if (e.Item == null)
				return;

			// check if selected option is correct to increment
			// the success counter and play the sound effect
			bool isTrueCurrentAnswer = GameSession.QUESTIONS[GameSession.INDEX]
				.Answers[e.ItemIndex]
				.IsTrue;
			if (isTrueCurrentAnswer) {
				GameSession.SUCCESS_COUNTER++;
				PlaySound(SOUND_OK);
			} else {
				PlaySound(SOUND_KO);
			}

			// wait 1 second
			await Task.Delay(1000);

			// deselect Item
			((ListView)sender).SelectedItem = null;

			// finish game if this was the last one
			// or show the next question
			if ((GameSession.INDEX + 1) == GameSession.QUESTIONS.Count) {

				// play finish sound effect
				PlaySound(SOUND_FINISH);

				// set player's data to save him into the ranking
				GameSession.PLAYER.LastPlay = DateTime.Now;
				GameSession.PLAYER.Points = GameSession.SUCCESS_COUNTER;

				// update ranking
				DataAccess.UpdateRanking(GameSession.PLAYER);

				// show a message
				await DisplayAlert("Game Over", "Thank you for playing!", "GO TO MAIN MENU");

				// open main menu
				await Navigation.PopToRootAsync();

			} else {

				// increment question index
				GameSession.INDEX++;

				// load next question
				this.UpdateQuestion();

			}

		}

		/**
		 * play an audio from project resources
		 */
		private void PlaySound(string resource) {

			// search file in project resources
			var assembly = Assembly.GetExecutingAssembly();
			Stream audioStream = assembly.GetManifestResourceStream(resource);

			// use an addon to play the sound
			var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
			player.Load(audioStream);
			player.Play();

		}

	}
}