using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameController : MonoBehaviour {

	public UnityEngine.UI.Text txtstrength;
	public UnityEngine.UI.Text txtgrip;
	public UnityEngine.UI.Text txtheight;
	public UnityEngine.UI.Text txtgameover;

	public float totalstrength;
	public float maxgrip;
	public float gripsum;
	public float grippctg;
	public bool gameover = false;
	public float startheight;
	public float totalheight;
	public float totalstrain;

	public GameObject climber;
	public GameObject holdcontainer;
	public Camera cam;

	void endGame(){
		gameover = true;
	}


	void Start(){
		gameover = false;
		totalstrength = 100;
		gripsum = 0;
		maxgrip = 20;
		startheight = cam.transform.position.y;
		totalheight = (cam.transform.position.y - startheight);
	}

	void Update(){

		totalheight = (cam.transform.position.y - startheight);
		if ((totalstrength < 1) && !gameover) {
			Debug.Log ("I'm ending this");
			endGame ();
		}
		txtstrength.text = ("Strength left: " + Mathf.Round(totalstrength));
		txtgrip.text = ("Current grip: -" + Mathf.Round(totalstrain));
		txtheight.text = ("Height: " + Mathf.Round((totalheight / 3)) + "m");
		//txtgameover.text = ("Game Over: You climbed " + Mathf.Round (totalheight / 3) + " metres!");
		totalstrength -= (GetStrain() * 0.001f);


	}


	float GetStrain(){
		
		totalstrain = 0;

		foreach (GameObject item in climber.GetComponent<ClimberClass> ().allholds) {
			totalstrain += (10-item.GetComponent<HoldClass>().size);
		}

		totalstrain += (0.1f * Mathf.Abs (climber.GetComponent<ClimberClass> ().rotate));
		totalstrain += (7-GetXSpread());
		totalstrain += (7-GetYSpread());

		return totalstrain;
	}

	float GetXSpread(){
		float lhandx = climber.GetComponent<ClimberClass> ().lefthand.transform.position.x;
		float rhandx = climber.GetComponent<ClimberClass> ().righthand.transform.position.x;
		float lfootx = climber.GetComponent<ClimberClass> ().leftfoot.transform.position.x;
		float rfootx = climber.GetComponent<ClimberClass> ().rightfoot.transform.position.x;
		List<float> coordsx = new List<float> ();
		coordsx.Add (lhandx);
		coordsx.Add (rhandx);
		coordsx.Add (lfootx);
		coordsx.Add (rfootx);
		float left = lhandx;
		float right = lhandx;
		foreach (float i in coordsx) {
			if (i < left) {
				left = i;
			}
			if (i > right) {
				right = i;
			}
		}
		return Mathf.Abs(right - left);
	}

	float GetYSpread(){
		float lhandy = climber.GetComponent<ClimberClass> ().lefthand.transform.position.y;
		float rhandy = climber.GetComponent<ClimberClass> ().righthand.transform.position.y;
		float lfooty = climber.GetComponent<ClimberClass> ().leftfoot.transform.position.y;
		float rfooty = climber.GetComponent<ClimberClass> ().rightfoot.transform.position.y;
		List<float> coordsy = new List<float> ();
		coordsy.Add (lhandy);
		coordsy.Add (rhandy);
		coordsy.Add (lfooty);
		coordsy.Add (rfooty);
		float top = lhandy;
		float bottom = lhandy;
		foreach (float i in coordsy) {
			if (i < top) {
				top = i;
			}
			if (i > bottom) {
				bottom = i;
			}
		}
		return Mathf.Abs(top - bottom);
	}
}
