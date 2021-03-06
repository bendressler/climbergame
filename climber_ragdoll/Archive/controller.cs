﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class controller : MonoBehaviour {

	public int holdcount;
	public float totalstrength;
	public float maxgrip;
	public float gripsum;
	public float grippctg;
	public float jumpmltpl;
	public bool gameover = false;
	public ArrayList allholds = new ArrayList();
	public List<GameObject> newforks = new List<GameObject> ();
	public List<GameObject> oldforks = new List<GameObject> ();
	public GameObject hold;
	public GameObject temphold;

	public UnityEngine.UI.Text txtstrength;
	public UnityEngine.UI.Text txtgrip;
	public UnityEngine.UI.Text txtheight;
	public UnityEngine.UI.Text txtgameover;

	//difficulty variables
	public float difflever;
	public float diffavg;
	public float diffvar;
	public int radius;
	public int minholds;
	public int maxholds;
	public int forkmax;
	public int anglemin;
	public int anglemax;
	public float fieldwidth;
	public Vector2 fork;
	public Vector2 newfork;
	private GameObject forker;
	public float starty;
	public float distance;

	private GameObject torso;
	public Camera cam;

	private float upperlimit;
	public GameObject highesthold;


	private void setholds (Vector2 anchor){
		holdcount = Random.Range(minholds,maxholds);
		for (int i = 0; i < holdcount; i++) {
			temphold = createHold (new Vector2 (Random.Range (anchor.x - (radius*0.5f), anchor.x + (radius*0.5f)), Random.Range (anchor.y - radius, anchor.y + radius)), Random.Range (diffavg - diffvar, diffavg + diffvar));
			if (temphold.GetComponent<HoldClass> ().colliding) {
				Destroy (temphold);
				holdcount += 1;
			}
		}
	}


	public GameObject createHold(Vector2 position, float holdsize) {
		temphold = Instantiate(hold, position, Quaternion.identity) as GameObject;
		temphold.GetComponent<HoldClass>().init(holdsize, position);
		return temphold;
	}
		

	public float subtractGrip (List<GameObject> activeholds, bool jumping){
		gripsum = 0;
		if (activeholds.Count > 0) {
		foreach (GameObject item in activeholds) {
			gripsum += item.GetComponent<HoldClass>().size;
			}
		}
		if (jumping) {
			gripsum =  gripsum / jumpmltpl;
		}
		totalstrength -= (maxgrip/1.75f - gripsum) * difflever;
		distance = (torso.transform.position.y - starty);
		return totalstrength;
	}

	void endgame(){
		gameover = true;
	}

	bool noholdabove(float limit){
		if (highesthold.transform.position.y > cam.transform.position.y + limit){
			return false;
		} else {
			return true;
		}
	}



	// Use this for initialization
	void Start () {

		difflever = 0.5f; //smaller for lower difficulty
		torso = GameObject.Find("torso");
		forker = GameObject.Find("forker");
		gameover = false;
		totalstrength = 140;
		gripsum = 0;
		maxgrip = 20;
		//difficulty variables
		diffavg = 3.0f;
		diffvar = 2.0f;
		minholds = 3;
		maxholds = 6;
		jumpmltpl = 2.5f;
		radius = 4;
		forkmax = 6;
		fieldwidth = 30;
		starty = torso.transform.position.y;
		distance = (torso.transform.position.y - starty);
		setholds (forker.transform.position);
		newforks.Add (forker);
		upperlimit = 3;

	}
	
	// Update is called once per frame
	void Update () {
		if (noholdabove(upperlimit)) {
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
				setholds (item.transform.position);
				}
		}

		if ((totalstrength < 1) && !gameover) {
			Debug.Log ("I'm ending this");
			endgame ();
		}
		grippctg = (gripsum / maxgrip) * 100;
		txtstrength.text = ("Strength left: " + Mathf.Round(totalstrength));
		txtgrip.text = ("Current grip: " + Mathf.Round(grippctg) + "%");
		txtheight.text = ("Height: " + Mathf.Round((distance / 3)) + "m");
		txtgameover.text = ("Game Over: You climbed " + Mathf.Round (distance / 3) + " metres!");

	}


		/*
		if (totalstrength < 0.1) {
			gameover = true;
		}

*/
}
