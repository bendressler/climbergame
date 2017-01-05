using UnityEngine;
using System.Collections;

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

	public GameObject oldhold;
	public bool reaching;
	private Vector2 oldposition;



	// Use this for initialization
	void Start () {
		
		armreach = 3f;
		legreach = 3.5f;
		selectlimb = rightfoot;
		target = rightfoot.GetComponent<Transform> ().position;
		reach = 5;
		oldposition = selectlimb.transform.position;
		reaching = false;

	}


	void Update (){
		
		if (Input.GetKey (KeyCode.Q)) {
			selectlimb = lefthand;
			reach = armreach;
		}

		if (Input.GetKey (KeyCode.W)) {
			selectlimb = righthand;
			reach = armreach;
		}

		if (Input.GetKey (KeyCode.E)) {
			selectlimb = leftfoot;
			reach = legreach;
		}

		if (Input.GetKey (KeyCode.R)) {
			selectlimb = rightfoot;
			reach = legreach;
		}
			
		if (Input.GetMouseButton (0)) {
			Reach ();
			oldposition = selectlimb.transform.position;
		}

		if (reaching == true) {
			if (selectlimb.GetComponent<DistanceJoint2D> ().distance > 0.05f) {
				Approximate ();
			} else {
				Debug.Log ("Didn't manage to grab a hold");
				reaching = false;
				selectlimb.GetComponent<DistanceJoint2D> ().distance = 0.1f;
				selectlimb.GetComponent<DistanceJoint2D> ().connectedAnchor = oldhold.transform.position;
			}
		}


	}

	void Approximate (){

		selectlimb.GetComponent<DistanceJoint2D> ().distance -= 0.01f;
		Debug.Log ("Pulling closer");

	}


	void Reach(){
		target = Input.mousePosition;
		selectlimb.GetComponent<DistanceJoint2D> ().connectedAnchor = cam.ScreenToWorldPoint (target);
		selectlimb.GetComponent<Limb>().reaching = true;
		Debug.Log ("Reaching for a hold");
	}
}

