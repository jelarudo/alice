using UnityEngine;
using System.Collections;

public class TurnPageFinishCheck : MonoBehaviour {

    public TurnPage turnpage;
    public PageCreate create;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (turnpage == null)
        {
            GameObject[] turnpages = GameObject.FindGameObjectsWithTag("TurnPage");
            foreach (GameObject turn in turnpages)
            {
                turnpage = turn.GetComponent<TurnPage>();
                if( turnpage != null )
                {
                    break;
                }
            }
            if( turnpages.Length >= 4 )
            {
                int num = 0;
                float min = turnpages[num].transform.position.y;
                
                for (int i = 1; i < turnpages.Length; ++i)
                {
                    if( min > turnpages[i].transform.position.y)
                    {
                        num = i;
                        min = turnpages[i].transform.position.y;
                    }
                }
                Destroy( turnpages[num] );
            }
        }

	    else if( turnpage.IsEnd() )
        {
            create.Create();
            Destroy( turnpage );
        }
	}
}
