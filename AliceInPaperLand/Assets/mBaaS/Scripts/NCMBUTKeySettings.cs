using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

/// <summary>
/// APIキーの設定を保持するScriptableObject
/// </summary>
public class NCMBUTKeySettings:ScriptableObject
{
	private static NCMBUTKeySettings instance;

	/// <summary>
	/// インスタンスの取得
	/// </summary>
	/// <value>インスタンス</value>
	private static NCMBUTKeySettings Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Resources.Load("NCMBUTKeySettings") as NCMBUTKeySettings;
				if (instance == null)
				{
					instance = CreateInstance<NCMBUTKeySettings>();

					// UnityEditorの場合にのみ、ScriptableObjectが無い場合は新規で作成する
					#if UNITY_EDITOR
					string filePath = "Assets/mBaaS/Resources/NCMBUTKeySettings.asset";
					AssetDatabase.CreateAsset(instance, filePath);
					#endif
				}
			}
			return instance;
		}
	}
	
#if UNITY_EDITOR
	/// <summary>
	/// Inspectorに、キー設定を表示する
	/// </summary>
	public static void Edit()
	{
		Selection.activeObject = Instance;
	}
#endif
	
	[SerializeField]
	[Tooltip("mBaaSのアプリケーションキー")]
	public string
		ApplicationKey = "YOUR_APPLICATION_KEY";
	[SerializeField]
	[Tooltip("mBaaSのクライアントキー")]
	public string
		ClientKey = "YOUR_CLIENT_KEY";
}