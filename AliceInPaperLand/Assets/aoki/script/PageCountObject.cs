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

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Player")
        {
            count.page += 1;
            Destroy( gameObject );
            SoundManager.Instance.PlayVoice( Random.Range( 17, 21 + 1 ) );
        }
    }
}
