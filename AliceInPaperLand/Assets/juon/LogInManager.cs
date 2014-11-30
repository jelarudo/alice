using UnityEngine;
using System.Collections;
using NCMB;

/// <summary>
/// 会員登録機能使用時のテンプレートクラス	
/// </summary>
[AddComponentMenu("Scripts/NCMBUT/Custom Template/LogInManager")]
public class LogInManager:VRGUI
{
		/// <summary>
		/// 会員管理のコントローラクラスを代入する変数
		/// Inspector上で、HierarchyにあるNCMBUTUserConnectionのプレハブをアタッチする		
		/// </summary>
		public NCMBUTUserConnection Connection;
		public GUIStyle guiStyle;
		public GUIStyle guiStyle2;

#region SignUp Methods
		/// <summary>
		/// 新規登録を行うメソッド
		/// ユーザIDの重複不可
		/// </summary>
		/// <param name="userId">ユーザID</param>
		/// <param name="password">パスワード</param>
		private void signUp (string userId, string password)
		{
				Connection.SignUp (userId, password, new ErrorCallBack (setSignUpError));
		}
	
		/// <summary>
		/// 新規登録の結果を受け取るメソッド
		/// エラーがnullだと成功
		/// </summary>
		/// <param name="error">エラーの有無</param>
		private void setSignUpError (NCMBException error)
		{
				if (error == null) {
						Application.LoadLevel ("firstScene");
				}
		}
#endregion

#region LogIn Methods
		/// <summary>
		/// ログインを行うメソッド
		/// </summary>
		/// <param name="userId">ユーザID</param>
		/// <param name="password">パスワード</param>
		private void logIn (string userId, string password)
		{
				Connection.LogIn (userId, password, new ErrorCallBack (setLogInError));
		}

		/// <summary>
		/// ログインの結果を受け取るメソッド
		/// エラーがnullだと成功
		/// </summary>
		/// <param name="error">エラーの有無</param>
		private void setLogInError (NCMBException error)
		{
				if (error == null) {
						Application.LoadLevel ("firstScene");
				}
		}
#endregion

		private GameObject guiTextLogIn;   // ログインテキスト

		// ボタンが押されると対応する変数がtrueになる
		private bool logInButton;
		private bool signUpMenuButton;
		private bool signUpButton;
		private bool backButton;
	
		// テキストボックスで入力される文字列を格納
		public string id;
		public string pw;
	
		void Awake ()
		{
				if (Connection.GetIsLogIn) {
						Application.LoadLevel ("firstScene");
				}
		
				// ゲームオブジェクトを検索し取得する
				guiTextLogIn = GameObject.Find ("GUITextLogIn");
				guiTextLogIn.SetActive (true);
		}
		/*
	void OnGUI()
	{
		drawLogInMenu();
			
		// ログインボタンが押されたら
		if (logInButton)
		{
			logIn(id, pw);
		}
			
		// 新規登録画面に移動するボタンが押されたら
		if (signUpMenuButton)
		{
			signUp(id, pw);
		}
	}
	*/
	
		public override void OnVRGUI ()
		{
				//GUI.color = NowColor;
				//GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture);
				drawTitle ();
				drawLogInMenu ();
		
				// ログインボタンが押されたら
				if (logInButton) {
						logIn (id, pw);
				}
		
				// 新規登録画面に移動するボタンが押されたら
				if (signUpMenuButton) {
						signUp (id, pw);
				}
		}
	
	
		private void drawTitle ()
		{
				// テキストボックスの設置と入力値の取得
				GUI.skin.textField.fontSize = 30;
				int txtW = 350, txtH = 60;
				GUI.Label (new Rect (Screen.width * 1 / 2 - txtW / 2, Screen.height * 1 / 6 - txtH * 1 / 2, txtW, txtH), " Alice In Paperland! ", guiStyle2);
		
		}
		private void drawLogInMenu ()
		{
				// テキストボックスの設置と入力値の取得
				GUI.skin.textField.fontSize = 20;
				int txtMv = 25;
				int txtW = 150, txtH = 40;
				GUI.Label (new Rect (Screen.width * 1 / 4, Screen.height * 1 / 3 - txtH * 1 / 2 + txtMv, txtW, txtH), " Name: ", guiStyle);
				GUI.Label (new Rect (Screen.width * 1 / 4, Screen.height * 1 / 2 - txtH * 1 / 2 + txtMv, txtW + 100, txtH), " Password: ", guiStyle);
				id = GUI.TextField (new Rect (Screen.width * 1 / 2, Screen.height * 1 / 3 - txtH * 1 / 2 + txtMv, txtW, txtH), id);
				pw = GUI.PasswordField (new Rect (Screen.width * 1 / 2, Screen.height * 1 / 2 - txtH * 1 / 2 + txtMv, txtW, txtH), pw, '*');
		
				// ボタンの設置
				int btnW = 180, btnH = 50;
				GUI.skin.button.fontSize = 20;
				logInButton = GUI.Button (new Rect (Screen.width * 1 / 4 - btnW * 1 / 2, Screen.height * 3 / 4 - btnH * 1 / 2, btnW, btnH), "Log In");
				signUpMenuButton = GUI.Button (new Rect (Screen.width * 3 / 4 - btnW * 1 / 2, Screen.height * 3 / 4 - btnH * 1 / 2, btnW, btnH), "Sign Up");
		}
}

