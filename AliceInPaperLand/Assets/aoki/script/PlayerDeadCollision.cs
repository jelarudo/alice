using UnityEngine;
using System.Collections;

public class PlayerDeadCollision : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				//Debug.Log("aa");
		}

		void OnTriggerEnter (Collider col)
		{
				Debug.Log ("atta");
				if (col.transform.tag == "Cube") {
            
						this.transform.parent.transform.position += new Vector3 (0.0f, -1.0f, 0.0f) * 3.0f;
						SoundManager.Instance.PlayVoice (Random.Range (24, 25 + 1));
				}
        
				//SCORE SAVE
				//Score scoreObj = GameObject.Find ("Score").GetComponent<Score> ();
				//Destroy (scoreObj);
				PlayerPrefs.SetInt ("score", Score.score);
				PlayerPrefs.SetInt ("highScore", Score.highScore);
				Debug.Log ("Score end PREF :" + PlayerPrefs.GetInt ("score", 0));
				Debug.Log ("HighScore end PREF :" + PlayerPrefs.GetInt ("highScore", 0));
		}


}
