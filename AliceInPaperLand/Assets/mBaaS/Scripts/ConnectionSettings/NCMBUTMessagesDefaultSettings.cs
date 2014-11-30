using UnityEngine;
using System.Collections;

namespace NCMBUT
{
	/// <summary>
	/// フレンド間のメッセージ情報を保持するデータストアのクラス情報
	/// </summary>
	public class NCMBUTMessagesDefaultSettings
	{
		// mBaaS上のクラス名
		public static readonly string MESSAGE_CLASS = "ncmbut_messages";

		// mBaaS上のフィールド名
		public enum MessagesDefaultFields
		{
			objectId,
			message,
			from,
			to,
			isEnable,
			createDate,
			updateDate,
			acl
		}
	}
}