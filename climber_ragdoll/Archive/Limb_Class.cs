using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Limb_Class : MonoBehaviour {

	public string own_name; //used to receive name from instantiate function
	public Vector2 pos; //own position, gets updated by torso when setting new position
	public Vector2 jnt; //joint, set to corresponding torso joint 
	public float rch; //reach, received upon instantiation
	public GameObject hold; //hold this limb is on
	public float strength;
	public float grip;

	//declare variables used in reach calculation
	public float dist_jnt;
	public float dist_tgt;

	private GameObject torso; //variable to store parent torso

	//initialise with reach, limb position, joint position and name; store torso into variable
	public void init (Vector2 extr, Vector2 joint, float reach, string nam, GameObject tor) {
		rch = reach;
		pos = extr;
		jnt = joint;
		own_name = nam; 
		torso = tor;

	}
		
	// check if coordinate difference is larger than reach - then give position the next step towards joint vector
	public void adjust_back(Vector2 pos, Vector2 jnt) {
		dist_jnt = Vector2.Distance (pos, jnt);
		if (dist_jnt >= rch) {
			gameObject.transform.position = Vector2.MoveTowards (gameObject.transform.position, jnt, 0.1f);
			pos = gameObject.transform.position;
			} else {

		}
	}

	public void deselect(){
		if (hold != null) {
			hold.GetComponent<HoldClass> ().busy = false;
			torso.GetComponent<torsoClass>().activeholds.Remove (hold);
		}
	}

	public void selecttarget(GameObject target){
		hold = target;
		hold.GetComponent<HoldClass> ().busy = true;
		torso.GetComponent<torsoClass>().activeholds.Add (hold);
	}

	//check if already selected, if not appoint self as executing limb. if selected, deselect self
	void OnMouseDown() {

		if (torso.GetComponent<torsoClass> ().selectlimb != this) {
			torso.GetComponent<torsoClass> ().selectlimb = this;
			Debug.Log ("I'm the selected limb");
			foreach (Limb_Class item in torso.GetComponent<torsoClass>().limbs) {
				item.transform.localScale = new Vector3 ((1), (1), (0)); //set all limbs to basic scale
			}
			transform.localScale += new Vector3((0.2f), (0.2f), (0)); //increase scale of this limb
		} else {
			torso.GetComponent<torsoClass> ().selectlimb = null;
			transform.localScale = new Vector3((1), (1), (0));
		}
	}



	//check if joint->target distance is within reach, return TRUE if it is
	public bool check_reach(Vector2 target) {
		dist_tgt = Vector2.Distance (target, jnt);
		if (rch >= dist_tgt) {
			Debug.Log ("I can reach");
			return true;
		}
		else {
			} 
		Debug.Log ("I cant reach");
		return false;
	}

	public void update_pos (){
		adjust_back (pos,jnt);
		pos = this.transform.position;
		dist_jnt = Vector2.Distance (pos, jnt);
		transform.localScale = new Vector3((1), (1), (0));
	}

	// checks whether a limb can be stretched towards x given a target point and limb reach
	public bool canstretch_x (GameObject target, Transform tor){
		Transform torsoCentre = tor;
		float dirx = Mathf.Sign (target.transform.position.x - torsoCentre.position.x); //target - torso 
		float limbdirx = Mathf.Sign (this.transform.position.x - torsoCentre.position.x); //calculate torso to limb direction on x
		//if the limb under observation is the limb trying to reach return true, else...
		if (this.own_name == torso.GetComponent<torsoClass> ().selectlimb.own_name) {
			//Debug.Log (own_name + ": I'm the chosen limb so I can move to X");
			return true;
		} 
		else {	
			if ((limbdirx == dirx) || (Mathf.Abs ((this.transform.position.x - jnt.x)) <= rch)) {
				return true; //return true if limb is not fully stretched or in the same direction on x
			} else {
				return false; //return false if limb is in other x direction and fully stretched
			}
		} 
	} 

	// checks whether a limb can be stretched towards y given a target point and limb reach
	public bool canstretch_y (GameObject target, Transform tor){
		Transform torsoCentre = tor;
		float diry = Mathf.Sign (target.transform.position.y - torsoCentre.position.y); //calculate torso to target direction on y
		float limbdiry = Mathf.Sign (this.transform.position.y - torsoCentre.position.y); //calculate torso to limb direction on y
		//if the limb under observation is the limb trying to reach return true, else...
		if (this.own_name == torso.GetComponent<torsoClass> ().selectlimb.own_name) { 
			//Debug.Log ("I'm the chosen limb so I can move to Y");
			return true;
		} 
		else {	
			if ((limbdiry == diry) || (Mathf.Abs ((this.transform.position.y - jnt.y)) <= rch)) {
				return true; //return true if limb is not fully stretched or in the same direction on y
			} else {
				return false; //return false if limb is in other y direction and fully stretched
			}
		} 
	} 

	public bool limbstretched(){
		return (Vector2.Distance (pos, jnt) >= (rch - 0.05f));
	}






	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		
	}
}





/*
 * // checks whether a limb can be stretched towards x given a target point and limb reach
public bool canstretch_x (GameObject target){
	float dirx = Mathf.Sign (target.transform.position.x - jnt.x); //calculate torso to target direction on x
	float limbdirx = Mathf.Sign (this.transform.position.x - jnt.x); //calculate torso to limb direction on x
	//if the limb under observation is the limb trying to reach return true, else...
	if (this.own_name == torso.GetComponent<torsoClass> ().selectlimb.own_name) {
		Debug.Log (own_name + ": I'm the chosen limb so I can move to X");
		return true;
	} 
	else {	
		if ((limbdirx == dirx) || (Mathf.Abs ((this.transform.position.x - jnt.x)) <= rch)) {
			Debug.Log (own_name + " wants to move its X towards " + dirx + " and it can.");
			return true; //return true if limb is not fully stretched or in the same direction on x
		} else {
			Debug.Log (own_name + " wants to move its X towards " + dirx + " but it is stretched.");
			return false; //return false if limb is in other x direction and fully stretched
		}
	} 
} 

// checks whether a limb can be stretched towards y given a target point and limb reach
public bool canstretch_y (GameObject target){
	float diry = Mathf.Sign (target.transform.position.y - jnt.y); //calculate torso to target direction on y
	float limbdiry = Mathf.Sign (this.transform.position.y - jnt.y); //calculate torso to limb direction on y
	//if the limb under observation is the limb trying to reach return true, else...
	if (this.own_name == torso.GetComponent<torsoClass> ().selectlimb.own_name) { 
		Debug.Log ("I'm the chosen limb so I can move to Y");
		return true;
	} 
	else {	
		if ((limbdiry == diry) || (Mathf.Abs ((this.transform.position.y - jnt.y)) <= rch)) {
			Debug.Log (own_name + " wants to move its Y towards " + diry + " and it can.");
			return true; //return true if limb is not fully stretched or in the same direction on y
		} else {
			Debug.Log (own_name + " wants to move its Y towards " + diry + " but it is stretched.");
			return false; //return false if limb is in other y direction and fully stretched
		}
	} 
} 
*/