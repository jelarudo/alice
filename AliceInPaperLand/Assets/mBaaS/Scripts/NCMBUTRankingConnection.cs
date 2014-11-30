using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NCMB;
using NCMBUT;

[AddComponentMenu("Scripts/NCMBUT/Connection/Ranking Connection")]
public sealed class NCMBUTRankingConnection:NCMBUTConnectionBase
{
	// ステージ情報
	public int Stage = 0;

	// スコア情報の強制上書きフラグ
	public bool ForceUpdate = false;

	/// <summary>
	/// mBaaS標準のフィールドを返す
	/// 派生クラスでoverrideして、それぞれのフィールドを追加する
	/// </summary>
	/// <returns>フィールドリスト</returns>
	public override string[] GetDefaultFields()
	{
		return Enum.GetNames(typeof(NCMBUTRankingsDefaultSettings.RankingsDefaultFields));
	}

#region Get Ranking
	/// <summary>
	/// ランキングの一覧を取得する
	/// </summary>
	/// <param name="callback">コールバック関数</param>
	public void GetRankingList(ListCallback callback)
	{
		NCMBQuery<NCMBObject> query = GetQuery(NCMBUTRankingsDefaultSettings.RANKING_CLASS);
		query.WhereEqualTo(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.stage.ToString(), Stage);
		query.Include(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.player.ToString());
		query.FindAsync((List<NCMBObject> objList, NCMBException error) => {
			ClearValues();
			callback(objList, error);
			return;
		});
	}
#endregion

#region Send Score
	/// <summary>
	/// スコア送信を行う
	/// </summary>
	/// <param name="score">スコア</param>
	/// <param name="callback">コールバック関数</param>
	public void SendScore(int score, ErrorCallBack callback)
	{
		// 会員登録を行っていた場合、同会員のレコードを検索する
		if (GetIsLogIn)
		{
			NCMBQuery<NCMBObject> query = GetQuery(NCMBUTRankingsDefaultSettings.RANKING_CLASS);
			query.WhereEqualTo(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.player.ToString(), NCMBUser.CurrentUser);
			query.WhereEqualTo(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.stage.ToString(), Stage);
			query.FindAsync((List<NCMBObject> objList, NCMBException error) => {
				if (error != null)
				{
					callback(error);
					return;
				}

				if (objList.Count == 0)
				{
					// 対象会員のレコードがないため新規レコードを作成する
					NCMBObject scoreObj = GetClassObject(NCMBUTRankingsDefaultSettings.RANKING_CLASS);
					scoreObj.Add(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.player.ToString(), NCMBUser.CurrentUser);
					scoreObj.Add(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.score.ToString(), score);
					scoreObj.Add(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.stage.ToString(), Stage);
					saveRecord(scoreObj, callback);
				}
				else
				{
					// 強制上書きフラグがFalseの時かつ、スコアを更新するか確認する
					if (!ForceUpdate)
					{
						switch (Sort)
						{
							case NCMBUTSortType.Ascending:
								if (int.Parse(objList[0][NCMBUTRankingsDefaultSettings.RankingsDefaultFields.score.ToString()].ToString()) < score)
								{
									callback(null);
									return;
								}
								break;
							case NCMBUTSortType.Descending:
								if (int.Parse(objList[0][NCMBUTRankingsDefaultSettings.RankingsDefaultFields.score.ToString()].ToString()) > score)
								{
									callback(null);
									return;
								}
								break;
						}
					}
					objList[0][NCMBUTRankingsDefaultSettings.RankingsDefaultFields.score.ToString()] = score;
					saveRecord(objList[0], callback);
				}
			});
		}
		else
		{
			// 会員登録していないため常に新規でレコードを作成する
			NCMBObject createObj = GetClassObject(NCMBUTRankingsDefaultSettings.RANKING_CLASS);
			createObj.Add(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.score.ToString(), score);
			createObj.Add(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.stage.ToString(), Stage);
			saveRecord(createObj, callback);
		}
	}

	/// <summary>
	/// NCMBObjectの保存を行う
	/// </summary>
	/// <param name="targetObj">保存を行うNCMBObject</param>
	/// <param name="callback">コールバック関数</param>
	private void saveRecord(NCMBObject obj, ErrorCallBack callback)
	{
		obj.SaveAsync((NCMBException error) => {
			ClearValues();
			callback(error);
		});
	}
#endregion

#region Get User Score
	/// <summary>
	/// 現在の自分のスコアを取得する。
	/// ※ ログインが必須
	/// </summary>
	/// <param name="callback">コールバック関数</param>
	public void GetUserScore(IntCallback callback) {
		// ログイン状態の確認を行う
		if (!GetIsLogIn)
		{
			callback(0, new NCMBException(NCMBUTErrorMessage.NOT_LOGIN_ERROR));
			return;
		}

		NCMBQuery<NCMBObject> findQuery = GetPlaneQuery(NCMBUTRankingsDefaultSettings.RANKING_CLASS);
		findQuery.WhereEqualTo(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.player.ToString(), NCMBUser.CurrentUser);
		findQuery.WhereEqualTo(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.stage.ToString(), Stage);
		findQuery.FindAsync((List<NCMBObject> objList, NCMBException findError) => {
			// 検索結果がなければ0を返す
			if (objList.Count == 0)
			{
				callback(0, findError);
				return;
			}

			callback(int.Parse(objList[0]["score"].ToString()), findError);
		});
	}
#endregion

#region Ranking Info
	/// <summary>
	/// 現在の自分の順位を取得する。
	/// ※ ログインが必須
	/// </summary>
	/// <param name="callback">コールバック関数</param>
	public void GetCurrentRank(IntCallback callback)
	{
		// ログイン状態の確認を行う
		if (!GetIsLogIn)
		{
			callback(0, new NCMBException(NCMBUTErrorMessage.NOT_LOGIN_ERROR));
			return;
		}

		// 自分のスコアデータを取得する
		NCMBQuery<NCMBObject> findQuery = GetPlaneQuery(NCMBUTRankingsDefaultSettings.RANKING_CLASS);
		findQuery.WhereEqualTo(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.player.ToString(), NCMBUser.CurrentUser);
		findQuery.WhereEqualTo(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.stage.ToString(), Stage);
		findQuery.FindAsync((List<NCMBObject> objList, NCMBException findError) => {
			// 検索結果がなければ0を返す
			if (objList.Count == 0)
			{
				callback(0, findError);
				return;
			}

			// スコアデータを元に順位を取得
			NCMBQuery<NCMBObject> countQuery = GetQuery(NCMBUTRankingsDefaultSettings.RANKING_CLASS);

			switch (Sort)
			{
				case NCMBUTSortType.Ascending:
					countQuery.WhereLessThan(SortField, objList[0][SortField]);
					break;
				case NCMBUTSortType.Descending:
					countQuery.WhereGreaterThan(SortField, objList[0][SortField]);
					break;
			}

			countQuery.WhereEqualTo(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.stage.ToString(), Stage);
			countQuery.CountAsync((int count, NCMBException countError) => {
				callback(count + 1, countError);
				return;
			});
		});
	}
	
	/// <summary>
	/// mBaaS上のランキングクラスから総プレイヤー数を取得する
	/// </summary>
	/// <param name="callback">コールバック関数</param>
	public void GetTotalPlayers(IntCallback callback)
	{
		NCMBQuery<NCMBObject> query = GetQuery(NCMBUTRankingsDefaultSettings.RANKING_CLASS);
		query.WhereEqualTo(NCMBUTRankingsDefaultSettings.RankingsDefaultFields.stage.ToString(), Stage);
		query.CountAsync((int count, NCMBException error) => {
			callback(count, error);
			return;
		});
	}
#endregion

#region Util Methods
	/// <summary>
	/// ランキングのオブジェクトから、ユーザ名を取得する
	/// </summary>
	/// <returns>ユーザ名</returns>
	/// <param name="obj">ランキングのオブジェクト</param>
	/// <param name="fieldName">ユーザ名のフィールド(追加設定した場合)</param>
	public string GetRankingUserName(NCMBObject obj, string fieldName = "")
	{
		// 指定のクラスのオブジェクトでなければ、エラーとする
		base.CheckMatchClass(obj.ClassName, new string[]{NCMBUTRankingsDefaultSettings.RANKING_CLASS});

		NCMBUser user = base.GetTargetUser(obj, NCMBUTRankingsDefaultSettings.RankingsDefaultFields.player.ToString());
		if (user == null)
		{
			if (obj.ContainsKey(fieldName))
			{
				return obj[fieldName].ToString();
			}
			return "";
		}

		return user.UserName;
	}
#endregion
}
