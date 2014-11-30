using UnityEngine;
using System.Collections;

namespace NCMBUT
{
	/// <summary>
	/// フレンド情報を保持するデータストアのクラス情報
	/// </summary>
	public class NCMBUTFriendsDefaultSettings
	{
		// mBaaS上のクラス名
		public static readonly string FRIEND_CLASS = "ncmbut_friends";

		// mBaaS上のフィールド名
		public enum FriendsDefaultFields
		{
			objectId,
			from,
			to,
			isEnable,
			isAccept,
			isPending,
			createDate,
			updateDate,
			acl
		}
	}
}