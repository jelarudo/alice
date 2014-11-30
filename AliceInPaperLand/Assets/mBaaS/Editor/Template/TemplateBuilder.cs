using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace NCMBUT.EditorTools
{
	/// <summary>
	/// テキストからテンプレートクラスの作成を行う
	/// </summary>
	public class TemplateBuilder:EditorWindow
	{
		private static GUIContent templateLabelContent = new GUIContent("", "カスタムテンプレートの作成を行います。使用するメソッドにチェックをつけて保存して下さい");

		private static readonly string TEMPLATE_PASS = "Assets/mBaaS/Editor/Template/";
		private static readonly string REGION = "#region ";
		private static readonly string METHODS = " Methods";

		private static string currentTemplate;
		private static List<string> methodList = new List<string>();
		private static List<bool> useList = new List<bool>();

		/// <summary>
		/// ランキングのテンプレート作成	
		/// </summary>
		public static void CreateRanking()
		{
			init("RankingCustomTemplate");
		}

		/// <summary>
		/// 会員管理のテンプレート作成
		/// </summary>
		public static void CreateUser()
		{
			init("UserCustomTemplate");
		}

		/// <summary>
		/// フレンドのテンプレート作成
		/// </summary>
		public static void CreateFriend()
		{
			init("FriendCustomTemplate");
		}

		/// <summary>
		/// エディタ初期化
		/// </summary>
		/// <param name="templateName">テンプレートファイル名</param>
		private static void init(string templateName)
		{
			templateLabelContent.text = templateName;
			currentTemplate = templateName;

			// テンプレートのテキスト取得
			string file = File.ReadAllText(TEMPLATE_PASS + currentTemplate + ".txt");

			string[] lines = file.Split('\n');

			// リストの初期化
			methodList.Clear();
			useList.Clear();

			// テンプレートのメソッド取得
			// regionで呼び出しメソッドとdelegateがくくってある
			for (int i = 0; i < lines.Length; ++i)
			{
				if (lines[i].IndexOf(REGION) > -1)
				{
					methodList.Add(lines[i].Replace(REGION, "").Replace(METHODS, ""));
					useList.Add(true);
				}
			}

			GetWindow<TemplateBuilder>().Close();

			TemplateBuilder window = CreateInstance<TemplateBuilder>();
			window.title = "Custom Template";
			window.Show();
		}

		/// <summary>
		/// GUIの表示
		/// </summary>
		void OnGUI()
		{
			EditorGUILayout.LabelField(templateLabelContent, EditorStyles.boldLabel);

			EditorGUILayout.Space();

			for (int i = 0; i < methodList.Count; ++i)
			{
				useList[i] = EditorGUILayout.ToggleLeft(methodList[i], useList[i]);
			}

			EditorGUILayout.Space();

			// 1つ以上メソッドが選択されていればボタンを有効にする
			EditorGUI.BeginDisabledGroup(useList.Where(o => o == true).Count() == 0);
			if (GUILayout.Button("Save"))
			{
				SaveTemplate();
			}
			EditorGUI.EndDisabledGroup();
		}

		/// <summary>
		/// カスタムテンプレートの保存	
		/// </summary>
		private void SaveTemplate()
		{
			// 保存先を取得
			string savePath = EditorUtility.SaveFilePanel("Save Template", "Assets/", currentTemplate + ".cs", "cs");
			if (savePath.Length != 0)
			{
				string baseName = GetBaseName(savePath);
			
				StringBuilder builder = new StringBuilder();
			
				// テンプレートのテキストを取得し、1行ずつにする
				string file = File.ReadAllText(TEMPLATE_PASS + currentTemplate + ".txt");
				string[] lines = file.Split('\n');

				bool useFlag = true;
				int methodCount = 0;
				string oldLine = "";

				// チェックの付いているメソッドだけ追加する
				for (int i = 0; i < lines.Length; ++i)
				{
					if (lines[i].IndexOf(REGION) > -1)
					{
						if (lines[i].Replace(REGION, "").Replace(METHODS, "") == methodList[methodCount])
						{
							useFlag = useList[methodCount];
							++methodCount;
						}
					}
					else if (!useFlag && lines[i].IndexOf("#endregion") > -1)
					{
						useFlag = true;
						continue;
					}

					if (!useFlag)
					{
						continue;
					}

					if (lines[i] != oldLine)
					{
						builder.AppendLine(lines[i]);
						oldLine = lines[i];
					}
				}

				// クラス名を置換して保存
				File.WriteAllText(savePath, builder.ToString().Replace("#CLASS_NAME#", baseName), Encoding.UTF8);

				// アセットのリフレッシュ
				AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
			}
		}

		/// <summary>
		/// ファイル名の取得(拡張子除く)
		/// </summary>
		/// <returns>ファイル名</returns>
		/// <param name="path">保存パス</param>
		private string GetBaseName(string path)
		{
			string baseName = "";
			
			// /で分割
			string[] pathList = path.Split('/');
			// .で分割して、ファイル名取得
			baseName = pathList[pathList.Length - 1].Split('.')[0];
		
			return baseName;
		}
	}
}