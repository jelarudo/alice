using UnityEngine;
using System.Collections;

public class PageCreate : MonoBehaviour {

    public GameObject _cube;
    public GameObject _countObj;
    public GameObject _rotateAxis;

    public GameObject _createPosition;

    public int width = 8;
    public int height = 8;
    public float cubeScale;

    private int count = 0;
    private int random;


	// Use this for initialization
	void Start () {
        Create();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

   public void Create()
    {
        count = 0;
        int beforeRandom = random;
        while( random == beforeRandom )
        {
            random = Random.Range(0, 36 + 1);
        
        }
        _cube.transform.localScale = new Vector3(1.0f, 1.0f / cubeScale, 1.0f) * cubeScale;

        Vector3 position = _createPosition.transform.position;
        GameObject parentObj = Instantiate(_cube, position, _cube.transform.rotation) as GameObject;
        GameObject rotateAxisObj = Instantiate(_rotateAxis, position, _cube.transform.rotation) as GameObject;
        parentObj.transform.localScale = Vector3.one * 0.1f;
        parentObj.transform.parent = rotateAxisObj.transform;
        

        for (int i = 0; i < height; ++i)
        {
            for (int j = 0; j < width; ++j)
            {
                count++;
                if( i == 0 || i >= height -1 || j == 0 || j >= width -1)
                {
                    count--;
                    position = _createPosition.transform.position + new Vector3(-i, -0.5f, -j) * cubeScale;
                    GameObject childObj = Instantiate(_cube, position, _cube.transform.rotation) as GameObject;
                    SpriteSheet sprite = childObj.GetComponent<SpriteSheet>();
                    sprite.number = (16 * i) + (8 + j);

                    childObj.transform.parent = parentObj.transform;
                }
                else if (count != random )
                {
                    position = _createPosition.transform.position + new Vector3(-i, -0.5f, -j) * cubeScale;
                    GameObject childObj = Instantiate(_cube, position, _cube.transform.rotation) as GameObject;
                    SpriteSheet sprite = childObj.GetComponent<SpriteSheet>();
                    sprite.number = (16 * i) + (8 + j);

                    childObj.transform.parent = parentObj.transform;
                }
                else
                {
                    position = _createPosition.transform.position + new Vector3(-i, -0.5f, -j) * cubeScale;
                    position += Vector3.down;
                    GameObject countObj = Instantiate(_countObj, position, _cube.transform.rotation) as GameObject;

                    countObj.transform.parent = parentObj.transform;
                }
            }
        }
        
        _createPosition.transform.position += new Vector3(0.0f, 1.0f, 0.0f);
	
    }
}
