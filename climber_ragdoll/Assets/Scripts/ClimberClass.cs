using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClimberClass : MonoBehaviour {

	public Vector2 target;
	public float armreach;
	public float legreach;
	public GameObject righthand;
	public GameObject lefthand;
	public GameObject rightfoot;
	public GameObject leftfoot;
	public GameObject torso;
	public GameObject selectlimb;
	public float reach;
	public Camera cam;
	public List<GameObject> allholds = new List<GameObject>();
	public float rotate;

	private Rigidbody2D torsorb;


	// Use this for initialization
	void Start () {

		torsorb = torso.GetComponent<Rigidbody2D> ();
		armreach = 3f;
		legreach = 3.5f;
		selectlimb = rightfoot;
		target = rightfoot.GetComponent<Transform> ().position;
		reach = 5;
		rotate = torsorb.rotation;

		//set centre of gravity in the torso lower
		torsorb.centerOfMass = new Vector2 (torsorb.centerOfMass.x, torsorb.centerOfMass.y - 0.5f);
	
	}


	void Update (){

		//if the climber is clicked, call the selected limb's Reach() function
		if (Input.GetMouseButton (0)) {

			selectlimb.GetComponent<Limb> ().Reach (cam);

		}

		//update the torso's rotation
		rotate = torsorb.rotation;

		//if the torso tilts too far, apply an anti-force
		if (rotate < -5) {
			//Debug.Log ("Positive torque");
			torsorb.AddTorque (20);
		} else if (rotate > 5) {
			torsorb.AddTorque (-20);
			//Debug.Log ("Negative torque");
		} else if (rotate == 0) {
			torsorb.AddTorque (0);
			//Debug.Log ("Neutralise torque");
		}
	}





}

