/// <summary>
/// ACLの設定をInspectorに表示するクラス
/// </summary>
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace NCMBUT.EditorTools
{
	/// <summary>
	/// ACLの設定を行うクラス
	/// </summary>
	public class NCMBUTCustomAclSettings
	{
		private GUIContent targetContent = new GUIContent("Target", "対象の設定を行います");
		private GUIContent targetIdContent = new GUIContent("Target ID", "対象の会員のobjectIdの設定を行います");
		private GUIContent readContent = new GUIContent("Read", "読込の設定を行います");
		private GUIContent writeContent = new GUIContent("Write", "更新・削除の設定を行います");
		private GUIContent aclSettingsContent = new GUIContent("ACL Settings", "ACLの設定を行います。ACLについては、mBaaSドキュメントを参照してください");
		private GUIContent defaultAclContent = new GUIContent("Default ACL", "標準のACLを使用する設定を行います");

		private NCMBUTCustomEditorBase customEditor;
		private NCMBUTConnectionBase connection;

		/// <summary>
		/// コンストラクタ <see cref="NCMBUT.EditorTools.NCMBUTCustomAclSettings"/> 
		/// </summary>
		/// <param name="custom">エディタのルートクラス</param>
		public NCMBUTCustomAclSettings(NCMBUTCustomEditorBase custom)
		{
			customEditor = custom;
			connection = custom.target as NCMBUTConnectionBase;
		}

		private Vector2 scrollPos;
		private NCMBUTInputError inputError = NCMBUTInputError.None;
		private NCMBUTCustomDataInputError dataInputError = new NCMBUTCustomDataInputError();
		private ReorderableList list;

		/// <summary>
		/// エディタルートから呼ぶ。ReorderableListの初期化
		/// </summary>
		public void OnEnable()
		{
			// リストの初期化
			list = new ReorderableList(customEditor.serializedObject, customEditor.serializedObject.FindProperty("AclDataList"), true, true, true, true);

			// ヘッダーの作成
			list.drawHeaderCallback = (Rect rect) => {
				EditorGUI.LabelField(new Rect(rect.x + 22, rect.y, 90, EditorGUIUtility.singleLineHeight), targetContent);
				EditorGUI.LabelField(new Rect(rect.x + rect.width * 0.5f - 40, rect.y, 84, EditorGUIUtility.singleLineHeight), targetIdContent);
				EditorGUI.LabelField(new Rect(rect.x + rect.width - 100, rect.y, 70, EditorGUIUtility.singleLineHeight), readContent);
				EditorGUI.LabelField(new Rect(rect.x + rect.width - 65, rect.y, 70, EditorGUIUtility.singleLineHeight), writeContent);
			};

			// エレメント一覧
			list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				if (index >= connection.AclDataList.Count)
					return;

				SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
				rect.y += 2;

				string[] targetList = getTargetList(connection.AclDataList[index].Type);
				int tmpIndex = getIndex(targetList, connection.AclDataList[index].Type.ToString());
				tmpIndex = (int)EditorGUI.Popup(new Rect(rect.x, rect.y, 90, EditorGUIUtility.singleLineHeight), tmpIndex, targetList);
				if (GUI.changed)
				{
					Undo.RecordObject(connection, "Change Target");
					connection.AclDataList[index].Type = getAclType(targetList[tmpIndex]);
					customEditor.serializedObject.ApplyModifiedProperties();
				}

				EditorGUI.BeginDisabledGroup(connection.AclDataList[index].Type == NCMBUTACLType.All);
				EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 150, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("ObjectId"), GUIContent.none);
				EditorGUI.EndDisabledGroup();
				EditorGUI.PropertyField(new Rect(rect.x + rect.width - 90, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("IsRead"), GUIContent.none);
				EditorGUI.PropertyField(new Rect(rect.x + rect.width - 55, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("IsWrite"), GUIContent.none);
			};

			// Addメニューの作成
			list.onAddDropdownCallback = (Rect rect, ReorderableList l) => {
				GenericMenu menu = new GenericMenu();
				if (connection.AclDataList.Where(o => o.Type == NCMBUTACLType.All).Count() == 0)
				{
					menu.AddItem(new GUIContent("All"), false, selectAddObject, "All");
				}
				menu.AddItem(new GUIContent("Member"), false, selectAddObject, "Member");
				menu.ShowAsContext();
			};
		}

		/// <summary>
		/// エレメントの追加
		/// </summary>
		/// <param name="target">選択文字列</param>
		private void selectAddObject(object target)
		{
			// サイズ増やす
			int index = list.serializedProperty.arraySize;
			++list.serializedProperty.arraySize;

			// インデックス取得
			list.index = index;

			// 追加したインデックスのエレメント初期化
			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			if (target.ToString() == "All")
			{
				element.FindPropertyRelative("Type").enumValueIndex = (int)NCMBUTACLType.All;
			}
			else
			{
				element.FindPropertyRelative("Type").enumValueIndex = (int)NCMBUTACLType.Member;
			}
			element.FindPropertyRelative("ObjectId").stringValue = "";
			element.FindPropertyRelative("IsRead").boolValue = true;
			element.FindPropertyRelative("IsWrite").boolValue = true;

			// 保存
			customEditor.serializedObject.ApplyModifiedProperties();
		}

		/// <summary>
		/// ACL設定の描画
		/// </summary>
		public void DrawAclSettings()
		{
			connection.IsOpenAcl = EditorGUILayout.Foldout(connection.IsOpenAcl, aclSettingsContent);

			if (connection.IsOpenAcl)
			{
				++EditorGUI.indentLevel;

				defaultPermission();

				if (!connection.UseDefaultPermission)
				{
					if (list == null)
					{
						OnEnable();
					}
					list.DoLayoutList();

					checkErrorField();
					checkEmptyACL();
				}

				--EditorGUI.indentLevel;
			}
		}

		/// <summary>
		/// デフォルトのACL設定を使うかどうかの設定
		/// </summary>
		private void defaultPermission()
		{
			EditorGUILayout.PropertyField(customEditor.serializedObject.FindProperty("UseDefaultPermission"), defaultAclContent);
		}

		/// <summary>
		/// 入力エラーの表示
		/// </summary>
		private void checkErrorField()
		{
			inputError = dataInputError.CheckKey(connection.AclDataList);
			dataInputError.DrawInputError(inputError);
		}

		/// <summary>
		/// 権限が無い場合は警告出す
		/// </summary>
		private void checkEmptyACL()
		{
			if (connection.AclDataList.Any(o => !o.IsRead && !o.IsWrite))
			{
				EditorGUILayout.HelpBox(NCMBUTEditorErrorMessage.ACL_IGNORED, MessageType.Warning);
			}
		}

		/// <summary>
		/// 対象リストの取得。Allは重複不可
		/// </summary>
		/// <returns>対象リスト</returns>
		/// <param name="type">選択中の対象</param>
		private string[] getTargetList(NCMBUTACLType type)
		{
			string[] enumList = Enum.GetNames(typeof(NCMBUTACLType));

			// 他にAllがあり、かつ選択中がAllでない場合はtrue
			bool hasAll = (connection.AclDataList.Where(o => o.Type == NCMBUTACLType.All).Count() > 0 && type != NCMBUTACLType.All);

			int len = enumList.Length;
			if (hasAll)
			{
				len -= 1;
			}
			string[] list = new string[len];

			int count = 0;
			for (int i = 0; i < enumList.Length; ++i)
			{
				if (hasAll && i == (int)NCMBUTACLType.All)
				{
					continue;
				}
				list[count] = enumList[i];
				++count;
			}

			return list;
		}

		/// <summary>
		/// リスト内で一致するインデックスを取得する。一致するものがなければ0を返す
		/// </summary>
		/// <returns>インデックス</returns>
		/// <param name="list">対象リスト</param>
		/// <param name="curret">選択中の対象</param>
		private int getIndex(string[] list, string curret)
		{
			for (int i = 0; i < list.Length; ++i)
			{
				if (list[i] == curret)
				{
					return i;
				}
			}
			return 0;
		}

		/// <summary>
		/// 文字列からACLのタイプを取得する
		/// </summary>
		/// <returns>ACLのタイプ</returns>
		/// <param name="aclType">ACLの文字列</param>		
		private NCMBUTACLType getAclType(string aclType)
		{
			if (aclType == NCMBUTACLType.All.ToString())
			{
				return NCMBUTACLType.All;
			}
			else if (aclType == NCMBUTACLType.Member.ToString())
			{
				return NCMBUTACLType.Member;
			}

			return NCMBUTACLType.All;
		}
	}
}