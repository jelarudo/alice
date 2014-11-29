﻿using UnityEngine;
using System.Collections;

public class PlayerDeadState : MonoBehaviour {

    public RuntimeAnimatorController animeControler;
    public Animator animator;
    FadeOVR fade;

	// Use this for initialization
	void Start () {
        fade = this.GetComponentInChildren<FadeOVR>();
	}
	
	// Update is called once per frame
	void Update () {
        if( fade.IsEnd() && animator.runtimeAnimatorController == animeControler )
        {
            Application.LoadLevel( "result" );
        }
	
	}
    void OnCollisionEnter(Collision collision)
    {
        if( collision.transform.name == "AliveCube" )
        {
            Destroy( this.GetComponent<UnityChan.UnityChanControlScriptWithRgidBody>() );
            GameObject.Find("CamPos").transform.position = this.transform.position + Vector3.up * 3.0f;
            GameObject.Find("CamPos").transform.rotation = Quaternion.LookRotation( Vector3.down );
            animator.runtimeAnimatorController = animeControler;
            fade.FadeOut();

        }
    }



}