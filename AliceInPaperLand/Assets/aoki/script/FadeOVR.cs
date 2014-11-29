using UnityEngine;
using System.Collections;

public class FadeOVR : VRGUI {

    private Color NowColor;

    public float Timer = 0.0f;

    public float WaitTimer = 10.0f;

    private Texture2D Texture;

    bool isFadeOut;
    bool isFadeIn = true;

    // Use this for initialization
    void Start()
    {
        // スタート時明るくする
        FadeIn();
        //Timer = WaitTimer;
        Texture = new Texture2D(1, 1);
        Texture.SetPixel(0, 0, Color.black);
        Texture.Apply();
    }

    // Update is called once per frame
    void Update()
    {
        NowColor = new Color(0.0f, 0.0f, 0.0f, Mathf.Lerp(1.0f, 0.0f, Timer / WaitTimer));
        if( isFadeIn )
        {
            Timer += Time.deltaTime * 1.0f;
        }
        else if( isFadeOut )
        {
            Timer -= Time.deltaTime * 1.0f;
        }
        Timer = Mathf.Clamp( Timer, 0.0f, WaitTimer );
    }

    
    /*void OnGUI()
    {
        GUI.color = NowColor;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture);
    }
    */
    public override void OnVRGUI()
    {
        GUI.color = NowColor;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture);
    }

    // 明るくなる
    public void FadeIn()
    {
        isFadeIn = true;
        isFadeOut = false;
    }

    // 暗くなる
    public void FadeOut()
    {
        isFadeIn = false;
        isFadeOut = true;
    }

    public bool IsEnd()
    {
        return ( isFadeOut && Timer <= 0 ) ||
               ( isFadeIn  && Timer >= WaitTimer );
    }

}
