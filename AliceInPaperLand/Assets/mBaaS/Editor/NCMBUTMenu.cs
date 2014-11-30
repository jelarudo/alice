using UnityEngine;
using UnityEditor;
using System.Collections;
using NCMBUT.EditorTools;

/// <summary>
/// NCMBUTのメニュー
/// </summary>
public class NCMBUTMenu
{
#region Application Setting
	/// <summary>
	/// APIキーの設定をInspectorに表示する
	/// </summary>
	[MenuItem("NCMBUT/API Key Settings", false, 0)]
	public static void SetAPIKeys()
	{
		NCMBUTKeySettings.Edit();
	}
#endregion

#region Template Settings
	/// <summary>
	/// 会員管理のテンプレート作成
	/// </summary>
	[MenuItem("NCMBUT/Template/Set User", false, 100)]
	public static void SetUserTemplate()
	{
		NCMBUTCustomUserEditor.SetTemplate();
	}

	/// <summary>
	/// 会員管理のテンプレートのアクティブ条件
	/// </summary>
	/// <returns><c>true</c>の時にアクティブ、<c>false</c>の時は非アクティブ</returns>
	[MenuItem("NCMBUT/Template/Set User", true)]
	public static bool IsSetUserTemplate()
	{
		return NCMBUTCustomUserEditor.IsSetTemplate();
	}

	/// <summary>
	/// ランキングのテンプレートの作成
	/// </summary>
	[MenuItem("NCMBUT/Template/Set Ranking", false, 101)]
	public static void SetRankingTemplate()
	{
		NCMBUTCustomRankingEditor.SetTemplate();
	}

	/// <summary>
	/// ランキングのテンプレートのアクティブ条件
	/// </summary>
	/// <returns><c>true</c>の時にアクティブ、<c>false</c>の時は非アクティブ</returns>
	[MenuItem("NCMBUT/Template/Set Ranking", true)]
	public static bool IsSetRankingTemplate()
	{
		return NCMBUTCustomRankingEditor.IsSetTemplate();
	}

	/// <summary>
	/// フレンドのテンプレートの作成
	/// </summary>
	[MenuItem("NCMBUT/Template/Set Friend", false, 102)]
	public static void SetFriendTemplate()
	{
		NCMBUTCustomFriendEditor.SetTemplate();
	}

	/// <summary>
	/// フレンドのテンプレートのアクティブ条件
	/// </summary>
	/// <returns><c>true</c>の時にアクティブ、<c>false</c>の時は非アクティブ</returns>
	[MenuItem("NCMBUT/Template/Set Friend", true)]
	public static bool IsSetFriendTemplate()
	{
		return NCMBUTCustomFriendEditor.IsSetTemplate();
	}

	/// <summary>
	/// 会員管理のカスタムテンプレートの作成
	/// </summary>
	[MenuItem("NCMBUT/Template/Create/Custom User", false, 200)]
	public static void CreateCustomUser()
	{
		TemplateBuilder.CreateUser();
	}

	/// <summary>
	/// ランキングのカスタムテンプレートの作成
	/// </summary>
	[MenuItem("NCMBUT/Template/Create/Custom Ranking", false, 201)]
	public static void CreateCustomRanking()
	{
		TemplateBuilder.CreateRanking();
	}

	/// <summary>
	/// フレンドのカスタムテンプレートの作成
	/// </summary>
	[MenuItem("NCMBUT/Template/Create/Custom Friend", false, 202)]
	public static void CreateCustomFriend()
	{
		TemplateBuilder.CreateFriend();
	}
#endregion

#region Documents
	/// <summary>
	/// mBaaSのログイン画面(ダッシュボード)をブラウザで開く
	/// </summary>
	[MenuItem("NCMBUT/Documents/LogIn", false, 300)]
	public static void LogInAccount()
	{
		string url = "https://sso.nifty.com/pub/login.cgi?service=ncmb&back=https%3a%2f%2fconsole.mb.cloud.nifty.com%2f&am=1.2.0";
		Application.OpenURL(url);
	}

	/// <summary>
	/// mBaaSのドキュメントを開く
	/// </summary>
	[MenuItem("NCMBUT/Documents/SDK Document", false, 301)]
	public static void OpenDocument()
	{
		string url = "http://mb.cloud.nifty.com/doc/index.html#/Unity";
		Application.OpenURL(url);
	}

	/// <summary>
	/// mBaaSのSDKリファレンスを開く
	/// </summary>
	[MenuItem("NCMBUT/Documents/SDK Reference", false, 302)]
	public static void OpenReference()
	{
		string url = "http://mb.cloud.nifty.com/assets/sdk_doc/unity/Help/index.html";
		Application.OpenURL(url);
	}
#endregion
}
