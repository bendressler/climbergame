using UnityEngine;
using System.Collections;

public class holdcontainerScript : MonoBehaviour {

	public int holdcount;
	public GameObject masterhold;
	public GameObject holdcontainer;

	// Use this for initialization
	void Start () {
		Vector2 target;
		for (int i = 0; i < holdcount; i++){
			target = new Vector2 (this.gameObject.transform.position.x + Random.Range (1, 20), this.gameObject.transform.position.y + Random.Range (1, 20));
			Instantiate (masterhold, target, Quaternion.identity);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
