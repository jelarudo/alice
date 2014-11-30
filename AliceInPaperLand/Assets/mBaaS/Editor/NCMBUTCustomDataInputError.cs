/// <summary>
/// Fieldのキー入力チェックを行うクラス
/// </summary>
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NCMBUT.EditorTools
{
	/// <summary>
	/// 入力した内容の確認
	/// </summary>
	public class NCMBUTCustomDataInputError
	{
		/// <summary>
		/// キーのチェック
		/// </summary>
		/// <returns>エラー</returns>
		/// <param name="list">Fieldデータリスト</param>
		public NCMBUTInputError CheckKey(List<NCMBUTFieldData> list, string[] defaultField)
		{
			foreach (NCMBUTFieldData data in list)
			{
				// キーが空
				if (data.Key.Length == 0)
				{
					return NCMBUTInputError.Empty;
				}
			
				// キーが既にある
				if (list.Where(o => o.Key == data.Key).Count() > 1)
				{
					return NCMBUTInputError.Duplicate;
				}

				if (defaultField.Where(o => o == data.Key).Count() > 0)
				{
					return NCMBUTInputError.Duplicate;
				}
			
				// キーが半角英数+_のみかどうか
				for (int i = 0; i < data.Key.Length; ++i)
				{
					string subStr = data.Key.Substring(i, 1);
					if (!Regex.IsMatch(subStr, "[$_a-zA-Z0-9]"))
					{
						return NCMBUTInputError.Validation;
					}
				}
			
				// キー数字から始まる
				if (Regex.IsMatch(data.Key.Substring(0, 1), "[0-9]"))
				{
					return NCMBUTInputError.Type;
				}
			
				// キーが使えないフィールド名
				if (data.Key == "_id")
				{
					return NCMBUTInputError.Resorve;
				}
			}
		
			return NCMBUTInputError.None;
		}

		/// <summary>
		/// キーのチェック
		/// </summary>
		/// <returns>エラー</returns>
		/// <param name="list">Fieldデータリスト</param>
		public NCMBUTInputError CheckKey(List<NCMBUTACLData> list)
		{
			foreach (NCMBUTACLData data in list)
			{
				if (data.Type == NCMBUTACLType.All)
				{
					continue;
				}

				// キーが空
				if (data.ObjectId.Length == 0)
				{
					return NCMBUTInputError.AclEmpty;
				}
				
				// キーが既にある
				if (list.Where(o => o.ObjectId == data.ObjectId).Count() > 1)
				{
					return NCMBUTInputError.Duplicate;
				}
				
				// キーが半角英数+_のみかどうか
				for (int i = 0; i < data.ObjectId.Length; ++i)
				{
					string subStr = data.ObjectId.Substring(i, 1);
					if (!Regex.IsMatch(subStr, "[$_a-zA-Z0-9]"))
					{
						return NCMBUTInputError.Validation;
					}
				}
			}
			
			return NCMBUTInputError.None;
		}
	
		/// <summary>
		/// エラーの描画
		/// </summary>
		/// <param name="inputError">エラータイプ</param>
		public void DrawInputError(NCMBUTInputError inputError)
		{
			// エラーがある場合はInspectorに表示
			switch (inputError)
			{
				case NCMBUTInputError.Empty:
					EditorGUILayout.HelpBox(NCMBUTEditorErrorMessage.EMPTY_KEY, MessageType.Error);
					break;
				case NCMBUTInputError.Duplicate:
					EditorGUILayout.HelpBox(NCMBUTEditorErrorMessage.DUPLICATION_KEY, MessageType.Error);
					break;
				case NCMBUTInputError.Validation:
					EditorGUILayout.HelpBox(NCMBUTEditorErrorMessage.VALIDATION_KEY, MessageType.Error);
					break;
				case NCMBUTInputError.Type:
					EditorGUILayout.HelpBox(NCMBUTEditorErrorMessage.TYPE_KEY, MessageType.Error);
					break;
				case NCMBUTInputError.Resorve:
					EditorGUILayout.HelpBox(NCMBUTEditorErrorMessage.RESERVED_KEY, MessageType.Error);
					break;
				case NCMBUTInputError.AclEmpty:
					EditorGUILayout.HelpBox(NCMBUTEditorErrorMessage.ACL_EMPTY, MessageType.Error);
					break;
			}
		}
	}
}