using UnityEngine;
using System.Collections;

public class PageCountObject : MonoBehaviour {

    public PageCount count;

	// Use this for initialization
	void Start () {
        count = GameObject.FindGameObjectWithTag("Score").GetComponent<PageCount>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            count.page += 1;
            Destroy( gameObject );
        }
    }
}
