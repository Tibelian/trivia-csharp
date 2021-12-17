using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrivialFinal.Views;
using Xamarin.Forms;

namespace TrivialFinal {
	public partial class MainPage : ContentPage {
		public MainPage() {

			InitializeComponent();

			// check if player's nickname was already inserted
			if (GameSession.PLAYER != null)
				NicknameEntry.Text = GameSession.PLAYER.Nickname;
			
		}

		/**
		 * start new game view
		 */
		public async void OnStartNewGameClicked(object sender, EventArgs e) {
			if (GameSession.PLAYER == null) {
				await DisplayAlert("Warning", "Do not forget to insert a nickname", "OK");
				NicknameEntry.Focus();
			} else {
				Navigation.PushAsync(new StartNewGamePage());
			}
		}

		/**
		 * show top 10 players
		 */
		public void OnShowRankingClicked(object sender, EventArgs e) {
			Navigation.PushAsync(new RankingPage());
		}

		/**
		 * close the app
		 */
		public void OnExitClicked(object sender, EventArgs e) {
			Environment.Exit(1);
		}

		/**
		 * save player's name
		 */
		public void OnNicknameCompleted(object sender, EventArgs e) {
			if (((Entry)sender).Text != "") {
				GameSession.PLAYER = new Models.Player {
					Nickname = ((Entry)sender).Text
				};
			} else {
				GameSession.PLAYER = null;
				DisplayAlert("Warning", "Insert a valid nickname", "OK");
			}
		}

	}
}
