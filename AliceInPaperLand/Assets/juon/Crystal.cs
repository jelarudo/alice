using UnityEngine;
using System.Collections;

public class Crystal: MonoBehaviour
{
	
		// Use this for initialization
		void Start ()
		{
		
		}
	
		// Update is called once per frame
		void Update ()
		{
				transform.Rotate (0, 1, 0);
		}
	
		void OnTriggerEnter (Collider col)
		{
				if (col.tag == "Player") {
						Score.score += Score.CRYSTALSCOREPOINT;
						Destroy (gameObject);
			
				} else {
						Destroy (this.gameObject);
				}
		
		}
}
