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
		
		armreach = 3f;
		legreach = 3.5f;
		selectlimb = rightfoot;
		target = rightfoot.GetComponent<Transform> ().position;
		reach = 5;
		torsorb = torso.GetComponent<Rigidbody2D> ();
		torsorb.centerOfMass = new Vector2 (torsorb.centerOfMass.x, torsorb.centerOfMass.y - 0.5f);
		rotate = torsorb.rotation;
	
	}


	void Update (){
			
		if (Input.GetMouseButton (0)) {
			selectlimb.GetComponent<Limb> ().Reach (cam);

		}

		rotate = torsorb.rotation;

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

