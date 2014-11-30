/// <summary>
/// クエリ設定の描画クラス
/// </summary>
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

namespace NCMBUT.EditorTools
{
	/// <summary>
	/// クエリの設定を行うクラス
	/// </summary>
	public class NCMBUTCustomQuerySettings
	{
		private GUIContent querySettingsContent = new GUIContent("Query Settings", "検索条件の設定を行います");
		private GUIContent sortContent = new GUIContent("Sort", "ソート条件の設定を行います");
		private GUIContent sortFieldContent = new GUIContent("Sort Field", "ソート対象のフィールドを設定を行います");
		private GUIContent skipContent = new GUIContent("Skip", "取得開始位置の設定を行います");
		private GUIContent limitContent = new GUIContent("Limit", "取得件数の設定を行います");

		private NCMBUTCustomEditorBase customEditor;
		private NCMBUTConnectionBase connection;
		private int currentIndex = 0;

		/// <summary>
		/// コンストラクタ <see cref="NCMBUT.EditorTools.NCMBUTCustomQuerySettings"/>
		/// </summary>
		/// <param name="custom">エディタルートクラス</param>
		public NCMBUTCustomQuerySettings(NCMBUTCustomEditorBase custom)
		{
			customEditor = custom;
			connection = custom.target as NCMBUTConnectionBase;
		}

		/// <summary>
		/// クエリ設定の描画	
		/// </summary>
		public void DrawQuerySettings()
		{
			connection.IsOpenQuery = EditorGUILayout.Foldout(connection.IsOpenQuery, querySettingsContent);

			++EditorGUI.indentLevel;

			if (connection.IsOpenQuery)
			{
				// 降順/昇順の設定
				EditorGUILayout.PropertyField(customEditor.serializedObject.FindProperty("Sort"), sortContent);

				if (connection.Sort != NCMBUTSortType.None)
				{
					// ソート対象のFieldの設定
					// デフォルトのFieldと追加したFieldから設定する
					string[] defaultList = connection.GetDefaultFields();
					string[] addList = new string[connection.FieldDataList.Count];
					for (int i = 0; i < connection.FieldDataList.Count; ++i)
					{
						addList[i] = connection.FieldDataList[i].Key;
					}
					string[] list = new string[defaultList.Length + addList.Length];
					defaultList.CopyTo(list, 0);
					addList.CopyTo(list, defaultList.Length);

					// 選択中のソート対象
					// ない場合はcreateDate
					int tmpIndex = Array.IndexOf(list, connection.SortField);
					if (tmpIndex > -1)
					{
						currentIndex = tmpIndex;
					}
					else
					{
						currentIndex = Array.IndexOf(list, "createDate");
						connection.SortField = list[currentIndex];

						connection.Sort = NCMBUTSortType.None;
						customEditor.serializedObject.ApplyModifiedProperties();
					}

					GUIContent[] contents = new GUIContent[list.Length];

					for (int i = 0; i < list.Length; ++i)
					{
						contents[i] = new GUIContent(list[i]);
					}

					currentIndex = (int)EditorGUILayout.Popup(sortFieldContent, currentIndex, contents);

					if (GUI.changed)
					{
						Undo.RecordObject(connection, "Change Sort Field");
						connection.SortField = list[currentIndex];
					}
				}

				EditorGUILayout.PropertyField(customEditor.serializedObject.FindProperty("Skip"), skipContent);
				EditorGUILayout.PropertyField(customEditor.serializedObject.FindProperty("Limit"), limitContent);
			}

			--EditorGUI.indentLevel;
		}
	}
}