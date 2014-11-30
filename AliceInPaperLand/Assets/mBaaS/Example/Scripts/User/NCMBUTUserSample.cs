using UnityEngine;
using System.Collections;
using NCMB;

/// <summary>
/// 会員登録機能使用時のテンプレートクラス	
/// </summary>
public class NCMBUTUserSample:MonoBehaviour
{
	/// <summary>
	/// 会員管理のコントローラクラスを代入する変数
	/// Inspector上で、HierarchyにあるNCMBUTUserConnectionのプレハブをアタッチする		
	/// </summary>
	public NCMBUTUserConnection Connection;
	
#region SignUp Methods
	/// <summary>
	/// 新規登録を行うメソッド
	/// ユーザIDの重複不可
	/// </summary>
	/// <param name="userId">ユーザID</param>
	/// <param name="password">パスワード</param>
	private void signUp(string userId, string password)
	{
		Connection.SignUp(userId, password, new ErrorCallBack(setSignUpError));
	}
	
	/// <summary>
	/// 新規登録の結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="error">エラーの有無</param>
	private void setSignUpError(NCMBException error)
	{
		systemMessage = getSystemMessage(error);
	}
#endregion

#region LogIn Methods
	/// <summary>
	/// ログインを行うメソッド
	/// </summary>
	/// <param name="userId">ユーザID</param>
	/// <param name="password">パスワード</param>
	private void logIn(string userId, string password)
	{
		Connection.LogIn(userId, password, new ErrorCallBack(setLogInError));
	}
	
	/// <summary>
	/// ログインの結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="error">エラーの有無</param>
	private void setLogInError(NCMBException error)
	{
		systemMessage = getSystemMessage(error);
	}
#endregion

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
		systemMessage = getSystemMessage(error);
	}
#endregion

#region GUI Methods
	private string userId = "";
	private string password = "";
	private string systemMessage = "";
	private float margin = 10.0f;

	/// <summary>
	/// 会員登録のGUIを表示
	/// </summary>
	void OnGUI()
	{
		guiSettings();

		float contentsWidth = Screen.width - margin * 2.0f;
		
		Rect titleRect = new Rect(margin, 0, contentsWidth, Screen.height * 0.1f);
		drawTitle(titleRect);

		Rect formRect = new Rect(margin, titleRect.y + titleRect.height, contentsWidth, Screen.height * 0.7f);
		drawForm(formRect);

		Rect errorRect = new Rect(margin, formRect.y + formRect.height, contentsWidth, Screen.height * 0.1f);
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
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("User Sample", GUILayout.MinWidth(115));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
	}

	/// <summary>
	/// 入力フォームの表示
	/// </summary>
	/// <param name="rect">表示範囲</param>
	private void drawForm(Rect rect)
	{
		GUI.Box(rect, "");

		GUILayout.BeginArea(rect);

		// ログイン中のユーザがいるかどうかで、処理を分ける
		if (Connection.GetIsLogIn)
		{
			drawLogOutForm();
		}
		else
		{
			drawLogInForm();
		}
		
		GUILayout.EndArea();
	}

	/// <summary>
	/// ログインフォームの表示
	/// </summary>
	private void drawLogInForm()
	{
		GUILayout.FlexibleSpace();

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("ID", GUILayout.MaxWidth(100));
		userId = GUILayout.TextField(userId, GUILayout.MaxWidth(300));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Password", GUILayout.MaxWidth(100));
		password = GUILayout.PasswordField(password, '*', GUILayout.MaxWidth(300));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Log In", GUILayout.MinHeight(50)))
		{
			logIn(userId, password);
		}

		if (GUILayout.Button("Sign Up", GUILayout.MinHeight(50)))
		{
			signUp(userId, password);
		}
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();
	}

	/// <summary>
	/// ログアウトの表示
	/// </summary>
	private void drawLogOutForm()
	{
		GUILayout.FlexibleSpace();

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Hello " + NCMBUser.CurrentUser.UserName);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.FlexibleSpace();

		if (GUILayout.Button("Log Out", GUILayout.MinHeight(50)))
		{
			logOut();
		}
		GUILayout.FlexibleSpace();
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
	/// ホームボタンの表示
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
