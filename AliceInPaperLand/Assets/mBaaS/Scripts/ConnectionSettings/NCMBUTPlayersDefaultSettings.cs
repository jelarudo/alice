using UnityEngine;
using System.Collections;

namespace NCMBUT
{
	/// <summary>
	/// ユーザ検索時のユーザ情報を保持するデータストアのクラス情報
	/// </summary>
	public class NCMBUTPlayersDefaultSettings
	{
		// mBaaS上のクラス名
		public static readonly string PLAYERS_CLASS = "ncmbut_players";
	
		// mBaaS上のフィールド名
		public enum PlayersDefaultFields
		{
			objectId,
			player,
			userName,
			isEnable,
			createDate,
			updateDate,
			acl
		}
	}
}