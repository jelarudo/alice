using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NCMB;

/// <summary>
/// ランキング機能を使用時のテンプレートクラス
/// </summary>
public class NCMBUTRankingSample:MonoBehaviour
{
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
	private void sendScore(int score)
	{
		/**************
		 * Inspectorで、フィールドの追加を行うことができる
		 * ex. Connection.SetValue("userName", userName);
		 **************/
		Connection.SetValue("userName", userName);
		Connection.SendScore(score, new ErrorCallBack(setSendError));
	}
	
	/// <summary>
	/// スコア送信の結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="error">エラーの有無</param>
	private void setSendError(NCMBException error)
	{
		systemMessage = getSystemMessage(error);
	}
#endregion
	
#region Get Ranking Methods
	/// <summary>
	/// ランキング取得を行うメソッド
	/// </summary>
	private void getRankingList()
	{
		Connection.GetRankingList(new ListCallback(setRankingList));
	}
	
	/// <summary>
	/// ランキング取得の結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="results">ランキング一覧</param>
	/// <param name="error">エラーの有無</param>
	private void setRankingList(List<NCMBObject> results, NCMBException error)
	{
		rankingList = results;
		systemMessage = getSystemMessage(error);
	}
#endregion
	
#region Get Player Current Methods
	/// <summary>
	/// 現在のユーザの順位を取得するメソッド
	/// 会員登録機能使用時のみ使用可能
	/// </summary>
	private void getCurrentRank()
	{
		Connection.GetCurrentRank(new IntCallback(setCurrentRank));
	}
	
	/// <summary>
	/// 現在のユーザの順位の結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="count">現在のユーザの順位</param>
	/// <param name="error">エラーの有無</param>
	private void setCurrentRank(int count, NCMBException error)
	{
		currentRank = count;
		systemMessage = getSystemMessage(error);
	}
#endregion

#region Get Total User Methods
	/// <summary>
	/// 総ユーザ数を取得するメソッド
	/// </summary>
	private void getTotalUser()
	{
		Connection.GetTotalPlayers(new IntCallback(setTotalPlayers));
	}
	
	/// <summary>
	/// 総ユーザ数の結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="count">総ユーザ数</param>
	/// <param name="error">エラーの有無</param>
	private void setTotalPlayers(int count, NCMBException error)
	{
		totalUser = count;
		systemMessage = getSystemMessage(error);
	}
#endregion

#region Get User Score
	/// <summary>
	/// データストアから現在のスコアを取得するメソッド
	/// </summary>
	private void getUserScore()
	{
		Connection.GetUserScore(new IntCallback(setUserScore));
	}
	
	/// <summary>
	/// データストアから現在のスコアの結果を受け取るメソッド
	/// </summary>
	/// <param name="score">現在のスコア</param>
	/// <param name="error">エラーの有無</param>
	private void setUserScore(int score, NCMBException error)
	{
		highScore = score;
		systemMessage = getSystemMessage(error);
	}
#endregion

#region GUI Methods
	private string userName = "Name";
	private string score = "";
	private int highScore = 0;
	private List<NCMBObject> rankingList = null;
	private string systemMessage = "";
	private int currentRank = 0;
	private int totalUser = 0;
	private float margin = 10.0f;
	private Vector2 scrollPosition;

	/// <summary>
	/// ランキングのGUIを表示
	/// </summary>
	void OnGUI()
	{
		guiSettings();

		float contentsWidth = Screen.width - margin * 2.0f;

		Rect titleRect = new Rect(margin, 0, contentsWidth, Screen.height * 0.1f);
		drawTitle(titleRect);

		Rect sendRect = new Rect(margin, titleRect.y + titleRect.height, Screen.width - margin * 2.0f, Screen.height * 0.1f);
		drawSendScore(sendRect);

		Rect rankRect = new Rect(margin, sendRect.y + sendRect.height, contentsWidth / 3.0f, Screen.height * 0.1f);
		drawCurrentRank(rankRect);

		Rect totalRect = new Rect(margin + contentsWidth / 3.0f, sendRect.y + sendRect.height, contentsWidth / 3.0f, Screen.height * 0.1f);
		drawTotalUser(totalRect);

		Rect scoreRect = new Rect(margin + contentsWidth - contentsWidth / 3.0f, sendRect.y + sendRect.height, contentsWidth / 3.0f, Screen.height * 0.1f);
		drawHighScore(scoreRect);

		Rect rankingRect = new Rect(margin, rankRect.y + rankRect.height, contentsWidth, Screen.height * 0.5f);
		drawRanking(rankingRect);

		Rect errorRect = new Rect(margin, rankingRect.y + rankingRect.height, contentsWidth, Screen.height * 0.1f);
		drawError(errorRect);

		Rect backRect = new Rect(margin, errorRect.y + errorRect.height, contentsWidth, Screen.height * 0.1f);
		drawBack(backRect);
	}

	private void guiSettings()
	{
		GUI.skin.label.fontSize = 20;
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.skin.textField.fontSize = 20;
	}

	/// <summary>
	/// タイトルの表示
	/// </summary>
	/// <param name="rect">表示範囲</param>
	private void drawTitle(Rect rect)
	{
		GUILayout.BeginArea(rect);
		GUILayout.Label("Ranking Sample"); 
		GUILayout.EndArea();
	}

	/// <summary>
	/// スコア送信の表示
	/// </summary>
	/// <param name="rect">表示範囲</param>
	private void drawSendScore(Rect rect)
	{
		GUI.Box(rect, "");
		GUILayout.BeginArea(rect);
		GUILayout.Label("Send Score");
		GUILayout.BeginHorizontal();

		if (Connection.GetIsLogIn)
		{
			userName = NCMBUser.CurrentUser.UserName;
			GUILayout.Label(userName);
		}
		else
		{
			userName = GUILayout.TextField(userName);
		}

		score = GUILayout.TextField(score);
		if (GUILayout.Button("Send", GUILayout.MinHeight(30)))
		{
			int number;
			bool result = int.TryParse(score, out number);
			if (!result)
			{
				systemMessage = "Score does it must be a number.";
				return;
			}
			sendScore(number);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	/// <summary>
	/// ユーザの現在の順位を表示
	/// </summary>
	/// <param name="rect">表示範囲</param>
	private void drawCurrentRank(Rect rect)
	{
		GUI.Box(rect, "");

		GUILayout.BeginArea(rect);
		GUILayout.Label("Your Current Rank");
		GUILayout.BeginHorizontal();
		GUILayout.Label(currentRank.ToString());
		if (GUILayout.Button("Get Current Rank", GUILayout.MaxWidth(150), GUILayout.MinHeight(30)))
		{
			getCurrentRank();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	/// <summary>
	/// 総プレイ人数を表示
	/// </summary>
	/// <param name="rect">表示範囲</param>
	private void drawTotalUser(Rect rect)
	{
		GUI.Box(rect, "");
		
		GUILayout.BeginArea(rect);
		GUILayout.Label("Total Users");
		GUILayout.BeginHorizontal();
		GUILayout.Label(totalUser.ToString());
		if (GUILayout.Button("Get Total User", GUILayout.MaxWidth(150), GUILayout.MinHeight(30)))
		{
			getTotalUser();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="rect">表示範囲</param>
	private void drawHighScore(Rect rect)
	{
		GUI.Box(rect, "");
		
		GUILayout.BeginArea(rect);
		GUILayout.Label("High Score");
		GUILayout.BeginHorizontal();
		GUILayout.Label(highScore.ToString());
		if (GUILayout.Button("Get High Score", GUILayout.MaxWidth(150), GUILayout.MinHeight(30)))
		{
			getUserScore();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}


	/// <summary>
	/// ランキングの表示
	/// </summary>
	/// <param name="rect">表示範囲</param>
	private void drawRanking(Rect rect)
	{
		GUI.Box(rect, "");

		GUILayout.BeginArea(rect);
		GUILayout.Label("Ranking");
		if (GUILayout.Button("Get Ranking", GUILayout.MinHeight(30)))
		{
			getRankingList();
		}

		// リストがnullでなければ、ランキングを持っている
		if (rankingList != null)
		{
			GUILayout.Space(0.1f);
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			for (int i = 0; i < rankingList.Count; ++i)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label((i + 1).ToString(), GUILayout.MaxWidth(80));					// 順位
				GUILayout.Label(Connection.GetRankingUserName(rankingList[i], "userName"));	// ユーザ名(追加したフィールド)
				GUI.skin.label.alignment = TextAnchor.MiddleRight;
				GUILayout.Label(rankingList[i]["score"].ToString());	// スコア
				GUI.skin.label.alignment = TextAnchor.MiddleLeft;
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
			GUILayout.Space(0.1f);
		}
		GUILayout.EndArea();
	}
	
	/// <summary>
	/// エラーの表示
	/// </summary>
	/// <param name="rect">表示範囲</param>
	private void drawError(Rect rect)
	{
		GUI.Box(rect, "");

		GUILayout.BeginArea(rect);
		GUILayout.Label("System Message");
		GUILayout.Label(systemMessage);
		GUILayout.EndArea();
	}

	/// <summary>
	/// Gets the system message.
	/// </summary>
	/// <returns>The system message.</returns>
	/// <param name="exception">Exception.</param>
	private string getSystemMessage(NCMBException exception)
	{
		return (exception == null) ? "Success!" : exception.Message;
	}

	/// <summary>
	/// ホームボタン表示
	/// </summary>
	/// <param name="rect">表示範囲</param>
	private void drawBack(Rect rect)
	{
		GUILayout.BeginArea(rect);
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Back", GUILayout.MinHeight(50)))
		{
			Application.LoadLevel("NCMBUTHome");
		}
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
	}
#endregion
}
