/// <summary>
/// 会員管理クラスのInspector拡張		
/// </summary>
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text;
using System.IO;

namespace NCMBUT.EditorTools
{
	/// <summary>
	/// 会員管理のエディタ
	/// </summary>
	[CustomEditor(typeof(NCMBUTUserConnection))]
	public sealed class NCMBUTCustomUserEditor:NCMBUTCustomEditorBase
	{
		private GUIContent userSettingsContent = new GUIContent("User Settings", "会員管理に関する設定を行います");
		private GUIContent idValidationContent = new GUIContent("User Name Validation", "User Nameの入力チェックを行います");
		private GUIContent minUserNameContent = new GUIContent("Min User Name", "User Nameの最小文字数の設定を行います");
		private GUIContent maxUserNameContent = new GUIContent("Max User Name", "User Nameの最大文字数の設定を行います");
		private GUIContent passValidationContent = new GUIContent("Password Validation", "Passwordの入力チェックを行います");
		private GUIContent minPasswordContent = new GUIContent("Min Passwoed", "Passwordの最小文字数の設定を行います");
		private GUIContent maxPasswordContent = new GUIContent("Max Password", "Passwordの最大文字数の設定を行います");

		/// <summary>
		/// Inspectorの表示
		/// </summary>
		public override void OnInspectorGUI()
		{
			// 更新
			serializedObject.Update();
			NCMBUTUserConnection conn = target as NCMBUTUserConnection;

			EditorGUILayout.LabelField(userSettingsContent, EditorStyles.boldLabel);

			EditorGUILayout.PropertyField(serializedObject.FindProperty("IsUserNameValidation"), idValidationContent);

			// UserName入力の文字数制限
			if (conn.IsUserNameValidation)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("MinUserName"), minUserNameContent);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxUserName"), maxUserNameContent);
			}

			EditorGUILayout.PropertyField(serializedObject.FindProperty("IsUsePasswordValidation"), passValidationContent);

			// Passwordの文字数制限	
			if (conn.IsUsePasswordValidation)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("MinPassword"), minPasswordContent);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("MaxPassword"), maxPasswordContent);
			}

			// 保存
			if (GUI.changed)
			{
				EditorUtility.SetDirty(target);
				serializedObject.ApplyModifiedProperties();
			}

			// オプションの表示
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
				TemplateBuilder.CreateUser();
			}
		}

		/// <summary>
		/// テンプレートをHierarchyに作成
		/// </summary>
		public static void SetTemplate()
		{
			NCMBUTUserConnection connection = (NCMBUTUserConnection)GameObject.FindObjectOfType(typeof(NCMBUTUserConnection));
			if (connection == null)
			{
				GameObject prefab = (GameObject)PrefabUtility.InstantiatePrefab(Resources.Load("Prefabs/" + typeof(NCMBUTUserConnection).Name, typeof(GameObject)));
				prefab.name = typeof(NCMBUTUserConnection).Name;
				connection = prefab.GetComponent<NCMBUTUserConnection>();

				Undo.RegisterCreatedObjectUndo(prefab, "Create Prefab Object");
			}

			NCMBUTUserTemplate template = (NCMBUTUserTemplate)GameObject.FindObjectOfType(typeof(NCMBUTUserTemplate));
			if (template == null)
			{
				GameObject go = new GameObject();
				go.name = typeof(NCMBUTUserTemplate).Name;
				template = go.AddComponent<NCMBUTUserTemplate>();
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
			NCMBUTUserConnection connection = (NCMBUTUserConnection)GameObject.FindObjectOfType(typeof(NCMBUTUserConnection));
			NCMBUTUserTemplate template = (NCMBUTUserTemplate)GameObject.FindObjectOfType(typeof(NCMBUTUserTemplate));
			return (connection == null || template == null);
		}
	}
}