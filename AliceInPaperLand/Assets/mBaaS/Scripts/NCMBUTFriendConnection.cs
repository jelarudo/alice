using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NCMB;
using NCMBUT;

[AddComponentMenu("Scripts/NCMBUT/Connection/Friend Connection")]
public sealed class NCMBUTFriendConnection:NCMBUTConnectionBase
{
	/// <summary>
	/// ACL設定を行うかどうかの設定
	/// </summary>
	/// <returns><c>true</c>, if use acl settings was gotten, <c>false</c> otherwise.</returns>
	public override bool GetUseAclSettings()
	{
		UseAclSettings = false;
		return UseAclSettings;
	}

	/// <summary>
	/// mBaaS標準のフィールドを返す
	/// </summary>
	/// <returns>フィールドリスト</returns>
	public override string[] GetDefaultFields()
	{
		return Enum.GetNames(typeof(NCMBUTMessagesDefaultSettings.MessagesDefaultFields));
	}

#region Friend List
	/// <summary>
	/// フレンドリストの一覧を取得
	/// ※ ログイン必須
	/// </summary>
	/// <param name="callback">コールバック関数</param>
	public void GetFriendList(ListCallback callback)
	{
		if (!GetIsLogIn)
		{
			callback(null, new NCMBException(NCMBUTErrorMessage.NOT_LOGIN_ERROR));
			return;
		}

		NCMBQuery<NCMBObject> query = GetQuery(NCMBUTFriendsDefaultSettings.FRIEND_CLASS);
		query.WhereEqualTo(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.from.ToString(), NCMBUser.CurrentUser);
		query.Include(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.to.ToString());

		query.FindAsync((List<NCMBObject> objList, NCMBException error) => {
			callback(objList, error);
			return;
		});
	}
#endregion

#region Friend Request
	/// <summary>
	/// フレンド申請を行う
	/// </summary>
	/// <param name="obj">フレンド申請を行う相手</param>
	/// <param name="callback">コールバック関数</param>
	public void SendFriendRequest(NCMBObject obj, ErrorCallBack callback)
	{
		base.CheckMatchClass(obj.ClassName, new string[]{NCMBUTPlayersDefaultSettings.PLAYERS_CLASS});

		if (!GetIsLogIn)
		{
			callback(new NCMBException(NCMBUTErrorMessage.NOT_LOGIN_ERROR));
			return;
		}

		// 自分から相手へのフレンド申請を登録する
		NCMBObject fromObj = GetPlaneClassObject(NCMBUTFriendsDefaultSettings.FRIEND_CLASS);
		fromObj.Add(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.from.ToString(), NCMBUser.CurrentUser);
		fromObj.Add(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.to.ToString(), (NCMBObject)obj["player"]);
		fromObj.Add(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isAccept.ToString(), false);
		fromObj.Add(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isPending.ToString(), true);
		fromObj.SaveAsync((NCMBException fromError) => {
			callback(fromError);

			if (fromError != null)
			{
				return;
			}

			// 相手から自分へのフレンドを登録する
			NCMBObject toObj = GetPlaneClassObject(NCMBUTFriendsDefaultSettings.FRIEND_CLASS);
			toObj.Add(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.from.ToString(), (NCMBObject)obj["player"]);
			toObj.Add(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.to.ToString(), NCMBUser.CurrentUser);
			toObj.Add(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isAccept.ToString(), false);
			toObj.Add(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isPending.ToString(), false);
			toObj.SaveAsync((NCMBException toError) => {
				callback(toError);
				return;
			});	
		});
	}

	/// <summary>
	/// 自分宛てにきているフレンド申請を承認する
	/// </summary>
	/// <param name="obj">承認するフレンド申請</param>
	/// <param name="callback">コールバック関数</param>
	public void AcceptFriendRequest(NCMBObject obj, ErrorCallBack callback)
	{
		base.CheckMatchClass(obj.ClassName, new string[]{NCMBUTFriendsDefaultSettings.FRIEND_CLASS});

		if (!GetIsLogIn)
		{
			callback(new NCMBException(NCMBUTErrorMessage.NOT_LOGIN_ERROR));
			return;
		}

		//from to 
		obj[NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isAccept.ToString()] = true;
		obj.SaveAsync((NCMBException error) => {
			callback(error);

			if (error != null)
			{
				return;
			}

			NCMBQuery<NCMBObject> findQuery = GetPlaneQuery(NCMBUTFriendsDefaultSettings.FRIEND_CLASS);
			findQuery.WhereEqualTo(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.from.ToString(), obj["to"]);
			findQuery.WhereEqualTo(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.to.ToString(), NCMBUser.CurrentUser);
			findQuery.FindAsync((List<NCMBObject> objList, NCMBException findError) => {
				if (findError != null)
				{
					callback(findError);
					return;
				}

				if (objList.Count == 1)
				{
					objList[0][NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isAccept.ToString()] = true;
					objList[0][NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isPending.ToString()] = false;
					objList[0].SaveAsync((NCMBException saveError) => {
						callback(saveError);
						return;
					});
				}
			});
		});
	}

	/// <summary>
	/// 自分宛てにきているフレンド申請を非承認にする
	/// </summary>
	/// <param name="obj">非承認にするフレンド申請</param>
	/// <param name="callback">コールバック関数</param>
	public void DeclineFriendRequest(NCMBObject obj, ErrorCallBack callback)
	{
		base.CheckMatchClass(obj.ClassName, new string[]{NCMBUTFriendsDefaultSettings.FRIEND_CLASS});

		if (!GetIsLogIn)
		{
			callback(new NCMBException(NCMBUTErrorMessage.NOT_LOGIN_ERROR));
			return;
		}

		obj[NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isAccept.ToString()] = false;
		obj[NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isEnable.ToString()] = false;
		obj.SaveAsync((NCMBException error) => {
			callback(error);

			if (error != null)
			{
				return;
			}
			// TODO isEnable?
			NCMBQuery<NCMBObject> findQuery = GetPlaneQuery(NCMBUTFriendsDefaultSettings.FRIEND_CLASS);
			findQuery.WhereEqualTo(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.from.ToString(), (NCMBObject)obj["to"]);
			findQuery.WhereEqualTo(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.to.ToString(), NCMBUser.CurrentUser);
			findQuery.FindAsync((List<NCMBObject> objList, NCMBException findError) => {
				callback(findError);

				if (findError != null)
				{
					return;
				}

				if (objList.Count == 1)
				{
					objList[0][NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isAccept.ToString()] = false;
					objList[0][NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isEnable.ToString()] = false;
					objList[0].SaveAsync((NCMBException saveError) => {
						callback(saveError);
						return;
					});
				}
			});
		});
	}

	/// <summary>
	/// 送信したフレンドリクエストをキャンセルする
	/// </summary>
	/// <param name="obj">キャンセルするフレンド申請</param>
	/// <param name="callback">コールバック</param>
	public void CancelRequest(NCMBObject obj, ErrorCallBack callback)
	{
		base.CheckMatchClass(obj.ClassName, new string[]{NCMBUTFriendsDefaultSettings.FRIEND_CLASS});
		
		if (!GetIsLogIn)
		{
			callback(new NCMBException(NCMBUTErrorMessage.NOT_LOGIN_ERROR));
			return;
		}

		obj[NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isEnable.ToString()] = false;
		obj[NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isPending.ToString()] = false;
		obj.SaveAsync((NCMBException error) => {
			callback(error);

			if (error != null)
			{
				return;
			}

			NCMBQuery<NCMBObject> findQuery = new NCMBQuery<NCMBObject>(NCMBUTFriendsDefaultSettings.FRIEND_CLASS);
			findQuery.WhereEqualTo(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.from.ToString(), (NCMBObject)obj["to"]);
			findQuery.WhereEqualTo(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.to.ToString(), NCMBUser.CurrentUser);
			findQuery.WhereEqualTo(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isEnable.ToString(), true);
			findQuery.FindAsync((List<NCMBObject> objList, NCMBException findError) => {
				callback(findError);

				if (findError != null)
				{
					return;
				}

				if (objList.Count == 1)
				{
					objList[0][NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isEnable.ToString()] = false;
					objList[0][NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isPending.ToString()] = false;
					objList[0].SaveAsync((NCMBException saveError) => {
						callback(saveError);
						return;
					});
				}
			});
		});
	}

	/// <summary>
	/// フレンドを削除する
	/// </summary>
	/// <param name="obj">削除対象のフレンド</param>
	/// <param name="callback">コールバック関数</param>
	public void RemoveFriend(NCMBObject obj, ErrorCallBack callback)
	{
		base.CheckMatchClass(obj.ClassName, new string[]{NCMBUTFriendsDefaultSettings.FRIEND_CLASS});

		if (!GetIsLogIn)
		{
			callback(new NCMBException(NCMBUTErrorMessage.NOT_LOGIN_ERROR));
			return;
		}

		obj[NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isEnable.ToString()] = false;
		obj.SaveAsync((NCMBException error) => {
			callback(error);

			if (error != null)
			{
				return;
			}

			NCMBQuery<NCMBObject> findQuery = GetPlaneQuery(NCMBUTFriendsDefaultSettings.FRIEND_CLASS);
			findQuery.WhereEqualTo(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.from.ToString(), (NCMBObject)obj["to"]);
			findQuery.WhereEqualTo(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.to.ToString(), NCMBUser.CurrentUser);
			findQuery.FindAsync((List<NCMBObject> objList, NCMBException findError) => {
				callback(findError);

				if (findError != null)
				{
					return;
				}

				if (objList.Count == 1)
				{
					objList[0][NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isEnable.ToString()] = false;
					objList[0].SaveAsync((NCMBException saveError) => {
						callback(saveError);
						return;
					});
				}
			});
		});
	}
#endregion

#region Message
	/// <summary>
	/// メッセージを送信する
	/// </summary>
	/// <param name="obj">メッセージの送信相手</param>
	/// <param name="message">メッセージ内容</param>
	/// <param name="callback">コールバック関数</param>
	public void SendMessage(NCMBUser obj, string message, ErrorCallBack callback)
	{
		if (!GetIsLogIn)
		{
			callback(new NCMBException(NCMBUTErrorMessage.NOT_LOGIN_ERROR));
			return;
		}

		if (message == "")
		{
			callback(new NCMBException(NCMBUTErrorMessage.EMPTY_MESSAGE));
			return;
		}

		NCMBObject messageObj = GetClassObject(NCMBUTMessagesDefaultSettings.MESSAGE_CLASS);
		messageObj.Add(NCMBUTMessagesDefaultSettings.MessagesDefaultFields.message.ToString(), message);
		messageObj.Add(NCMBUTMessagesDefaultSettings.MessagesDefaultFields.from.ToString(), NCMBUser.CurrentUser);
		messageObj.Add(NCMBUTMessagesDefaultSettings.MessagesDefaultFields.to.ToString(), obj);
		messageObj.SaveAsync((NCMBException error) => {
			ClearValues();
			callback(error);
			return;
		});
	}

	/// <summary>
	/// 自分宛てにきているメッセージの一覧を取得する
	/// </summary>
	/// <param name="callback">コールバック関数</param>
	public void GetMessageList(ListCallback callback)
	{
		if (!GetIsLogIn)
		{
			callback(null, new NCMBException(NCMBUTErrorMessage.NOT_LOGIN_ERROR));
			return;
		}

		NCMBQuery<NCMBObject> messObj = GetQuery(NCMBUTMessagesDefaultSettings.MESSAGE_CLASS);
		messObj.WhereEqualTo(NCMBUTMessagesDefaultSettings.MessagesDefaultFields.to.ToString(), NCMBUser.CurrentUser);
		messObj.Include(NCMBUTMessagesDefaultSettings.MessagesDefaultFields.from.ToString());
		messObj.FindAsync((List<NCMBObject> objList, NCMBException error) => {
			callback(objList, error);
			return;
		});
	}

	/// <summary>
	/// メッセージを削除する
	/// </summary>
	/// <param name="obj">削除対象のメッセージ</param>
	/// <param name="callback">コールバック関数</param>
	public void RemoveMessage(NCMBObject obj, ErrorCallBack callback)
	{
		base.CheckMatchClass(obj.ClassName, new string[]{NCMBUTMessagesDefaultSettings.MESSAGE_CLASS});

		if (!GetIsLogIn)
		{
			callback(new NCMBException(NCMBUTErrorMessage.NOT_LOGIN_ERROR));
			return;
		}

		obj[NCMBUTFriendsDefaultSettings.FriendsDefaultFields.isEnable.ToString()] = false;
		obj.SaveAsync((NCMBException error) => {
			callback(error);
			return;
		});
	}
#endregion

#region Search
	/// <summary>
	/// ユーザを検索する
	/// </summary>
	/// <param name="userName">検索する名前</param>
	/// <param name="callback">コールバック関数</param>
	public void GetSearchUserList(string userName, ListCallback callback)
	{
		if (!GetIsLogIn)
		{
			callback(null, new NCMBException(NCMBUTErrorMessage.NOT_LOGIN_ERROR));
			return;
		}

		if (userName == "")
		{
			callback(null, new NCMBException(NCMBUTErrorMessage.EMPTY_USER_NAME));
			return;
		}

		//find exist friends
		NCMBQuery<NCMBObject> friendsQuery = GetQuery(NCMBUTFriendsDefaultSettings.FRIEND_CLASS);
		friendsQuery.WhereEqualTo(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.from.ToString(), NCMBUser.CurrentUser);
		friendsQuery.Include(NCMBUTFriendsDefaultSettings.FriendsDefaultFields.to.ToString());
		friendsQuery.FindAsync((List<NCMBObject> objList, NCMBException error) => {
			if (error != null)
			{
				callback(null, error);
				return;
			}

			List<NCMBObject> players = new List<NCMBObject>();
			players.Add(NCMBUser.CurrentUser as NCMBObject);
			foreach (NCMBObject friend in objList)
			{
				players.Add(friend["to"] as NCMBObject);
			}

			NCMBQuery<NCMBObject> query = GetQuery(NCMBUTPlayersDefaultSettings.PLAYERS_CLASS);
			query.WhereEqualTo("userName", userName);
			query.WhereNotContainedIn(NCMBUTPlayersDefaultSettings.PlayersDefaultFields.player.ToString(), players);
			query.Include(NCMBUTPlayersDefaultSettings.PlayersDefaultFields.player.ToString());
			query.FindAsync((List<NCMBObject> playerList, NCMBException findError) => {
				callback(playerList, findError);
				return;
			});
		});
	}
#endregion

#region Util Methods
	/// <summary>
	/// オブジェクトから、ユーザ名を取得する
	/// </summary>
	/// <returns>相手のNCMBUser</returns>
	/// <param name="obj">オブジェクト</param>
	public NCMBUser GetUserPointer(NCMBObject obj)
	{
		// toのポインタを取得
		NCMBUser user = base.GetTargetUser(obj, "to");

		// 取得したNCMBUserのobjectIdが一致しなければ、相手のポインタ
		if (user.ObjectId != NCMBUser.CurrentUser.ObjectId)
		{
			return user;
		}
		
		return base.GetTargetUser(obj, "from");
	}
#endregion
}