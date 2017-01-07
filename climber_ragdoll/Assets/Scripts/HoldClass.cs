using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoldClass : MonoBehaviour {
	
	public Vector2 coord;
	public bool busy;
	public bool colliding;
	public int slots;

	public List<GameObject> limbs = new List<GameObject>();

	public Sprite small;
	public Sprite normal;
	public Sprite large;

	public float size;

	private GameObject holdcontainer;
	private float temps;


	//when created by the HoldContainer, assume coordinates, size and adjust sprite size accordingly
	public void init(float holdsize, Vector2 position) {
		
		coord = position;
		size = holdsize;
		busy = false;
		this.transform.position = new Vector3 (coord.x, coord.y, 0.2f);
		transform.localScale = new Vector3 (0.1f, 0.1f, 0);
		transform.localScale += new Vector3((0.2f * size), (0.2f * size), (0));

	}


	//if colliding with a physical object, return true
	void OnCollisionStay2D (Collision2D col){

		colliding = true;

	}

	void OnCollisionExit2D (Collision2D col2){
		
		if (limbs.Count < 1) {
			colliding = false;
		}
	}


	void Start () {
		
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
			
		if (size > 4.5f) {
			gameObject.GetComponent<SpriteRenderer> ().sprite = large;

		} else if (size < 1.5f) {
			gameObject.GetComponent<SpriteRenderer> ().sprite = small;
		} else {
			gameObject.GetComponent<SpriteRenderer> ().sprite = normal;
		}

		slots = (Mathf.RoundToInt(size / 3));
		if (slots < 1) {
			slots = 1;
		}

	}


	// Update is called once per frame
	void Update () {

		//if there are equal or more limbs attached than the hold has slots, be busy
		if (limbs.Count >= slots) {
			busy = true;
		} else {
			busy = false;
		}

	}
}