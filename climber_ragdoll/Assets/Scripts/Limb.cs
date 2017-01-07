using UnityEngine;
using System.Collections;

public class Limb : ClimberClass {

	public GameObject oldhold;
	public bool reaching;
	public GameObject climber;
	public GameObject controller;


	// Use this for initialization
	void Start () {

		reaching = false;
		controller = GameObject.Find("controller");

	}


	// Update is called once per frame
	void Update () {

		//if the limb is trying to reach and selected, call the approximate() function
		if ((reaching == true) && (climber.GetComponent<ClimberClass>().selectlimb == gameObject)) {
			
				Approximate ();

		}
			
	}

	//on mouseclick, pass this limb to the climber for selection
	void OnMouseDown(){

		climber.GetComponent<ClimberClass> ().selectlimb = gameObject;

	}

	//if colliding with a hold, given that hold is not the previous hold and its not busy: 
	//remove the old hold from the holds list, add the colliding hold and stop reaching
	void OnCollisionEnter2D(Collision2D col){

		if ((col.gameObject.layer == 8) && (col.gameObject != oldhold) && (col.gameObject.GetComponent<HoldClass>().busy == false)) {
			
			climber.GetComponent<ClimberClass> ().allholds.Remove (oldhold);
			climber.GetComponent<ClimberClass> ().allholds.Add (col.gameObject);
			col.gameObject.GetComponent<HoldClass>().limbs.Add (gameObject);
			oldhold = col.gameObject;
			reaching = false;
			Debug.Log ("Got a new hold!");

			}
		}
		
	//if leaving a collision with the old hold, remove itself from the hold's list
	void OnCollisionExit2D(Collision2D colex){
		
		if ((colex.gameObject.layer == 8) && (colex.gameObject == oldhold)){
		colex.gameObject.GetComponent<HoldClass>().limbs.Remove (gameObject);
		}
	}


	//set the limbs distance joint to pull it towards the mouse position and set it to "reaching"
	public void Reach(Camera camera){
		
		target = Input.mousePosition;
		GetComponent<DistanceJoint2D> ().connectedAnchor = camera.ScreenToWorldPoint (target);
		reaching = true;
		Debug.Log ("Reaching for a hold");

	}

	//if the distance joint is spread too far, pull it closer; if not, stop reaching and go back to the previous hold
	public void Approximate (){
		
		if (GetComponent<DistanceJoint2D> ().distance > 0.05f) {
			GetComponent<DistanceJoint2D> ().distance -= 0.025f;
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
