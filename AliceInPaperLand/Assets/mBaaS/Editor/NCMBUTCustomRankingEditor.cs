/// <summary>
/// ランキングのInspector拡張クラス
/// </summary>
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text;
using System.IO;
using System.Reflection;

namespace NCMBUT.EditorTools
{
	/// <summary>
	/// NCMBUT custom ranking editor.
	/// </summary>
	[CustomEditor(typeof(NCMBUTRankingConnection))]
	public sealed class NCMBUTCustomRankingEditor:NCMBUTCustomEditorBase
	{
		private GUIContent rankingSettingsContent = new GUIContent("Ranking Settings", "ncmbut_rankingsクラスに関する設定を行います");
		private GUIContent stageContent = new GUIContent("Stage", "ステージ数の設定を行います");
		private GUIContent forceUpdateContent = new GUIContent("ForceUpdate", "強制更新の設定を行います");

		/// <summary>
		/// InspectorにGUI表示
		/// </summary>
		public override void OnInspectorGUI()
		{
			// 更新
			serializedObject.Update();

			EditorGUILayout.LabelField(rankingSettingsContent, EditorStyles.boldLabel);

			EditorGUILayout.PropertyField(serializedObject.FindProperty("Stage"), stageContent);

			EditorGUILayout.PropertyField(serializedObject.FindProperty("ForceUpdate"), forceUpdateContent);

			NCMBUTRankingConnection conn = target as NCMBUTRankingConnection;
			if (conn.ForceUpdate)
			{
				EditorGUILayout.HelpBox(NCMBUTEditorErrorMessage.FORCE_UPDATE, MessageType.Info);
			}

			// 保存

			if (GUI.changed)
			{
				EditorUtility.SetDirty(target);
				serializedObject.ApplyModifiedProperties();
			}

			// オプション表示
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
				TemplateBuilder.CreateRanking();
			}
		}

		/// <summary>
		/// テンプレートをHierarchyに作成
		/// </summary>
		public static void SetTemplate()
		{
			NCMBUTRankingConnection connection = (NCMBUTRankingConnection)GameObject.FindObjectOfType(typeof(NCMBUTRankingConnection));
			if (connection == null)
			{
				GameObject prefab = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/" + typeof(NCMBUTRankingConnection).Name, typeof(GameObject)));
				prefab.name = typeof(NCMBUTRankingConnection).Name;
				connection = prefab.GetComponent<NCMBUTRankingConnection>();

				Undo.RegisterCreatedObjectUndo(prefab, "Create Prefab Object");
			}

			NCMBUTRankingTemplate template = (NCMBUTRankingTemplate)GameObject.FindObjectOfType(typeof(NCMBUTRankingTemplate));
			if (template == null)
			{
				GameObject go = new GameObject();
				go.name = typeof(NCMBUTRankingTemplate).Name;
				template = go.AddComponent<NCMBUTRankingTemplate>();
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
			NCMBUTRankingConnection connection = (NCMBUTRankingConnection)GameObject.FindObjectOfType(typeof(NCMBUTRankingConnection));
			NCMBUTRankingTemplate template = (NCMBUTRankingTemplate)GameObject.FindObjectOfType(typeof(NCMBUTRankingTemplate));
			return (connection == null || template == null);
		}
	}
}