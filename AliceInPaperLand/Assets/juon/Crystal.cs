using UnityEngine;
using System.Collections;

public class Crystal: MonoBehaviour
{
    public GameObject particle;
	
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
                        Instantiate( particle, this.transform.GetChild(0).transform.position, particle.transform.rotation  );
                        SoundManager.Instance.PlaySE(3);
			
				} else {
						Destroy (this.gameObject);
				}
		
		}
}
