using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TrivialFinal {
	public partial class App : Application {
		public App() {
			InitializeComponent();

			// set the mainpage as root
			MainPage = new NavigationPage(new MainPage());

			// play music
			var backgroundMusic = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.CreateSimpleAudioPlayer();
			backgroundMusic.Load(
				Assembly.GetExecutingAssembly()
				.GetManifestResourceStream("TrivialFinal.Data.sound_background.mp3")
			);
			backgroundMusic.Loop = true; // infinte
			backgroundMusic.Volume = 0.5; // from 0 to 1
			backgroundMusic.Play();

		}

		protected override void OnStart() {
		}

		protected override void OnSleep() {
		}

		protected override void OnResume() {
		}
	}
}
