using UnityEngine;
using System.Collections;

public class ClimberClass : MonoBehaviour {

	public Vector2 target;
	public Vector2 startpos;
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




	void approximate (){
		selectlimb.GetComponent<DistanceJoint2D> ().distance -= 0.01f;
	}


	// Use this for initialization
	void Start () 
	{
		armreach = 3f;
		legreach = 3.5f;
		selectlimb = rightfoot;
		target = rightfoot.GetComponent<Transform> ().position;
		reach = 5;
		startpos = selectlimb.transform.position;

	}

	void Update ()
	{
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
			target = Input.mousePosition;
			selectlimb.GetComponent<DistanceJoint2D> ().connectedAnchor = cam.ScreenToWorldPoint (target);
			Debug.Log (Input.mousePosition);
		} else {
			if (selectlimb.GetComponent<DistanceJoint2D> ().distance > 0.05f) {
				approximate ();
			}
		}
	}



	}



/*
 * if (Vector2.Distance (selectlimb.transform.position, torso.transform.position) < reach) {				
			
			if (Input.GetKey (KeyCode.UpArrow)) {
				selectlimb.GetComponent<Rigidbody2D> ().MovePosition (new Vector2 (selectlimb.transform.position.x, selectlimb.transform.position.y + 0.1f));
			}

			if (Input.GetKey (KeyCode.DownArrow)) {
				selectlimb.GetComponent<Rigidbody2D> ().MovePosition (new Vector2 (selectlimb.transform.position.x, selectlimb.transform.position.y - 0.1f));
			}

			if (Input.GetKey (KeyCode.LeftArrow)) {
				selectlimb.GetComponent<Rigidbody2D> ().MovePosition (new Vector2 (selectlimb.transform.position.x - 0.1f, selectlimb.transform.position.y));
			}

			if (Input.GetKey (KeyCode.RightArrow)) {
				selectlimb.GetComponent<Rigidbody2D> ().MovePosition (new Vector2 (selectlimb.transform.position.x + 0.1f, selectlimb.transform.position.y));
			}
		}


 * 
 * bool stretched(){
		if (rightfoot.GetComponent<DistanceJoint2D>().distance >= legreach) {
			Debug.Log ("I'm stretched");
			return true;
		} else {
			return false;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			target = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
			Debug.Log (target);
		}

		if (!stretched()) {
			rightfoot.GetComponent<Transform> ().position = Vector2.MoveTowards (rightfoot.GetComponent<Transform> ().position, target, 0.1f);
		}
		}
 * 
 * 
 * 
 *toggle = false;
		stretched = false;
		torso.GetComponent<Rigidbody2D> ().centerOfMass = new Vector2 (0, -1);	



 *if (Input.GetKey("space")) 
		{
			toggle = false;
		} 
		else 
		{
			toggle = true;
		}


		if (toggle == true) 
		{
			righthand.GetComponent<Rigidbody2D> ().isKinematic = true;
			lefthand.GetComponent<Rigidbody2D> ().isKinematic = true;
			rightfoot.GetComponent<Rigidbody2D> ().isKinematic = true;
			leftfoot.GetComponent<Rigidbody2D> ().isKinematic = true;
			if (stretched == false) {
				Vector2.MoveTowards(righthand.GetComponent<Transform>().position, new Vector2(righthand.GetComponent<Transform>().position.x + 1, righthand.GetComponent<Transform>().position.y),0.5f);
			} else {
			}


			Debug.Log ("I'm kinematic");
		} else 
		{
			righthand.GetComponent<Rigidbody2D> ().isKinematic = false;
			lefthand.GetComponent<Rigidbody2D> ().isKinematic = false;
			rightfoot.GetComponent<Rigidbody2D> ().isKinematic = false;
			leftfoot.GetComponent<Rigidbody2D> ().isKinematic = false;	


		}
	
		if (righthand.GetComponent<DistanceJoint2D> ().distance > 0.3f) {
			stretched = true;
			Debug.Log ("I'm ripping apart!");
		} else {
			stretched = false;
		}
 *
 *public bool toggle;
	public bool stretched;
 *
 *
 * */