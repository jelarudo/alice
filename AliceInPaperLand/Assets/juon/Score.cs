using UnityEngine;
using System.Collections;

public class Score : MonoBehaviour
{
		public static int score; 
		static int highScore;
		GameObject crystal;
		// Use this for initialization
		void Start ()
		{
				crystal = GameObject.Find ("crystal");
				//crystal.SetActive (false);
				highScore = PlayerPrefs.GetInt ("highScore", 0);
		}
	
		void OnDestroy ()
		{
				PlayerPrefs.SetInt ("highScore", highScore);
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
