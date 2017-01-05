using UnityEngine;
using System.Collections;

public class HoldClass : MonoBehaviour {
	
	public Vector2 coord;
	public bool busy;
	public bool colliding;
	private float temps;

	public float size; //difficulty, is assessed by limbs/torso to calculate grip

	private GameObject climber;
	private GameObject holdcontainer;


	public void init(float holdsize, Vector2 position) {
		coord = position;
		size = holdsize;
		busy = false;
		this.transform.position = new Vector3 (coord.x, coord.y, 0.2f);
		transform.localScale = new Vector3 (0.5f, 0.5f, 0);
		transform.localScale += new Vector3((0.3f * size), (0.3f * size), (0));
	}

	//returns true if colliding with a physical object
	void OnCollisionStay (Collision collisionInfo){
		colliding = true;
	}

	void Start () {
		climber = GameObject.Find ("climber");
		holdcontainer = GameObject.Find ("holdContainer");
		busy = false;
		holdcontainer.GetComponent<HoldContainer> ().allholds.Add (gameObject);
		if (holdcontainer.GetComponent<HoldContainer> ().highesthold != null) {
			if (holdcontainer.GetComponent<HoldContainer> ().highesthold.transform.position.y < gameObject.transform.position.y) {
				holdcontainer.GetComponent<HoldContainer> ().highesthold = gameObject;
			}
		}
		else {
			holdcontainer.GetComponent<HoldContainer> ().highesthold = gameObject;
		}
	}


	/*
	//on click: if a limb is selected and the hold isnt busy, sends its own ID over to torsoClass as target variable, triggers torso movement
	void OnMouseUp() {
		if (Input.GetMouseButtonUp(0) && ((Time.time - temps) < 0.2)){
			if ((torso.GetComponent<Climber> ().selectlimb != null) && (!this.busy)) {
				torso.GetComponent<Climber> ().targetpoint = this.gameObject;
				torso.GetComponent<Climber> ().movetorso ();
				Debug.Log ("I'm the target");
			} else {
				Debug.Log ("There is no target");
			}
		}
		// on long press: if a limb is selected and the hold isnt busy and the torso can jump, makes self the target, asks limb to move and sets jumping to true
		if (Input.GetMouseButtonUp (0) && ((Time.time - temps) > 0.2)) {
			Debug.Log ("Wanna jump");
			if ((torso.GetComponent<torsoClass> ().selectlimb != null) && (!this.busy)) {
				if (torso.GetComponent<torsoClass> ().canjump(gameObject)){
					torso.GetComponent<torsoClass> ().targetvar = gameObject;
					torso.GetComponent<torsoClass>().moveSelectLimb();
					torso.GetComponent<torsoClass>().jumping = true;
					busy = false;
					Debug.Log ("I can jump");
				}
			}
		}
	}




	// Use this for initialization


	
	// Update is called once per frame
	void Update () {
		//longpress
		if ( Input.GetMouseButtonDown (0) )
		{
			temps = Time.time;

		}

	}
	*/
}