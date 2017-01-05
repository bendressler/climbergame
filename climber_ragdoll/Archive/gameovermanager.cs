using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class gameovermanager : MonoBehaviour {

	public float timer;
	Animator anim;
	float delay = 5f;
	GameObject contr;

	void awake (){
		
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		contr = GameObject.Find("controller");
	}
	
	// Update is called once per frame
	void Update () {
		if (contr.GetComponent<controller>().gameover == true) {
			Debug.Log ("Let's animate");
			anim.SetTrigger ("gameover");
			timer += Time.deltaTime;
			if (timer >= delay) {
				SceneManager.LoadScene ("climbr_scene");
			}
		} else {
		}
	}
}
