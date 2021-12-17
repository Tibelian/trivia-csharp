﻿
// basic namespaces imported by default
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

// included models to persist data
using TrivialFinal.Models;

namespace TrivialFinal {

    /**
     * CSV Files <--> Model
     */
    class DataAccess {

        /**
         * Obtain all questions from the file
         */
        public static List<Question> GetQuestions() {
            // list with the questions - this will be returned
            List<Question> questions = new List<Question>();
            // all code inside {try-catch} statement to control exceptions
            try {
                
                // load file from embedded resources
                var assembly = Assembly.GetExecutingAssembly();
                Stream stream = assembly.GetManifestResourceStream("TrivialFinal.Data.questions.csv");

                // reads all file content
                string fileContent = "";
                using (var reader = new StreamReader(stream)) {
                    fileContent = reader.ReadToEnd();
				}

                // each line is read as plain text
                string[] csvLines = fileContent.Split('\n');

                // loop array of strings with all questions
                for (int i = 0; i < csvLines.Length; i++) {
                    // current array position saved in a string variable type
                    string line = csvLines[i].ToString();
                    // all data is separated by the | character
                    string[] dataQuestion = line.Split('|');
                    // so we know that first pos is the asked question
                    // the second pos is the category
                    Question newQuestion = new Question {
                        Text = dataQuestion[0],
                        Category = dataQuestion[1]
                    };

                    // the third pos is the position of the correct answer
                    int correctAnswerIndex = 0;
                    try {
                        // we try to parse it
                        // normally if file is formatted correctly
                        // this try-catch wouldn't be necessary
                        correctAnswerIndex = int.Parse(dataQuestion[2]);
                    }
                    catch (FormatException) { }

                    // and the fourth pos are all the possible answers
                    // so in this position we have another array of string
                    // but this time this pos are separated by ; character
                    string[] dataAnswers = dataQuestion[3].Split(';');

                    // this number is the position of the answer in the loop
                    int answerNum = 0;
                    // init the list of possible answers
                    newQuestion.Answers = new List<Answer>();
                    // loop all the answers
                    foreach (string answer in dataAnswers) {
                        // create the answer
                        newQuestion.Answers.Add(new Answer {
                            Text = answer,
                            // to check if answer is correct
                            // we verify if the current position
                            // equals to the INDEX saved previously
                            IsTrue = (correctAnswerIndex == answerNum)
                        });
                        answerNum++;
                    }
                    // answer is appended to the list
                    questions.Add(newQuestion);
                }

            }
            catch (System.IO.FileNotFoundException) {
                Console.WriteLine("Questions file not found.");
            }
            // all questions from the file are returned
            // without any filter or limitation
            return questions;

        }

        /**
         * Obtain all the players
         */
        public static List<Player> GetRanking() {
            // the found players will be saved here
            List<Player> players = new List<Player>();
            try {

                string fileName = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "ranking.csv"
                );

                // each line in the file is one player record
                string[] csvLines = File.ReadAllLines(fileName);

                // loop all the found players
                for (int i = 0; i < csvLines.Length; i++) {

                    // parse current line to string
                    string line = csvLines[i].ToString();
                    // this string is parsed to array of strings
                    string[] dataPlayer = line.Split(',');
                    // and the formatted data is:
                    // pos 0: nickname
                    // pos 1: total points
                    // pos 2: last time played
                    players.Add(new Player {
                        Nickname = dataPlayer[0],
                        Points = int.Parse(dataPlayer[1]),
                        LastPlay = new DateTime(long.Parse(dataPlayer[2]))
                    });

                    Console.WriteLine("************************");
                    Console.WriteLine(csvLines[i]);
                    Console.WriteLine("************************");

                }

            }
            // exception not processed
            catch (FileNotFoundException) { }

            // order by points - from smallest to biggest
            players = players.OrderBy(o => o.Points).ToList();
            // so we fix it by reversing the order
            players.Reverse();

            // list is returned after the filter
            return players;

        }

        /**
         * Save a player in the ranking or
         * updates his status if already exists
         */
        public static void UpdateRanking(Player player) {
            // first we get the current ranking list
            List<Player> players = GetRanking();

            // boolean variable to control if the player already played
            bool found = false;
            // loop all players to prevent duplicated entry
            foreach (Player p in players) {
                // if player is found then just update points and timestamp
                if (p.Nickname.ToLower() == player.Nickname.ToLower()) {
                    found = true;
                    p.Points += player.Points; // increment the points
                    p.LastPlay = player.LastPlay; // updates the timestamp
                    // exit the loop with break;
                    break;
                }
            }

            // if not found just append the new player
            if (!found) players.Add(player);

            // creates the file with all the data
            string fileName = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "ranking.csv"
            );

            // delete previous file to prevent duplicated data
            if (File.Exists(fileName)) File.Delete(fileName);
            
            // create a new file     
            using (System.IO.StreamWriter sw = System.IO.File.CreateText(fileName)) {
                // 1 player per line, data separated by comma ,
                foreach (Player p in players) {
                    string lastPlay = "" + p.LastPlay.Ticks;
                    Console.WriteLine("##############");
                    Console.WriteLine("Last Play: " + lastPlay);
                    Console.WriteLine("##############");
                    sw.WriteLine(p.Nickname + "," + p.Points + "," + lastPlay);
                }
            }

        }

    }
}
