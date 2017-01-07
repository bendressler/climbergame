using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoldClass : MonoBehaviour {
	
	public Vector2 coord;
	public bool busy;
	public bool colliding;
	public int slots;
	public List<GameObject> limbs = new List<GameObject>();

	public float size; //difficulty, is assessed by limbs/torso to calculate grip

	private GameObject climber;
	private GameObject holdcontainer;
	private float temps;


	public void init(float holdsize, Vector2 position) {
		
		coord = position;
		size = holdsize;
		busy = false;
		this.transform.position = new Vector3 (coord.x, coord.y, 0.2f);
		transform.localScale = new Vector3 (0.2f, 0.2f, 0);
		transform.localScale += new Vector3((0.3f * size), (0.3f * size), (0));

	}


	//returns true if colliding with a physical object
	void OnCollisionStay2D (Collision2D col){

		colliding = true;


		if ((col.gameObject.layer == 9) && !limbs.Contains (col.gameObject) && !busy) {
			limbs.Add (col.gameObject);
		}

		if (limbs.Count >= slots) {
			busy = true;
		} else {
			busy = false;
		}

		foreach (GameObject item in limbs) {
			if (col.gameObject != item) {
				limbs.Remove (item);
			}
		}
	}

	void OnCollisionExit2D (Collision2D col2){
		
		if (limbs.Count < 1) {
			colliding = false;
		}
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


	// Update is called once per frame
	void Update () {
		slots = (Mathf.RoundToInt(size / 3) + 1);
	}
}