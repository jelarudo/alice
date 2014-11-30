/// <summary>
/// フレンドのInspector拡張
/// </summary>
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text;
using System.IO;

namespace NCMBUT.EditorTools
{
	/// <summary>
	/// NCMBUT custom friend editor.
	/// </summary>
	[CustomEditor(typeof(NCMBUTFriendConnection))]
	public sealed class NCMBUTCustomFriendEditor:NCMBUTCustomEditorBase
	{
		private GUIContent messageSettingsContent = new GUIContent("Message Settings", "ncmbut_messageクラスに関する設定を行います");

		/// <summary>
		/// Inspectorの表示
		/// </summary>
		public override void OnInspectorGUI()
		{
			EditorGUILayout.LabelField(messageSettingsContent, EditorStyles.boldLabel);

			base.DrawAllSettings();

			EditorGUILayout.Space();

			EditorGUILayout.LabelField(templateSettingsContent, EditorStyles.boldLabel);

			EditorGUI.BeginDisabledGroup(!IsSetTemplate());
			if (GUILayout.Button(setDefaultTemplateContent))
			{
				SetTemplate();
			}
			EditorGUI.EndDisabledGroup();

			if (GUILayout.Button(createCustomTemplateContent))
			{
				TemplateBuilder.CreateFriend();
			}
		}

		/// <summary>
		/// テンプレートをHierarchyに作成
		/// </summary>
		public static void SetTemplate()
		{
			NCMBUTFriendConnection connection = (NCMBUTFriendConnection)GameObject.FindObjectOfType(typeof(NCMBUTFriendConnection));
			if (connection == null)
			{
				GameObject prefab = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/" + typeof(NCMBUTFriendConnection).Name, typeof(GameObject)));
				prefab.name = typeof(NCMBUTFriendConnection).Name;
				connection = prefab.GetComponent<NCMBUTFriendConnection>();

				Undo.RegisterCreatedObjectUndo(prefab, "Create Prefab Object");
			}

			NCMBUTFriendTemplate template = (NCMBUTFriendTemplate)GameObject.FindObjectOfType(typeof(NCMBUTFriendTemplate));
			if (template == null)
			{
				GameObject go = new GameObject();
				go.name = typeof(NCMBUTFriendTemplate).Name;
				template =  go.AddComponent<NCMBUTFriendTemplate>();
				template.Connection = connection;

				Undo.RegisterCreatedObjectUndo(go, "Create Template Object");
			}
		}

		/// <summary>
		/// テンプレート配置のアクティブ条件
		/// </summary>
		/// <returns>ConnectionとTemplateの片方でも無い場合は<c>true</c>、両方無い場合は<c>false</c></returns>
		public static bool IsSetTemplate()
		{
			NCMBUTFriendConnection connection = (NCMBUTFriendConnection)GameObject.FindObjectOfType(typeof(NCMBUTFriendConnection));
			NCMBUTFriendTemplate template = (NCMBUTFriendTemplate)GameObject.FindObjectOfType(typeof(NCMBUTFriendTemplate));
			return (connection == null || template == null);
		}
	}
}