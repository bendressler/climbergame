using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class torsoClass : climber_class {

	public GameObject controller;
	//declare initial positions for limbs (could be cleaned up by "creating at Joint_pos+x,y")
	public Vector2 rh_pos;
	public Vector2 lh_pos;
	public Vector2 rf_pos;
	public Vector2 lf_pos;
	//declare joint positions; this is the limbs' anchor for reach
	public Vector2 lh_jnt;
	public Vector2 rh_jnt;
	public Vector2 lf_jnt;
	public Vector2 rf_jnt;
	//declare torso centre, will be used for center of gravity calculations later
	public Vector2 centre;
	//declare renderer box, only needed for coordinates of collider - clean up somewhow?
	public Renderer box;
	//declare reach variables
	public float armreach;
	public float legreach;
	public float jumpreach;
	public GameObject targetvar; //stores target hold last clicked on
	public Limb_Class selectlimb; //stores limb last clicked on
	//variables that store limbs
	public Limb_Class righthand;
	public Limb_Class lefthand;
	public Limb_Class rightfoot;
	public Limb_Class leftfoot;
	public Limb_Class limb; //only in limb instantiation - needed?
	//variables that store x and y coordinates for sides of collider box - determine joints
	public float right;
	public float left;
	public float top;
	public float bottom;
	public bool moving; //switch to tell update function whether torso should be moved
	public bool jumping;

	public List<GameObject> activeholds = new List<GameObject>();
	public List<Limb_Class> limbs = new List<Limb_Class>();

	public Sprite sprhand;
	public Sprite splhand;
	public Sprite sprfoot;
	public Sprite splfoot;

	private Vector2 savedpos; //saves torso position to move back if move is not possible
	public Vector2 movedir; //saves direction to move into

	Animator anim;

	//instantiate a limb and return object ID, requiring a position for itself, the joint it belongs to, a max reach and a name
	public Limb_Class createLimb(Vector2 extr, Vector2 joint, float reach, string nam) {
		Limb_Class limbInstance; 
		limbInstance = Instantiate(limb, extr, Quaternion.identity) as Limb_Class;
		limbInstance.init(extr, joint, reach, nam, this.gameObject);
		return limbInstance;
	}

	//if both feet have a stand and distance is in jumpreach, return true
	public bool canjump (GameObject hold){
		if (rightfoot.hold != null && leftfoot.hold != null && (Vector2.Distance(selectlimb.transform.position, hold.transform.position) <= jumpreach)) {
			return true;
		} else {
			Debug.Log ("Can't jump");
			return false;
		}
	}



	// main function that triggers either limb or torso movement
	public void movetorso() {
		savedpos = this.transform.position; //save current torso position
		if ((targetvar != null) && (selectlimb != null) && (!targetvar.GetComponent<HoldClass>().busy)) { //check if both a target and a limb have been selected and the target is not busy
			if (selectlimb.check_reach (targetvar.transform.position)) { //checks if the selected limb confirms its own reach to the target
				moveSelectLimb();
				disengage ();
			//	selectlimb.adjust_back (selectlimb.pos, selectlimb.jnt); //let the limb move back into its reach if it overstepped
			} else {
				moving = true; //if limb is not in reach, set the torso up to move and capture the direction of the target
				Debug.Log("Let's move my body");
				//setMoveDir();
			}
		}
	}

	//move the selected limb to the target position, then deselect its previous hold and remove it from the active holds; select the new hold
	public void moveSelectLimb (){
		selectlimb.transform.position = new Vector2(targetvar.transform.position.x, targetvar.transform.position.y);
		selectlimb.pos = selectlimb.transform.position;
		Debug.Log ("Moving limb");
		selectlimb.deselect();
		selectlimb.selecttarget(targetvar);
		selectlimb.update_pos();
		controller.GetComponent<controller> ().subtractGrip (activeholds, this.jumping);  //call controller to update totalstrength based on the cumulative gripsum
	}

	/*public void setMoveDir (){
		movedir.x = Mathf.Sign (targetvar.transform.position.x - centre.x);
		movedir.y = Mathf.Sign (targetvar.transform.position.y - centre.y);
	}
*/

	public void disengage (){
		selectlimb.update_pos();
		moving = false;
		jumping = false;
		targetvar = null;
		selectlimb = null;
		updateposition ();
	}

	//checks whether all limbs can be stretched towards x
	public bool allstretch_x(GameObject tor){
		if (lefthand.canstretch_x (targetvar, tor.transform)
			&& righthand.canstretch_x (targetvar, tor.transform)
			&& leftfoot.canstretch_x (targetvar, tor.transform)
			&& rightfoot.canstretch_x (targetvar, tor.transform)){
			return true;
		}
		else {
			return false;
		}
	}

	//checks whether all limbs can be stretched towards y
	public bool allstretch_y(GameObject tor){
		if (lefthand.canstretch_y (targetvar, tor.transform)
			&& righthand.canstretch_y (targetvar, tor.transform)
			&& leftfoot.canstretch_y (targetvar, tor.transform)
			&& rightfoot.canstretch_y (targetvar, tor.transform)){
			return true;
		}
		else {
			return false;
		}
	}

	// set all position variables within torso to latest position
	void updateposition(){
		right = transform.position.x + (box.bounds.size.x / 2);
		left = transform.position.x - (box.bounds.size.x / 2);
		top = transform.position.y + (box.bounds.size.y / 2);
		bottom = transform.position.y - (box.bounds.size.y / 2);
		lf_jnt = new Vector2 (left, bottom);
		rf_jnt = new Vector2 (right, bottom);
		rh_jnt = new Vector2 (right, top);
		lh_jnt = new Vector2 (left, top);
		if (righthand != null) {
			righthand.jnt = rh_jnt;
			lefthand.jnt = lh_jnt;
			rightfoot.jnt = rf_jnt;
			leftfoot.jnt = lf_jnt;
		}
		centre = this.transform.position;
	}
		
	void grabclosest(){
		targetvar = null;
		foreach (Limb_Class item in limbs) {
			float min = item.GetComponent<Limb_Class>().rch; //set max reach as starting point for iteration
			item.transform.position = item.GetComponent<Limb_Class> ().jnt; //move limb to new joint position
			//for each hold in the game, check if its closer than the min variable. if so set new min distance and override target with hold
			foreach (GameObject hold in controller.GetComponent<controller>().allholds){
				if (Vector2.Distance (item.transform.position, hold.transform.position) < min) {
					min = Vector2.Distance (item.transform.position, hold.transform.position);
					targetvar = hold;
				}
			}
		// if we end up with a result that is not busy, let the current limb move to it
			if ((targetvar != null) && (!targetvar.GetComponent<HoldClass> ().busy)) {
				selectlimb = item;
				moveSelectLimb ();
				Debug.Log ("I'm grabbing an empty hold");
		} else {
				//else if there is no free result in reach, let the limb hang down
				item.transform.position = new Vector2(item.transform.position.x, item.transform.position.y - item.GetComponent<Limb_Class> ().rch);
				Debug.Log ("I can't find a free hold");
			}
		}
	}

	void createlimbs (){
		limbs.Clear ();
		rh_pos = new Vector2(rh_jnt.x + (armreach - 0.5f),rh_jnt.y);
		//righthand = createLimb (rh_pos, new Vector2(right, top), armreach, "righthand");
		righthand = GameObject.Find("righthand").GetComponent<Limb_Class>();
		righthand.init (righthand.transform.position, rh_pos, armreach, "righthand", this.gameObject);
		righthand.GetComponent<SpriteRenderer> ().sprite = sprhand;
		limbs.Add (righthand);
		lh_pos = new Vector2(lh_jnt.x - (armreach - 0.5f),lh_jnt.y);
		//lefthand = createLimb (lh_pos, new Vector2(left, top), armreach, "lefthand");
		lefthand = GameObject.Find("lefthand").GetComponent<Limb_Class>();
		lefthand.init (lefthand.transform.position, lh_pos, armreach, "lefthand", this.gameObject);
		lefthand.GetComponent<SpriteRenderer> ().sprite = splhand;
		limbs.Add (lefthand);
		rf_pos = new Vector2(rf_jnt.x, rf_jnt.y - (legreach - 0.5f));
		//rightfoot = createLimb (rf_pos, new Vector2(right, bottom), legreach, "rightfoot");
		rightfoot = GameObject.Find("rightfoot").GetComponent<Limb_Class>();
		rightfoot.init (rightfoot.transform.position, rf_pos, legreach, "rightfoot", this.gameObject);
		rightfoot.GetComponent<SpriteRenderer> ().sprite = sprfoot;
		limbs.Add (rightfoot);
		lf_pos = new Vector2(lf_jnt.x, lf_jnt.y - (legreach - 0.5f));
		//leftfoot = createLimb (lf_pos, new Vector2(left, bottom), legreach, "leftfoot");
		leftfoot = GameObject.Find("leftfoot").GetComponent<Limb_Class>();
		leftfoot.init (leftfoot.transform.position, lf_pos, legreach, "leftfoot", this.gameObject);
		leftfoot.GetComponent<SpriteRenderer> ().sprite = splfoot;
		limbs.Add (leftfoot);
		righthand.jnt = rh_jnt;
		lefthand.jnt = lh_jnt;
		rightfoot.jnt = rf_jnt;
		leftfoot.jnt = lf_jnt;
	}

	void lowertorso(){
		gameObject.transform.Translate (0, -0.1f, 0);
		updateposition ();
		Debug.Log ("Lowering my butt");
	}

	void movetotarget(){
		this.transform.position = Vector2.MoveTowards (this.transform.position, targetvar.transform.position, 0.1f);
		updateposition ();
	}




// Use this for initialization
	void Start () {
		updateposition(); //run once to set position variables
		moving = false;
		jumping = false;
		targetvar = null; //make sure there is no target
		//initialise reach parameters
		armreach = 1.5f;
		legreach = 2f;
		jumpreach = 5f;
		createlimbs (); //create limbs
		foreach (Limb_Class item in limbs) {
			item.update_pos ();
		}
		grabclosest ();
		disengage ();
		anim = GetComponent<Animator> ();
	}
		
	// Update is called once per frame
	void Update ()
	{
		//lowering the torso if arms are not stretched and torso isn't moving
		if ((!moving) && (!jumping) && (!lefthand.limbstretched ()) && (!righthand.limbstretched ())) {
			lowertorso ();
		}
		//moves the torso towards a target hold as soon as "moving" is set to true
		if ((moving) && (!jumping)) {
			//setMoveDir();
			updateposition ();
			if (selectlimb.check_reach (targetvar.transform.position)) {
				Debug.Log ("I'm reaching over!");
				moveSelectLimb();
				disengage();
				//if both axes are stretched and can't move, move back to start position
			} 
			else if (allstretch_y (gameObject) && allstretch_x (gameObject)) { //...if y is not stretched
				movetotarget();
				Debug.Log ("I'm moving my torso!");
					}

			else {
				this.transform.position = savedpos;
				disengage ();
				Debug.Log ("I'm giving up");
				}
			}
		//if we're jumping iterate the torso towards it and trigger grabclosest
		if ((!moving) && (jumping)) {
			//setMoveDir();
			updateposition ();
			if (Vector2.Distance(gameObject.transform.position, selectlimb.transform.position) > armreach){
				movetotarget();	
				Debug.Log ("I'm jumping");
			}
			else {
				grabclosest ();
				disengage ();

			}
		}

		}
	}




// CODE FOR STRENGTH CALCULATION
/*function iterates through limbs and calls their check_reach function, then adds those that are TRUE to a new list
calculates surface area for each limb, using the formula:
	Area = sqrt(s × (s - a) × (s - b) × (s - c))
		s = (a + b + c) / 2 

		float triangle (Limb_Class limb, List<Limb_Class> limblist){
	List<Vector2> coords = new List<Vector2> ();
	float s = new float();
	float area = new float ();
	foreach (Limb_Class item in limblist) {
		coords.Add (item.pos);
	}
	s = ((Vector2.Distance (coords [0], coords [1])) + (Vector2.Distance (coords [1], coords [2])) + (Vector2.Distance (coords [0], coords [2]))) / 2;
	area = Mathf.Sqrt (s * (s - Vector2.Distance (coords [0], coords [1])) * (s - Vector2.Distance (coords [1], coords [2])) * (s - Vector2.Distance (coords [0], coords [2])));
	return area;

} */