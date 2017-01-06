﻿using UnityEngine;
using System.Collections;

public class Limb : ClimberClass {

	public GameObject oldhold;
	public bool reaching;
	public GameObject climber;




	// Use this for initialization
	void Start () {

		reaching = false;

	}


	// Update is called once per frame
	void Update () {
		
		if ((reaching == true) && (climber.GetComponent<ClimberClass>().selectlimb == gameObject)) {
			
				Approximate ();

		}

	}

	void OnMouseDown(){
		climber.GetComponent<ClimberClass> ().selectlimb = gameObject;
	}


	void OnCollisionStay2D(Collision2D col){

		if ((col.gameObject.layer == 8) && reaching) {

			if (col.gameObject != oldhold) {
				oldhold = col.gameObject;
				reaching = false;
				Debug.Log ("Got a new hold!");
			}
		}
	}


	public void Reach(Camera camera){
		
		target = Input.mousePosition;
		GetComponent<DistanceJoint2D> ().connectedAnchor = camera.ScreenToWorldPoint (target);
		reaching = true;
		Debug.Log ("Reaching for a hold");

	}

	public void Approximate (){
		if (GetComponent<DistanceJoint2D> ().distance > 0.05f) {
			GetComponent<DistanceJoint2D> ().distance -= 0.01f;
			Debug.Log ("Pulling closer");
		}
	 	else {
			Debug.Log ("Didn't manage to grab a hold");
			reaching = false;
			GetComponent<DistanceJoint2D> ().distance = 0.1f;
			GetComponent<DistanceJoint2D> ().connectedAnchor = oldhold.transform.position;
			}

	}





}