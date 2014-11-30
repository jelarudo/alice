using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NCMB;
using NCMBUT;

[AddComponentMenu("Scripts/NCMBUT/Connection/User Connection")]
public sealed class NCMBUTUserConnection:NCMBUTConnectionBase
{
	// ユーザ名の最小/最大文字数
	public int MinUserName = 1;
	public int MaxUserName = 16;
	// ユーザ名のバリデーションフラグ
	public bool IsUserNameValidation = false;

	// パスワードの最小/最大文字数フラグ
	public int MinPassword = 1;
	public int MaxPassword = 16;
	// パスワードのバリデーションフラグ
	public bool IsUsePasswordValidation = false;

	/// <summary>
	/// mBaaS標準のフィールドを返す
	/// 派生クラスでoverrideして、それぞれのフィールドを追加する
	/// </summary>
	/// <returns>フィールドリスト</returns>
	public override string[] GetDefaultFields()
	{

		return Enum.GetNames(typeof(NCMBUTPlayersDefaultSettings.PlayersDefaultFields));
	}

#region User Management
	/// <summary>
	/// 会員登録を行う
	/// </summary>
	/// <param name="userName">ユーザ名</param>
	/// <param name="passwd">パスワード</param>
	/// <param name="callback">コールバック関数</param>
	public void SignUp(string userName, string password, ErrorCallBack callback)
	{
		if (userName == "" || password == "")
		{
			callback(new NCMBException(NCMBUTErrorMessage.EMPTY_ID_PASS));
			return;
		}

		if (IsUserNameValidation)
		{
			if (!checkUserName(userName))
			{
				callback(new NCMBException(NCMBUTErrorMessage.USER_NAME_CONDITIONS));
				return;
			}
		}
		
		if (IsUsePasswordValidation)
		{
			if (!checkPassword(password))
			{
				callback(new NCMBException(NCMBUTErrorMessage.PASSWORD_CONDITIONS));
				return;
			}
		}
		
		NCMBUser user = GetUserObject();
		user.UserName = userName;
		user.Password = password;

		user.SignUpAsync((NCMBException error) =>
		{
			ClearValues();

			if (error != null)
			{
				callback(error);
				return;
			}

			callback(error);
			savePlayer();
		});
	}

	/// <summary>
	/// ログインする
	/// </summary>
	/// <param name="userName">ユーザ名</param>
	/// <param name="password">パスワード</param>
	/// <param name="callback">コールバック関数</param>
	public void LogIn(string userName, string password, ErrorCallBack callback)
	{
		if (GetIsLogIn)
		{
			callback(new NCMBException(NCMBUTErrorMessage.ALREADY_LOGIN));
			return;
		}

		if (userName == "" || password == "")
		{
			callback(new NCMBException(NCMBUTErrorMessage.EMPTY_ID_PASS));
			return;
		}
		
		NCMBUser.LogInAsync(userName, password, (NCMBException error) =>
		{
			callback(error);
			return;
		});
	}

	/// <summary>
	/// ログアウトする
	/// </summary>
	/// <param name="callback">コールバック関数</param>
	public void LogOut(ErrorCallBack callback)
	{
		if (!GetIsLogIn)
		{
			callback(new NCMBException(NCMBUTErrorMessage.NOT_LOGIN_ERROR));
			return;
		}
		
		NCMBUser.LogOutAsync((NCMBException error) =>
		{
			callback(error);
			return;
		});
	}

	/// <summary>
	/// Gets the current user.
	/// </summary>
	/// <returns>The current user.</returns>
	public NCMBUser GetCurrentUser()
	{
		return NCMBUser.CurrentUser;
	}
#endregion

#region Validation
	/// <summary>
	/// ユーザ名が最小/最大文字数以内かチェックする
	/// </summary>
	/// <returns>true or false</returns>
	/// <param name="userName">ユーザ名</param>
	private bool checkUserName(string userName)
	{
		if (userName.Length < MinUserName || userName.Length > MaxUserName)
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// パスワードが最小/最大文字数以内かチェックする
	/// </summary>
	/// <returns>true or false</returns>
	/// <param name="password">パスワード</param>
	private bool checkPassword(string password)
	{
		if (password.Length < MinPassword || password.Length > MaxPassword)
		{
			return false;
		}

		return true;
	}
#endregion

#region Save Player
	/// <summary>
	/// プレイヤークラスに追加する
	/// </summary>
	private void savePlayer()
	{
		NCMBQuery<NCMBObject> query = GetPlaneQuery(NCMBUTPlayersDefaultSettings.PLAYERS_CLASS);
		query.WhereEqualTo(NCMBUTPlayersDefaultSettings.PlayersDefaultFields.player.ToString(), NCMBUser.CurrentUser);
		query.FindAsync((List<NCMBObject> objList, NCMBException error) =>
		{
			if (error == null && objList.Count == 0)
			{
				NCMBObject player = GetPlaneClassObject(NCMBUTPlayersDefaultSettings.PLAYERS_CLASS);
				player.Add(NCMBUTPlayersDefaultSettings.PlayersDefaultFields.player.ToString(), NCMBUser.CurrentUser);
				player.Add(NCMBUTPlayersDefaultSettings.PlayersDefaultFields.userName.ToString(), NCMBUser.CurrentUser.UserName);
				player.SaveAsync();
			}

			return;
		});
	}
#endregion
}