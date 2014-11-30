using UnityEngine;
using System.Collections;
using NCMB;

public class Score : MonoBehaviour
{

		//CONSTANT SETTING
		public static int PAGEPASSINGSCOREPOINT = 100;
		public static int CRYSTALSCOREPOINT = 10;
	
		public static int score; 
		public static int highScore;
		GameObject crystal;

		/// <summary>
		/// ランキングのコントローラクラスを代入する変数
		/// Inspector上で、HierarchyにあるNCMBUTRankingConnectionのプレハブをアタッチする
		/// </summary>
		public NCMBUTRankingConnection Connection;
	
	#region Send Score Methods
		/// <summary>
		/// スコアの送信を行うメソッド
		/// ForceUpdateがtrueになっていると、強制更新を行う
		/// </summary>
		/// <param name="score">送信したいスコア</param>
		private void sendScore (int score)
		{
				/**************
         * Inspectorで、フィールドの追加を行うことができる
         * ex. Connection.SetValue("userName", userName);
         **************/
				Connection.SendScore (score, new ErrorCallBack (setSendError));
		}
	
		/// <summary>
		/// スコア送信の結果を受け取るメソッド
		/// エラーがnullだと成功
		/// </summary>
		/// <param name="error">エラーの有無</param>
		private void setSendError (NCMBException error)
		{
				// ゲーム開始前の状態に戻す
				Initialize ();
		}
	#endregion
	
	#region Get User Score
		private void getUserScore ()
		{
				Connection.GetUserScore (new IntCallback (setUserScore));
		}
	
		private void setUserScore (int score, NCMBException error)
		{
				highScore = score;
		}
	#endregion
	
		
		// Use this for initialization
		void Start ()
		{
				Initialize ();
				//crystal = GameObject.Find ("crystal");
				//crystal.SetActive (false);
				//highScore = PlayerPrefs.GetInt ("highScore", 0);
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
		
		// ゲーム開始前の状態に戻す
		private void Initialize ()
		{
				// スコアを0に戻す
				score = 0;
		
				// ハイスコアを取得する。
				getUserScore ();
		}
	
		// ポイントの追加
		public void AddPoint (int point)
		{
				score = score + point;
		}
	
		// ハイスコアの保存
		public void Save ()
		{
				Debug.Log ("SAVE SCORE" + highScore);
				// ハイスコアを保存する
				sendScore (highScore);
		}
}
