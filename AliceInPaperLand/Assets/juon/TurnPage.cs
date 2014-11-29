using UnityEngine;
using System.Collections;

public class TurnPage : MonoBehaviour
{
		public int secs2Wait = 300;
        public float speed = 10.0f;
		public float roundTime = 0;
		// Use this for initialization
		void Start ()
		{
		}

		// Update is called once per frame
		void Update ()
		{
				if (Input.GetMouseButtonDown (0)) {
					
				}
				if (!IsEnd()) {
						transform.RotateAround (this.transform.position, Vector3.right, speed * Time.deltaTime);
						roundTime += speed * Time.deltaTime;
				}

		}
        public bool IsEnd()
        {
            return roundTime > 180;
        }
        
}
