/// <summary>
/// 追加のField設定を描画するクラス
/// </summary>
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NCMBUT.EditorTools
{
	/// <summary>
	/// フィールド設定を行うクラス
	/// </summary>
	public class NCMBUTCustomFieldSettings
	{
		private GUIContent fieldSettingsContent = new GUIContent("Field Settings", "フィールドの設定を行います。追加でフィールドを設定したい場合は、こちらからフィールドを追加し、SetValueで値をセットします");
		private GUIContent requireContent = new GUIContent("Require", "値の入力を必須にするかの設定を行います");
		private GUIContent fieldNameContent = new GUIContent("Field Name", "フィールド名の設定を行います");
		private GUIContent typeContent = new GUIContent("Type", "フィールドのデータ型の設定を行います");

		private NCMBUTCustomEditorBase customEditor;
		private NCMBUTConnectionBase connection;

		/// <summary>
		/// コンストラクタ<see cref="NCMBUT.EditorTools.NCMBUTCustomFieldSettings"/>
		/// </summary>
		/// <param name="custom">エディタルート</param>
		public NCMBUTCustomFieldSettings(NCMBUTCustomEditorBase custom)
		{
			customEditor = custom;
			connection = custom.target as NCMBUTConnectionBase;
		}

		private NCMBUTInputError inputError = NCMBUTInputError.None;
		private NCMBUTCustomDataInputError dataInputError = new NCMBUTCustomDataInputError();
	
		private ReorderableList list;

		/// <summary>
		/// リストの初期化。エディタルートから呼ぶ
		/// </summary>
		public void OnEnable()
		{
			// 初期化
			list = new ReorderableList(customEditor.serializedObject, customEditor.serializedObject.FindProperty("FieldDataList"), true, true, true, true);

			// ヘッダー表示
			list.drawHeaderCallback = (Rect rect) => {
				EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), requireContent);
				EditorGUI.LabelField(new Rect(rect.x + rect.width * 0.5f - 60, rect.y, 92, EditorGUIUtility.singleLineHeight), fieldNameContent);
				EditorGUI.LabelField(new Rect(rect.x + rect.width - 87, rect.y, 110, EditorGUIUtility.singleLineHeight), typeContent);
			};

			// エレメント一覧
			list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
				if (index >= connection.FieldDataList.Count) return;

				SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
				rect.y += 2;

				EditorGUI.PropertyField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("IsRequire"), GUIContent.none);
				EditorGUI.PropertyField(new Rect(rect.x + 50, rect.y, rect.width - 140, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("Key"), GUIContent.none);
				EditorGUI.PropertyField(new Rect(rect.x + rect.width - 110, rect.y, 110, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("Type"), GUIContent.none);
			};
		}

		/// <summary>
		/// Field設定の描画
		/// </summary>
		public void DrawFieldSettings()
		{
			connection.IsOpenField = EditorGUILayout.Foldout(connection.IsOpenField, fieldSettingsContent);

			++EditorGUI.indentLevel;

			if (connection.IsOpenField)
			{
				if (list == null)
				{
					OnEnable();
				}
				list.DoLayoutList();
			}

			--EditorGUI.indentLevel;

			inputError = dataInputError.CheckKey(connection.FieldDataList, connection.GetDefaultFields());
			dataInputError.DrawInputError(inputError);
		}
	}
}