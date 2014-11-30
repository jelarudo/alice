using UnityEngine;
using System.Collections;

/// <summary>
/// NCMBUT home.
/// </summary>
public class NCMBUTHome:MonoBehaviour
{
	private float margin = 10.0f;

	/// <summary>
	/// Raises the GU event.
	/// </summary>
	void OnGUI()
	{
		guiSettings();

		float contentsWidth = Screen.width - margin * 2.0f;
		
		Rect titleRect = new Rect(margin, 0, contentsWidth, Screen.height * 0.1f);
		drawTitle(titleRect);

		Rect menuRect = new Rect(margin + contentsWidth * 0.25f, titleRect.y + titleRect.height, contentsWidth * 0.5f, Screen.height * 0.9f);
		drawMenu(menuRect);
	}

	private void guiSettings()
	{
		GUI.skin.label.fontSize = 20;
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.skin.textField.fontSize = 20;
	}

	/// <summary>
	/// Draws the title.
	/// </summary>
	/// <param name="rect">Rect.</param>
	private void drawTitle(Rect rect)
	{
		GUILayout.BeginArea(rect);
		GUILayout.FlexibleSpace();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label("Home", GUILayout.MinWidth(65));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
	}

	/// <summary>
	/// Draws the menu.
	/// </summary>
	/// <param name="rect">Rect.</param>
	private void drawMenu(Rect rect)
	{
		GUILayout.BeginArea(rect);
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("User Sample", GUILayout.MinHeight(50)))
		{
			Application.LoadLevel("NCMBUTUserSample");
		}
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Ranking Sample", GUILayout.MinHeight(50)))
		{
			Application.LoadLevel("NCMBUTRankingSample");
		}
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Friend Sample", GUILayout.MinHeight(50)))
		{
			Application.LoadLevel("NCMBUTFriendSample");
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndArea();
	}
}