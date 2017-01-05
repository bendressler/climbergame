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
	public float jumpmltpl;
	public bool gameover = false;
	public float startheight;
	public float totalheight;

	public GameObject climber;
	public GameObject holdcontainer;
	public Camera cam;

	void endGame(){
		gameover = true;
	}

	public float subtractGrip (ArrayList activeholds, bool jumping){
		gripsum = 0;
		if (activeholds.Count > 0) {
			foreach (GameObject item in activeholds) {
				gripsum += item.GetComponent<HoldClass>().size;
			}
		}
		if (jumping) {
			gripsum =  gripsum / jumpmltpl;
		}
		totalstrength -= (maxgrip/1.75f - gripsum) * holdcontainer.GetComponent<HoldContainer>().difflever;
		totalheight = (climber.transform.position.y - startheight);
		return totalstrength;
	}


	void Start(){
		gameover = false;
		totalstrength = 140;
		gripsum = 0;
		maxgrip = 20;
		jumpmltpl = 2.5f;
		startheight = climber.transform.position.y;
		totalheight = (climber.transform.position.y - startheight);
	}

	void Update(){
		if ((totalstrength < 1) && !gameover) {
			Debug.Log ("I'm ending this");
			endGame ();
		}
		grippctg = (gripsum / maxgrip) * 100;
		txtstrength.text = ("Strength left: " + Mathf.Round(totalstrength));
		txtgrip.text = ("Current grip: " + Mathf.Round(grippctg) + "%");
		txtheight.text = ("Height: " + Mathf.Round((totalheight / 3)) + "m");
		txtgameover.text = ("Game Over: You climbed " + Mathf.Round (totalheight / 3) + " metres!");
	}

}
