using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NCMB;

/// <summary>
/// フレンド機能使用時のテンプレートクラス
/// </summary>
public class NCMBUTFriendSample:MonoBehaviour
{
	/// <summary>
	/// フレンドのコントローラクラスを代入する変数
	/// Inspector上で、HierarchyにあるNCMBUTFriendConnectionのプレハブをアタッチする
	/// </summary>
	public NCMBUTFriendConnection Connection;
	
#region Search Users Methods
	/// <summary>
	/// ユーザ検索を行うメソッド
	/// </summary>
	/// <param name="targetName">検索するユーザ名</param>
	private void getSearchUserList(string targetName)
	{
		Connection.GetSearchUserList(targetName, new ListCallback(setSearchUserList));
	}
	
	/// <summary>
	/// ユーザ検索の結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="objList">ユーザ一覧</param>
	/// <param name="error">エラーの有無</param>
	private void setSearchUserList(List<NCMBObject> objList, NCMBException error)
	{
		searchUsersList = objList;
		systemMessage = getSystemMessage(error);
	}
#endregion
	
#region Send Friend Request Methods
	/// <summary>
	/// フレンド申請を送るメソッド
	/// </summary>
	/// <param name="target">申請するユーザ</param>
	private void sendFriendRequest(NCMBObject target)
	{
		Connection.SendFriendRequest(target, new ErrorCallBack(setSendFriendRequestError));
	}
	
	/// <summary>
	/// フレンド申請の結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="error">エラーの有無</param>
	private void setSendFriendRequestError(NCMBException error)
	{
		systemMessage = getSystemMessage(error);
	}
#endregion
	
#region Accept Friend Request Methods
	/// <summary>
	/// フレンド申請の承認を行うメソッド
	/// </summary>
	/// <param name="target">承認するユーザ</param>
	private void acceptFriendRequest(NCMBObject target)
	{
		Connection.AcceptFriendRequest(target, new ErrorCallBack(setAcceptFriendRequestError));
	}
	
	/// <summary>
	/// フレンド申請の承認の結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="error">エラーの有無</param>
	private void setAcceptFriendRequestError(NCMBException error)
	{
		systemMessage = getSystemMessage(error);
	}
#endregion
	
#region Decline Friend Request Methods
	/// <summary>
	/// フレンド申請の拒否を行うメソッド
	/// </summary>
	/// <param name="target">拒否するユーザ</param>
	private void declineFriendRequest(NCMBObject target)
	{
		Connection.DeclineFriendRequest(target, new ErrorCallBack(setDeclineFriendRequestError));
	}
	
	/// <summary>
	/// フレンド申請の拒否の結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="error">エラーの有無</param>
	private void setDeclineFriendRequestError(NCMBException error)
	{
		systemMessage = getSystemMessage(error);
	}
#endregion
	
#region Friend Request Cancel Methods
	/// <summary>
	/// フレンド申請をキャンセルするメソッド
	/// </summary>
	/// <param name="target">キャンセルするフレンド申請</param>
	private void cancelFriendRequest(NCMBObject target)
	{
		Connection.CancelRequest(target, new ErrorCallBack(setCancelFriendRequestError));
	}
	
	/// <summary>
	/// フレンド申請のキャンセルの結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="error">エラーの有無</param>
	private void setCancelFriendRequestError(NCMBException error)
	{
		systemMessage = getSystemMessage(error);
	}
#endregion

#region Get Friend List Methods
	/// <summary>
	/// フレンド一覧を取得するメソッド
	/// </summary>
	private void getFriendList()
	{
		Connection.GetFriendList(new ListCallback(setFriendList));
	}
	
	/// <summary>
	/// フレンド一覧の結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="objList">フレンド一覧</param>
	/// <param name="error">エラーの有無</param>
	private void setFriendList(List<NCMBObject> objList, NCMBException error)
	{
		friendsList = objList;
		systemMessage = getSystemMessage(error);
	}
#endregion

#region Remove Friend Methods
	/// <summary>
	/// フレンドの削除を行うメソッド
	/// </summary>
	/// <param name="friend">削除するユーザ</param>
	private void removeFriend(NCMBObject friend)
	{
		Connection.RemoveFriend(friend, new ErrorCallBack(setRemoveFriendError));
	}
	
	/// <summary>
	/// フレンドの削除の結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="error">エラーの有無</param>
	private void setRemoveFriendError(NCMBException error)
	{
		systemMessage = getSystemMessage(error);
	}
#endregion
	
#region Get Message List Methods
	/// <summary>
	/// メッセージ一覧を取得するメソッド
	/// </summary>
	private void getMessageList()
	{
		Connection.GetMessageList(new ListCallback(setMessageList));
	}
	
	/// <summary>
	/// メッセージ一覧の結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="objList">メッセージ一覧</param>
	/// <param name="error">エラーの有無</param>
	private void setMessageList(List<NCMBObject> objList, NCMBException error)
	{
		messagesList = objList;
		systemMessage = getSystemMessage(error);
	}
#endregion
	
#region Send Message Methods
	/// <summary>
	/// メッセージを送信するメソッド
	/// </summary>
	/// <param name="friend">メッセージを送るユーザ</param>
	/// <param name="message">メッセージ</param>
	private void sendMessage(NCMBUser friend, string message)
	{
		Connection.SendMessage(friend, message, new ErrorCallBack(setSendMessageError));
	}
	
	/// <summary>
	/// メッセージを送信した結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="error">エラーの有無</param>
	private void setSendMessageError(NCMBException error)
	{
		systemMessage = getSystemMessage(error);
	}
#endregion
	
#region Remove Message Methods
	/// <summary>
	/// メッセージを削除するメソッド
	/// </summary>
	/// <param name="target">削除するメッセージ</param>
	private void removeMessage(NCMBObject target)
	{
		Connection.RemoveMessage(target, new ErrorCallBack(setRemoveMessageError));
	}
	
	/// <summary>
	/// メッセージの削除の結果を受け取るメソッド
	/// エラーがnullだと成功
	/// </summary>
	/// <param name="error">エラーの有無</param>
	private void setRemoveMessageError(NCMBException error)
	{
		systemMessage = getSystemMessage(error);
	}
#endregion

#region GUI Methods
	private List<NCMBObject> friendsList = null;
	private List<NCMBObject> searchUsersList = null;
	private List<NCMBObject> messagesList = null;
	private string systemMessage = "";
	private string inputName = "";
	private string message = "";
	private float margin = 10.0f;
	private Vector2 sendPosition;
	private Vector2 messagePosition;
	private Vector2 friendsPosition;
	private Vector2 userPosition;

	/// <summary>
	/// フレンドのGUIを表示
	/// </summary>
	void OnGUI()
	{
		guiSettings();

		float contentsWidth = Screen.width - margin * 2.0f;
		
		Rect titleRect = new Rect(margin, 0, contentsWidth, Screen.height * 0.1f);
		drawTitle(titleRect);

		Rect searchRect = new Rect(margin, titleRect.y + titleRect.height, contentsWidth * 0.4f, Screen.height * 0.35f);
		drawSearch(searchRect);

		Rect friendRect = new Rect(margin + searchRect.width, titleRect.y + titleRect.height, contentsWidth * 0.6f, Screen.height * 0.50f);
		drawFriend(friendRect);

		Rect showMessageRect = new Rect(margin, searchRect.y + searchRect.height, contentsWidth * 0.4f, Screen.height * 0.35f);
		drawShowMessage(showMessageRect);

		Rect sendMessageRect = new Rect(margin + showMessageRect.width, friendRect.y + friendRect.height, contentsWidth * 0.6f, Screen.height * 0.20f);
		drawSendMessage(sendMessageRect);

		Rect errorRect = new Rect(margin, showMessageRect.y + showMessageRect.height, contentsWidth, Screen.height * 0.1f);
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
		GUILayout.Label("Friend Sample"); 
		GUILayout.EndArea();
	}

	/// <summary>
	/// フレンド一覧の表示
	/// </summary>
	/// <param name="rect">表示範囲</param>
	private void drawFriend(Rect rect)
	{
		GUI.Box(rect, "");

		GUILayout.BeginArea(rect);
		GUILayout.Label("Friend List");
		if (GUILayout.Button("Get Friend List", GUILayout.MinHeight(30)))
		{
			getFriendList();
		}

		// 一覧がnullでなければ表示
		if (friendsList != null)
		{
			GUILayout.Space(0.1f);
			friendsPosition = GUILayout.BeginScrollView(friendsPosition);
			for (int i = 0; i < friendsList.Count; ++i)
			{
				// toにフレンドの情報を持つ
				NCMBUser friend = Connection.GetUserPointer(friendsList[i]);
				GUILayout.BeginHorizontal();
				GUILayout.Label(friend["userName"].ToString());

				if ((bool)friendsList[i]["isPending"])
				{
					GUILayout.Label("Please...Waiting for Accept :)");
					if (GUILayout.Button("Cancel", GUILayout.MinHeight(30)))
					{
						cancelFriendRequest(friendsList[i]);
						friendsList.RemoveAt(i);
					}
				}
				else
				{
					// 承認済みかどうか
					if ((bool)friendsList[i]["isAccept"])
					{
						if (GUILayout.Button("Send Message", GUILayout.MinHeight(30)))
						{
							sendMessage(friend, message);
							message = "";
						}

						if (GUILayout.Button("Remove", GUILayout.MinHeight(30)))
						{
							removeFriend(friendsList[i]);
							friendsList.RemoveAt(i);
						}
					}
					else
					{
						if (GUILayout.Button("Accept", GUILayout.MinHeight(30)))
						{
							acceptFriendRequest(friendsList[i]);
						}

						if (GUILayout.Button("Decline", GUILayout.MinHeight(30)))
						{
							declineFriendRequest(friendsList[i]);
							friendsList.RemoveAt(i);
						}
					}
				}
				GUILayout.EndHorizontal();
			}
			GUILayout.EndScrollView();
			GUILayout.Space(0.1f);
		}
		GUILayout.EndArea();
	}

	/// <summary>
	/// メッセージ入力フォームの表示
	/// </summary>
	/// <param name="rect">表示範囲</param>
	private void drawSendMessage(Rect rect)
	{
		GUI.Box(rect, "");

		GUILayout.BeginArea(rect);
		GUILayout.Label("Message Form");
		sendPosition = GUILayout.BeginScrollView(sendPosition);
		message = GUILayout.TextArea(message, GUILayout.Height(rect.height * 0.75f));
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	/// <summary>
	/// メッセージ一覧の表示
	/// </summary>
	/// <param name="rect">表示範囲</param>
	private void drawShowMessage(Rect rect)
	{
		GUI.Box(rect, "");

		GUILayout.BeginArea(rect);
		GUILayout.Label("Message List");
		if (GUILayout.Button("Get Messages", GUILayout.MinHeight(30)))
		{
			getMessageList();
		}

		// 一覧がnullでなければ表示
		if (messagesList != null)
		{
			GUILayout.Space(0.1f);
			messagePosition = GUILayout.BeginScrollView(messagePosition);
			for (int i = 0; i < messagesList.Count; ++i)
			{
				// 送り元
				NCMBUser sender = Connection.GetUserPointer(messagesList[i]);
				GUILayout.Label("From: " + sender.UserName);
				GUILayout.BeginHorizontal();

				GUILayout.Label(messagesList[i]["message"].ToString());
				if (GUILayout.Button("Delete", GUILayout.MaxWidth(60), GUILayout.MinHeight(30)))
				{
					removeMessage(messagesList[i]);
					messagesList.RemoveAt(i);
				}
				GUILayout.EndHorizontal();
				GUI.skin.label.fontSize = 10;
				GUILayout.Label("___________________");
				GUI.skin.label.fontSize = 20;
			}
			GUILayout.EndScrollView();
			GUILayout.Space(0.1f);
		}
		GUILayout.EndArea();
	}

	/// <summary>
	/// ユーザ検索の表示
	/// </summary>
	/// <param name="rect">表示範囲</param>
	private void drawSearch(Rect rect)
	{
		GUI.Box(rect, "");

		GUILayout.BeginArea(rect);
		GUILayout.Label("Search User");
		GUILayout.BeginHorizontal();
		inputName = GUILayout.TextField(inputName, GUILayout.Width(rect.width * 0.70f), GUILayout.MinHeight(30));
		if (GUILayout.Button("Search", GUILayout.MinHeight(30)))
		{
			getSearchUserList(inputName);
		}
		GUILayout.EndHorizontal();

		// 一覧がnullでなければ表示
		if (searchUsersList != null)
		{
			GUILayout.Space(0.1f);
			userPosition = GUILayout.BeginScrollView(userPosition);
			for (int i = 0; i < searchUsersList.Count; ++i)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(searchUsersList[i]["userName"].ToString());
				if (GUILayout.Button("Request", GUILayout.MinHeight(30)))
				{
					sendFriendRequest((NCMBObject)searchUsersList[i]);
					searchUsersList.RemoveAt(i);
				}
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