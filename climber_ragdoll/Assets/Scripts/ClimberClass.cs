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



	// Use this for initialization
	void Start () {
		
		armreach = 3f;
		legreach = 3.5f;
		selectlimb = rightfoot;
		target = rightfoot.GetComponent<Transform> ().position;
		reach = 5;


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
			selectlimb.GetComponent<Limb>().Reach (cam);

		}
			

	}





}

