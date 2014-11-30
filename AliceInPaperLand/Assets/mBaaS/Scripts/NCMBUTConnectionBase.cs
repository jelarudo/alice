using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using NCMB;

/// <summary>
/// 通信を行うクラスの親クラス
/// </summary>
namespace NCMBUT
{
	public abstract class NCMBUTConnectionBase:MonoBehaviour
	{
#if UNITY_EDITOR
		/// <summary>
		/// InspectorのFoldOutのフラグ
		/// UnityEditor上のみ有効
		/// </summary>
		public bool IsOpenOptions = false;
		public bool IsOpenQuery = true;
		public bool IsOpenField = true;
		public bool IsOpenAcl = true;
#endif

#region Setting Parameters
		/// <summary>
		/// クエリ設定のパラメータ
		/// </summary>
		protected bool UseQuerySettings = true;
		public NCMBUTSortType Sort = NCMBUTSortType.Ascending;
		public string SortField = "createDate";
		public int Skip = 0;
		public int Limit = 100;

		/// <summary>
		/// ACL設定のパラメータ
		/// </summary>
		protected bool UseAclSettings = true;
		public bool UseDefaultPermission = true;
		public List<NCMBUTACLData> AclDataList = new List<NCMBUTACLData>();

		/// <summary>
		/// フィールド設定のパラメータ
		/// </summary>
		protected bool UseFieldSettings = true;
		public List<NCMBUTFieldData> FieldDataList = new List<NCMBUTFieldData>();
#endregion

#region Inspector Setting Methods
		/// <summary>
		/// クエリ設定を行うかどうかの設定
		/// 派生クラスでoverrideして変更する
		/// </summary>
		/// <returns>クエリ設定を使用するならば<c>true</c>を返し、使用しない場合は<c>false</c>を返す</returns>
		public virtual bool GetUseQuerySettings()
		{
			UseQuerySettings = true;
			return UseQuerySettings;
		}

		/// <summary>
		/// フィールド設定を行うかどうかの設定
		/// 派生クラスでoverrideして変更する
		/// </summary>
		/// <returns>フィールド設定を使用するならば<c>true</c>を返し、使用しない場合は<c>false</c>を返す</returns>
		public virtual bool GetUseFieldSettings()
		{
			UseFieldSettings = true;
			return UseFieldSettings;
		}

		/// <summary>
		/// ACL設定を行うかどうかの設定
		/// 派生クラスでoverrideして変更する
		/// </summary>
		/// <returns>ACL設定を使用するならば<c>true</c>を返し、使用しない場合は<c>false</c>を返す</returns>
		public virtual bool GetUseAclSettings()
		{
			UseAclSettings = true;
			return UseAclSettings;
		}

		/// <summary>
		/// mBaaS標準のフィールドを返す
		/// 派生クラスでoverrideして、それぞれのフィールドを追加する
		/// </summary>
		/// <returns>フィールドリスト</returns>
		public abstract string[] GetDefaultFields();
#endregion

		/// <summary>
		/// NCMBSettingsがHierarchyになければ、作成してキー設定を行う
		/// キーは、ScriptableObjectに設定した値を代入する
		/// </summary>
		protected virtual void Awake()
		{
			if (FindObjectOfType(typeof(NCMBSettings)) == null)
			{
				NCMBUTKeySettings data = (NCMBUTKeySettings)Resources.Load("NCMBUTKeySettings");
			
				GameObject obj = new GameObject("Settings");
				obj.AddComponent<NCMBSettings>();
			
				NCMBSettings.ApplicationKey = data.ApplicationKey;
				NCMBSettings.ClientKey = data.ClientKey;
			}
		}

		/// <summary>
		/// 追加したフィールドに、値を設定する
		/// </summary>
		/// <param name="key">追加したフィールド名</param>
		/// <param name="value">設定したい値</param>
		public void SetValue(string key, object value)
		{
			if (FieldDataList.Where(o => o.Key == key).Count() > 0)
			{
				int index = FieldDataList.FindIndex(o => o.Key == key);
				NCMBUTTypeUtil.CheckVariableType(FieldDataList[index], value);
				FieldDataList[index].Value = value;
			}
			else
			{
				throw new ArgumentException(NCMBUTErrorMessage.NO_GIVEN_KEY);
			}
		}

		/// <summary>
		/// 追加した値をクリアする
		/// </summary>
		protected void ClearValues()
		{
			foreach (NCMBUTFieldData data in FieldDataList)
			{
				data.Value = null;
			}
		}

		/// <summary>
		/// ログイン状態を取得する
		/// </summary>
		/// <returns>ログインしている時に<c>true</c>を返し、していない時に<c>false</c>を返す</returns>
		public bool GetIsLogIn
		{
			get
			{
				return (NCMBUser.CurrentUser != null && NCMBUser.CurrentUser.SessionToken != "");
			}
		}

#region Create NCMBObject
		/// <summary>
		/// 設定したパラメータを反映したNCMBUserを取得する
		/// </summary>
		/// <returns>NCMBUserのインスタンス</returns>
		protected NCMBUser GetUserObject()
		{
			NCMBUser obj = new NCMBUser();
			obj = objectSettings<NCMBUser>(obj);
			return obj;
		}

		/// <summary>
		/// 設定したパラメータを反映したNCMBObjectのインスタンスを取得する
		/// </summary>
		/// <returns>NCMBObjectのインスタンス</returns>
		/// <param name="className">クラス名</param>
		protected NCMBObject GetClassObject(string className)
		{
			NCMBObject obj = GetPlaneClassObject(className);
			obj = objectSettings<NCMBObject>(obj);
			return obj;
		}

		/// <summary>
		/// NCMBObjectの新規作成
		/// </summary>
		/// <returns>NCMBObject</returns>
		/// <param name="className">クラス名</param>
		protected NCMBObject GetPlaneClassObject(string className)
		{
			NCMBObject obj = new NCMBObject(className);
			obj.Add("isEnable", true);
			return obj;
		}

		/// <summary>
		/// NCMBObjectに、フィールドとACLの設定を追加する
		/// </summary>
		/// <returns>設定を追加したNCMBObject</returns>
		/// <param name="obj">設定を追加したいNCMBObjectのインスタンス</param>
		/// <typeparam name="T">NCMBObjectとその派生クラス</typeparam>
		private T objectSettings<T>(T obj) where T:NCMBObject
		{
			if (UseFieldSettings)
			{
				obj = addKeyValues<T>(obj);
			}

			if (UseAclSettings)
			{
				NCMBACL acl = SetACL();
				if (acl != null)
				{
					obj.ACL = acl;
				}
			}

			return obj;
		}

		/// <summary>
		/// NCMBObjectに、フィールドの設定を追加する
		/// </summary>
		/// <returns>設定を追加したNCMBObject</returns>
		/// <param name="obj">設定を追加したいNCMBObjectのインスタンス</param>
		/// <typeparam name="T">NCMBObjectとその派生クラス</typeparam>
		private T addKeyValues<T>(T obj) where T:NCMBObject
		{
			foreach (NCMBUTFieldData data in FieldDataList)
			{
				if (data.IsRequire && data.Value == null)
				{
					throw new ArgumentException(NCMBUTErrorMessage.REQUIRE_KEY, data.Key);
				}
				else if (!data.IsRequire && data.Value == null)
				{
					continue;
				}
				obj.Add(data.Key, data.Value);
			}

			return obj;
		}
		
		/// <summary>
		/// 設定したACLを返す
		/// デフォルトのACL使用時や、設定数が0の時はnullを返す
		/// </summary>
		/// <returns>ACL</returns>
		private NCMBACL SetACL()
		{
			if (UseDefaultPermission || AclDataList.Count == 0)
			{
				return null;
			}
			
			NCMBACL acl = new NCMBACL();

			bool settingFlag = false;

			foreach (NCMBUTACLData data in AclDataList)
			{
				if (!settingFlag && (data.IsRead || data.IsWrite))
				{
					settingFlag = true;
				}

				switch (data.Type)
				{
					case NCMBUTACLType.All:
						acl.PublicReadAccess = data.IsRead;
						acl.PublicWriteAccess = data.IsWrite;
						break;
					case NCMBUTACLType.Member:
						acl.SetReadAccess(data.ObjectId, data.IsRead);
						acl.SetWriteAccess(data.ObjectId, data.IsWrite);
						break;
				}
			}

			if (!settingFlag)
			{
				return null;
			}

			return acl;
		}
#endregion

#region Create NCMBQuery
		/// <summary>
		/// 設定を反映したクエリを返す
		/// </summary>
		/// <returns>設定を反映したNCMBQueryのインスタンス</returns>
		/// <param name="className">クエリを流したいクラス名</param>
		protected NCMBQuery<NCMBObject> GetQuery(string className)
		{
			NCMBQuery<NCMBObject> query = GetPlaneQuery(className);
			if (UseQuerySettings)
			{
				query = QuerySetting(query);
			}
			return query;
		}

		/// <summary>
		/// NCMBQueryの新規作成
		/// </summary>
		/// <returns>NCMBQuery</returns>
		/// <param name="className">クラス名</param>
		protected NCMBQuery<NCMBObject> GetPlaneQuery(string className)
		{
			NCMBQuery<NCMBObject> query = new NCMBQuery<NCMBObject>(className);
			query.WhereEqualTo("isEnable", true);
			return query;
		}
		
		/// <summary>
		/// クエリ設定の反映を行う
		/// </summary>
		/// <returns>設定を反映したNCMBQueryのインスタンス</returns>
		/// <param name="query">設定を追加したいNCMBQueryのインスタンス</param>
		protected virtual NCMBQuery<NCMBObject> QuerySetting(NCMBQuery<NCMBObject> query)
		{
			switch (Sort)
			{
				case NCMBUTSortType.Ascending:
					query.OrderByAscending(SortField);
					break;
				case NCMBUTSortType.Descending:
					query.OrderByDescending(SortField);
					break;
			}
						
			query.Limit = Limit;
			query.Skip = Skip;

			return query;
		}
#endregion

		/// <summary>
		/// オブジェクト内から、NCMBUserのポインタを取得する
		/// </summary>
		/// <returns>NCMBUser</returns>
		/// <param name="record">オブジェクト</param>
		/// <param name="field">対象のフィールド</param>
		protected NCMBUser GetTargetUser(NCMBObject record, string field)
		{
			NCMBUser user = null;
			if (record.ContainsKey(field) && record[field] != null)
			{
				user = (NCMBUser)record[field];
			}
			return user;
		}

		/// <summary>
		/// 引数に渡したオブジェクトが、処理するクラスか同化の確認をする
		/// </summary>
		/// <param name="objectClassName">オブジェクトのクラス名</param>
		/// <param name="correctClassName">処理するオブジェクトのクラス名</param>
		protected void CheckMatchClass(string objectClassName, string[] correctClassName)
		{
			for (int i = 0; i < correctClassName.Length; ++i)
			{
				if (objectClassName == correctClassName[i])
				{
					return;
				}
			}

			throw new ArgumentException(NCMBUTErrorMessage.NO_MATCH_CLASS_NAME);
		}
	}
}