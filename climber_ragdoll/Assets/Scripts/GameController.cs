using UnityEngine;
using System.Collections;

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
		grippctg = (gripsum / maxgrip) * 100;
		txtstrength.text = ("Strength left: " + Mathf.Round(totalstrength));
		txtgrip.text = ("Current grip: " + Mathf.Round(grippctg) + "%");
		txtheight.text = ("Height: " + Mathf.Round((totalheight / 3)) + "m");
		//txtgameover.text = ("Game Over: You climbed " + Mathf.Round (totalheight / 3) + " metres!");

		totalstrain = 0;
		foreach (GameObject item in climber.GetComponent<ClimberClass> ().allholds) {
			totalstrain += item.GetComponent<HoldClass>().size;
		}
		totalstrength -= (totalstrain * 0.001f);

	}

}
