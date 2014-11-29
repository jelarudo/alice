using UnityEngine;
using System.Collections;

public class SceneChange : MonoBehaviour {
	
	public float WaitTime = 0;
	public string SceneName;
	


	public KeyCode key;
    
    public enum SCENENAME
    {
        Title,
        GamePlay,
        HowToPlay,
        GameClear,
        GameOver,
        Continue,
        Credit,
        Result,
        MAX,
    }
    public SCENENAME test;
	public bool SceneChangeFlag = false;


    private Texture2D texture;
    private Color nowColor;

    public float timer;

    protected static SceneChange instance;
    public static SceneChange Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (SceneChange)FindObjectOfType(typeof(SceneChange));
                if (instance == null)
                {
                    Debug.LogError("SceneChange Instance Error");
                }
            }
            return instance;
        }
    }     

	// Use this for initialization
	void Start () {
        timer = WaitTime;
        texture = new Texture2D( 1, 1 );
        texture.SetPixel( 0, 0, Color.black );
        texture.Apply();

     
	}
	
	// Update is called once per frame
	void Update () {

        nowColor = new Color( 0.0f, 0.0f, 0.0f, Mathf.Lerp( 0.0f, 1.0f, timer / WaitTime ) );

        timer -= Time.deltaTime;
        timer = Mathf.Max(timer, 0.0f);
        if (SceneName.Length == 0)
        {
            return;
        }
		if( !SceneChangeFlag )
		{
            
			if( key == KeyCode.None )
			{
				//key = KeyCode.P;
				return;
			}
			if( Input.GetKeyDown( key ) )
			{
				SceneChangeFlag = true;
			}
            
            

			return;
		}


        timer += Time.deltaTime * 2.0f;
        timer = Mathf.Min(timer, WaitTime);
        StartCoroutine(Wait(WaitTime));
	}
	
	IEnumerator Wait( float time )
	{
		Debug.Log("");
        
		yield return new WaitForSeconds( time );
        SceneChangeFlag = false;
        
		Application.LoadLevel(SceneName);

        //SceneChange.Instance.Play(SceneName);
	}


    public void Play(string s)
    {
        SceneName = s;
       
        Play();

    }
    public void Play()
    {
        SceneChangeFlag = true;

    }
    public void Play(string s, float time)
    {
        SceneName = s;
        this.WaitTime = time;
        Play();

    }

    void OnGUI()
    {
        GUI.color = nowColor;
        GUI.DrawTexture( new Rect( 0, 0, Screen.width, Screen.height ), texture );
    }
}
