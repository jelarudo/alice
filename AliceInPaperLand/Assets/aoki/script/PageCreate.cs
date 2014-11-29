using UnityEngine;
using System.Collections;

public class PageCreate : MonoBehaviour {

    public GameObject _cube;
    public GameObject _trap;
    public GameObject _rotateAxis;

    public GameObject _createPosition;

    public int width = 8;
    public int height = 8;
    public float cubeScale;

    private int count = 0;
    private int random;


	// Use this for initialization
	void Start () {
        random = Random.Range( 0, 49 + 1 );
        _cube.transform.localScale = new Vector3( 1.0f, 1.0f / cubeScale, 1.0f ) * cubeScale;
        
        Vector3 position = _createPosition.transform.position;
        GameObject parentObj = Instantiate( _cube, position, _cube.transform.rotation ) as GameObject;
        GameObject rotateAxisObj = Instantiate( _rotateAxis, position, _cube.transform.rotation ) as GameObject;
        parentObj.transform.parent = rotateAxisObj.transform;
        for (int i = 0; i < height; ++i )
        {
            for (int j = 0; j < width; ++j )
            {
                if( i != 0 || j != 0 || i != height || j != width )
                {
                    ++count;
                }
                if (count == random)
                {
                    count++;
                }
                else
                {
                    position = _createPosition.transform.position + new Vector3(j, 0, -i) * cubeScale;
                    GameObject childObj = Instantiate(_cube, position, _cube.transform.rotation) as GameObject;
                    childObj.transform.parent = parentObj.transform;
                }
            }
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
