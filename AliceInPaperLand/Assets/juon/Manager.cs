using UnityEngine;
using System.Collections;
using NCMB;

/// <summary>
/// 会員登録機能使用時のテンプレートクラス	
/// </summary>
[AddComponentMenu("Scripts/NCMBUT/Custom Template/Manager")]
public class Manager:MonoBehaviour
{
	/// <summary>
	/// 会員管理のコントローラクラスを代入する変数
	/// Inspector上で、HierarchyにあるNCMBUTUserConnectionのプレハブをアタッチする		
	/// </summary>
	public NCMBUTUserConnection Connection;

#region LogOut Methods
	/// <summary>
	/// ログアウトを行うメソッド
	/// </summary>
	private void logOut()
	{
		Connection.LogOut(new ErrorCallBack(setLogOutError));
	}

	/// <summary>
	/// ログアウトの結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="error">エラーの有無</param>
	private void setLogOutError(NCMBException error)
	{
		if (error == null)
		{
			Application.LoadLevel("LogIn");
		}
	}
#endregion

	// Playerプレハブ
	public GameObject player;
	
	// タイトル
	private GameObject title;
	
	
	// ボタンが押されると対応する変数がtrueになる
	private bool leaderBoardButton;
	private bool logOutButton;
	
	void Start()
	{
		
		
		// Titleゲームオブジェクトを検索し取得する
		title = GameObject.Find("Title");
	}
	
	void OnGUI()
	{
		if (!IsPlaying())
		{
			if (!Connection.GetIsLogIn)
			{
				Application.LoadLevel("LogIn");
				return;
			}
			
			drawButton();
			
			// ランキングボタンが押されたら
			if (leaderBoardButton)
			{
				Application.LoadLevel("LeaderBoard");
			}
			
			// ログアウトボタンが押されたら
			if (logOutButton)
			{
				logOut();
			}
			
			// 画面タップでゲームスタート
			if (Event.current.type == EventType.MouseDown)
			{ 
				GameStart();
			}
		}
	}
	
	void GameStart()
	{
		// ゲームスタート時に、タイトルを非表示にしてプレイヤーを作成する
		title.SetActive(false);
		Instantiate(player, player.transform.position, player.transform.rotation);
	}
	
	public void GameOver()
	{
		PlayerPrefs.Save();
		
		FindObjectOfType<Score>().Save();
		// ゲームオーバー時に、タイトルを表示する
		title.SetActive(true);
	}
	
	public bool IsPlaying()
	{
		// ゲーム中かどうかはタイトルの表示/非表示で判断する
		return title.activeSelf == false;
	}
	
	private void drawButton()
	{    
		// ボタンの設置
		int btnW = 140, btnH = 50;
		GUI.skin.button.fontSize = 18;
		leaderBoardButton = GUI.Button(new Rect(0 * btnW, 0, btnW, btnH), "Leader Board");
		logOutButton = GUI.Button(new Rect(1 * btnW, 0, btnW, btnH), "Log Out");
	}
}