using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {


	public GameObject climber;

	private Vector3 offset;


	// Use this for initialization
	void Start () {

		//store the default difference between camera position and climber position
		offset = transform.position - climber.transform.position; 

	}
	
	// Update is called once per frame
	void Update () {

		//set camera to player position and add the default difference
		transform.position = offset + climber.transform.position; 
	}
}
