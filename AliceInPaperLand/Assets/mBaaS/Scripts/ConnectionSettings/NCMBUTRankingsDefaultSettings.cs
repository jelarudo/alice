using UnityEngine;
using System.Collections;

namespace NCMBUT
{
	/// <summary>
	/// ランキング情報を保持するデータストアのクラス情報
	/// スコアとランキングは基本的に同じ構成
	/// </summary>
	public class NCMBUTRankingsDefaultSettings
	{
		// mBaaS上のクラス名
		public static readonly string RANKING_CLASS = "ncmbut_rankings";
		
		// mBaaS上のフィールド名
		public enum RankingsDefaultFields
		{
			objectId,
			stage,
			score,
			player,
			isEnable,
			createDate,
			updateDate,
			acl
		}
	}
}