using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrivialFinal.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrivialFinal.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RankingPage : ContentPage {
		public RankingPage() {

			InitializeComponent();

			// load players from the history file
			List<Player> players = DataAccess.GetRanking();

			// bind players to the listview
			PlayersList.ItemsSource = players;

			// show a label if players not found
			if (players.Count == 0) NotFound.IsVisible = true;

		}
	}
}