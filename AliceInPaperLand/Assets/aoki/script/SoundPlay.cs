using UnityEngine;
using System.Collections;

public class SoundPlay : MonoBehaviour {

    public int bgm = 0;
    public float bgmVolume = 1.0f;

	// Use this for initialization
	void Start () {
        Screen.showCursor = false;
        if( SoundManager.Instance == null )
        {
            return;
        }
        SoundManager.Instance.PlayBGM( bgm );
        SoundManager.Instance.volume.BGM = bgmVolume;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
