using UnityEngine;
using System.Collections;

public class LowerLimbClass : MonoBehaviour {


	public GameObject climber;
	public GameObject limb;


	// Use this for initialization
	void Start () {
		climber = GameObject.Find ("climber");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//if clicked, make the climber select the attached limb 
	void OnMouseDown(){
		climber.GetComponent<ClimberClass> ().selectlimb = limb.gameObject;
	}
}
