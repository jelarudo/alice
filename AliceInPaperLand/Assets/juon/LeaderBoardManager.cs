using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NCMB;

/// <summary>
/// ランキング機能を使用時のテンプレートクラス
/// </summary>
[AddComponentMenu("Scripts/NCMBUT/Custom Template/LeaderBoard")]
public class LeaderBoardManager:VRGUI
{
		/// <summary>
		/// ランキングのコントローラクラスを代入する変数
		/// Inspector上で、HierarchyにあるNCMBUTRankingConnectionのプレハブをアタッチする
		/// </summary>
		public NCMBUTRankingConnection Connection;
		public NCMBUTUserConnection ConnectionUser;
		public GUIStyle guiStyle;

#region Get Ranking Methods
		/// <summary>
		/// ランキング取得を行うメソッド
		/// </summary>
		private void getRankingList ()
		{
				Connection.GetRankingList (new ListCallback (setRankingList));
		}

		/// <summary>
		/// ランキング取得の結果を受け取るメソッド
		/// エラーがnullだと成功
		/// </summary>
		/// <param name="objList">ランキング一覧</param>
		/// <param name="error">エラーの有無</param>
		private void setRankingList (List<NCMBObject> objList, NCMBException error)
		{
				if (error == null) {
						rankersList = objList;
				} else {
						Debug.Log (error.Message);
				}
		}
#endregion

#region Get Current Rank Methods
		/// <summary>
		/// 現在のユーザの順位を取得するメソッド
		/// 会員登録機能使用時のみ使用可能
		/// </summary>
		private void getCurrentRank ()
		{
				Connection.GetCurrentRank (new IntCallback (setCurrentRank));
		}

		/// <summary>
		/// 現在のユーザの順位の結果を受け取るメソッド
		/// エラーがnullだと成功
		/// </summary>
		/// <param name="count">現在のユーザの順位</param>
		/// <param name="error">エラーの有無</param>
		private void setCurrentRank (int count, NCMBException error)
		{
				if (error == null) {
						if (count != 0) {
								currentRank = count.ToString ();
						}
				}
		}
#endregion

#region Get Total User Methods
		/// <summary>
		/// 総ユーザ数を取得するメソッド
		/// </summary>
		private void getTotalUser ()
		{
				Connection.GetTotalPlayers (new IntCallback (setTotalPlayers));
		}

		/// <summary>
		/// 総ユーザ数の結果を受け取るメソッド
		/// エラーがnullだと成功
		/// </summary>
		/// <param name="count">総ユーザ数</param>
		/// <param name="error">エラーの有無</param>
		private void setTotalPlayers (int count, NCMBException error)
		{
				if (error == null) {
						if (count != 0) {
								totalPlayer = count.ToString ();
						}
				}
		}
#endregion

#region Get User Score
		private void getUserScore ()
		{
				Connection.GetUserScore (new IntCallback (setUserScore));
		}

		private void setUserScore (int score, NCMBException error)
		{
				if (error == null) {
						if (score != 0) {
								bestScore = score.ToString ();
						}
				}
		}
#endregion

	#region LogOut Methods
		/// <summary>
		/// ログアウトを行うメソッド
		/// </summary>
		private void logOut ()
		{
				ConnectionUser.LogOut (new ErrorCallBack (setLogOutError));
		}
	
		/// <summary>
		/// ログアウトの結果を受け取るメソッド
		/// エラーがnullだと成功
		/// </summary>
		/// <param name="error">エラーの有無</param>
		private void setLogOutError (NCMBException error)
		{
				if (error == null) {
						Application.LoadLevel ("LogIn");
				}
		}
	#endregion


		public GameObject[] top = new GameObject[5];
		private GameObject yourScore;
		private GameObject yourRank;
		private List<NCMBObject> rankersList = null;

		private string currentRank = "-";
		private string totalPlayer = "-";
		private string bestScore = "-";
	
		void Start ()
		{
            SoundManager.Instance.PlayBGM( 5 );
           
            SoundManager.Instance.PlayVoice(5);
				// テキストを表示するゲームオブジェクトを取得
				for (uint i = 0; i < 5; ++i) {
						top [i] = GameObject.Find ("Top" + i);
				}

				yourScore = GameObject.Find ("YourScore");
				yourRank = GameObject.Find ("YourRank");
		
				//call getRanking Method
				getRankingList ();
				getTotalUser ();
				getCurrentRank ();
				getUserScore ();
		}
		/*
		void OnGUI ()
		{
				drawMenu ();
				if (rankersList != null) {
						// 取得したトップ5ランキングを表示
						for (int i = 0; i < this.rankersList.Count; ++i) {
								string userName = Connection.GetRankingUserName (rankersList [i]);
								top [i].guiText.text = i + 1 + ". " + userName + " " + this.rankersList [i] ["score"].ToString ();
						}
				}
				yourScore.guiText.text = bestScore;
				yourRank.guiText.text = currentRank + " / " + totalPlayer;
		}
	*/
		public override void OnVRGUI ()
		{
				drawTitle ();
				drawMenu ();
				if (rankersList != null) {
						// 取得したトップ5ランキングを表示
						for (int i = 0; i < this.rankersList.Count; ++i) {
								string userName = Connection.GetRankingUserName (rankersList [i]);
								top [i].guiText.text = i + 1 + ". " + userName + " " + this.rankersList [i] ["score"].ToString ();
						}
				}
				yourScore.guiText.text = bestScore;
				yourRank.guiText.text = currentRank + " / " + totalPlayer;
				int txtW = 300, txtH = 100;
				GUI.TextField (new Rect (Screen.width * 1 / 2 - txtW, Screen.height * 1 / 6, txtW, txtH), "Your score: " + bestScore);
				GUI.TextField (new Rect (Screen.width * 1 / 2 - txtW, Screen.height * 1 / 6 + txtH, txtW, txtH), "Current rank" + currentRank + " / " + totalPlayer);
		
		}
	
		private void drawTitle ()
		{
				// テキストボックスの設置と入力値の取得
				GUI.skin.textField.fontSize = 20;
				int txtW = 150, txtH = 60;
				GUI.Label (new Rect (Screen.width * 1 / 2 - txtW / 2, Screen.height * 1 / 6 - txtH * 1 / 2, txtW, txtH), " Ranking list ", guiStyle);
		
		}
	    
		private void drawMenu ()
		{
				// ボタンの設置
				int btnW = 170, btnH = 30;
				GUI.skin.button.fontSize = 20;
				if (GUI.Button (new Rect (Screen.width * 1 / 2 - btnW * 1 / 2, Screen.height * 7 / 8 - btnH * 1 / 2, btnW, btnH), "Back")) {
                    SoundManager.Instance.PlayVoice( 4 );
						Application.LoadLevel ("firstScene");
				}
				if (GUI.Button (new Rect (Screen.width * 1 / 3 - btnW * 1 / 2, Screen.height * 7 / 8 - btnH * 1 / 2, btnW, btnH), "Logout")) {
                        this.logOut ();
                        SoundManager.Instance.PlayVoice( Random.Range( 0, 1 + 1 ) );
						Application.LoadLevel ("login");
				}
		}
}
