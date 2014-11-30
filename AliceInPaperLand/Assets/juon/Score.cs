using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
		//CONSTANT SETTING
		public static int PAGEPASSINGSCOREPOINT = 100;
		public static int CRYSTALSCOREPOINT = 10;
	
		public static int score; 
		public static int highScore;
		GameObject crystal;
		// Use this for initialization
		void Start ()
		{
				crystal = GameObject.Find ("crystal");
				//crystal.SetActive (false);
				highScore = PlayerPrefs.GetInt ("highScore", 0);
				PlayerPrefs.SetInt ("score", 0); //At start time, this session score = 0 
		}
	
		void OnDestroy ()
		{
				//PlayerPrefs.SetInt ("highScore", highScore);
				//PlayerPrefs.SetInt ("score", score);
		}
	
		// Update is called once per frame
		void Update ()
		{
				if (score > highScore) {
						highScore = score;
				}
				guiText.text = "Score :" + score + "\n HighScore" +
						highScore;
				if (Score.score >= 10) {
						//crystal.SetActive (true);
				}
		}
}
