using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoldContainer : MonoBehaviour {

	public int holdcount;

	public ArrayList allholds = new ArrayList();
	public List<GameObject> newforks = new List<GameObject> ();
	public List<GameObject> oldforks = new List<GameObject> ();
	public GameObject hold;
	public GameObject temphold;
	public GameObject wall;

	public Camera cam;


	//difficulty variables
	public float difflever;
	public float diffavg;
	public float diffvar;
	public float radius;
	public int minholds;
	public int maxholds;
	public int forkmax;
	public int anglemin;
	public int anglemax;
	public float fieldwidth;
	public Vector2 fork;
	public Vector2 newfork;
	private GameObject forker;
	private int wallcounter;

	private float upperlimit;
	public GameObject highesthold;

	private void setHolds (Vector2 anchor){
		GameObject finalhold;
		holdcount = Random.Range(minholds,maxholds);
		for (int i = 0; i < holdcount; i++) {
			temphold = createHold (new Vector2 (Random.Range (anchor.x - (radius*0.5f), anchor.x + (radius*0.5f)), Random.Range (anchor.y - radius, anchor.y + radius)), Random.Range (diffavg - diffvar, diffavg + diffvar));
			if (!temphold.GetComponent<HoldClass> ().colliding) {
				finalhold = createHold (temphold.transform.position, Random.Range (diffavg - diffvar, diffavg + diffvar));
				Destroy (temphold);
				//holdcount += 1;
			}
		}
	}


	public GameObject createHold(Vector2 position, float holdsize) {
		temphold = Instantiate(hold, position, Quaternion.identity) as GameObject;
		temphold.GetComponent<HoldClass>().init(holdsize, position);
		return temphold;
	}


	bool noHoldAbove(float limit){
		if (highesthold.transform.position.y > cam.transform.position.y + limit){
			return false;
		} else {
			return true;
		}
	}

	void setNewForks(){
			oldforks = new List<GameObject> (newforks); //hands new forks from last round into oldforks
			newforks.Clear ();
			//as long as we're under the fork limit, creates between 0 and 3 forks for each old fork
			foreach (GameObject item in oldforks) { 
				if (newforks.Count < forkmax) {
					for (int i = 0; i < (Random.Range (0, 4)); i++) {
						newforks.Add ((GameObject)Instantiate (item));
						Debug.Log ("Creating new fork");
					}
				} else {
					newforks.Remove (newforks [Random.Range (0, newforks.Count)]); //remove random fork if over max
					Debug.Log ("Deleting random fork");
				}
				if (newforks.Count < 1){
					newforks.Add ((GameObject)Instantiate (item)); //if too few forks, add one
				}
			}

			//for each new fork create a random direction vector, normalise it, move the fork to the new position, create holds
			foreach (GameObject item in newforks) {
				Vector3 angle = new Vector3 (Random.Range (-1.0f, 1.0f), Random.Range (0, 1.0f), 0);
				angle.Normalize ();
				item.transform.position = (item.transform.position + (angle * (radius*1.5f)));
				setHolds (item.transform.position);
			}
		}

	// Use this for initialization
	void Start () {

		difflever = 0.5f; //smaller for lower difficulty
		forker = GameObject.Find("forker");

		//difficulty variables
		diffavg = 3.0f;
		diffvar = 2.0f;
		minholds = 3;
		maxholds = 5;
	
		radius = 2.5f;
		forkmax = 6;
		fieldwidth = 30;
		wallcounter = 1;

		setHolds (forker.transform.position);
		newforks.Add (forker);
		upperlimit = 10;
		setNewForks ();

	}
	
	// Update is called once per frame
	void Update () {
		if ((noHoldAbove (upperlimit)) ) {
			setNewForks ();
		}

		if (cam.transform.position.y > (18 * wallcounter) - 5){
			wall = Instantiate (wall, new Vector2(wall.transform.position.x, wall.transform.position.y + 24),Quaternion.identity) as GameObject;
			wallcounter += 1;
		}
	}

}
