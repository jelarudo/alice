/// <summary>
/// Inspector拡張のルートクラス
/// </summary>
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System.Text;
using System.IO;

namespace NCMBUT.EditorTools
{
	/// <summary>
	/// カスタムエディタのスーパクラス
	/// </summary>
	[CustomEditor(typeof(NCMBUTConnectionBase))]
	public abstract class NCMBUTCustomEditorBase:Editor
	{
		private GUIContent optionsContent = new GUIContent("Options", "設定の追加を行います");

		protected GUIContent templateSettingsContent = new GUIContent("Template Settings", "テンプレートクラスの設定を行います。新規で通信の設定を行う場合は、こちらの使用を推奨します");
		protected GUIContent setDefaultTemplateContent = new GUIContent("Set Default Template", "シーンにデフォルトのテンプレートの追加を行います");
		protected GUIContent createCustomTemplateContent = new GUIContent("Create Custom Template", "カスタムテンプレートの作成を行います");

		private NCMBUTCustomAclSettings aclSettings;
		private NCMBUTCustomQuerySettings querySettings;
		private NCMBUTCustomFieldSettings fieldSettings;

		/// <summary>
		/// リストの初期化
		/// </summary>
		private void OnEnable()
		{
			if (fieldSettings != null)
			{
				fieldSettings.OnEnable();
			}
			if (aclSettings != null)
			{
				aclSettings.OnEnable();
			}
		}

		/// <summary>
		/// 設定を全て描画する
		/// </summary>
		protected void DrawAllSettings()
		{
			NCMBUTConnectionBase connection = target as NCMBUTConnectionBase;

			serializedObject.Update();

			connection.IsOpenOptions = EditorGUILayout.Foldout(connection.IsOpenOptions, optionsContent);

			++EditorGUI.indentLevel;
			if (connection.IsOpenOptions)
			{
				if (connection.GetUseQuerySettings())
				{
					DrawQuerySettings();
				}

				EditorGUILayout.Space();

				if (connection.GetUseFieldSettings())
				{
					DrawFieldSettings();
				}

				EditorGUILayout.Space();

				if (connection.GetUseAclSettings())
				{
					DrawAclSettings();
				}
			}
			--EditorGUI.indentLevel;

			if (GUI.changed)
			{
				serializedObject.ApplyModifiedProperties();
			}
		}

		/// <summary>
		/// クエリ設定を描画する
		/// </summary>
		protected virtual void DrawQuerySettings()
		{
			if (querySettings == null)
			{
				querySettings = new NCMBUTCustomQuerySettings(this);
			}

			querySettings.DrawQuerySettings();
		}

		/// <summary>
		/// Field設定を描画する
		/// </summary>
		protected virtual void DrawFieldSettings()
		{
			if (fieldSettings == null)
			{
				fieldSettings = new NCMBUTCustomFieldSettings(this);
			}
			
			fieldSettings.DrawFieldSettings();
		}

		/// <summary>
		/// ACL設定を描画する	
		/// </summary>
		protected virtual void DrawAclSettings()
		{
			if (aclSettings == null)
			{
				aclSettings = new NCMBUTCustomAclSettings(this);
			}
			
			aclSettings.DrawAclSettings();
		}
	}
}