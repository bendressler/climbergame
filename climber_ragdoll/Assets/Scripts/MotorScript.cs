using UnityEngine;
using System.Collections;

public class MotorScript : MonoBehaviour {

	public float angle;
	public float maxangle;
	public float minangle;
	public float targetangle;
	public float force;
	public float reaction;
	public HingeJoint2D hinge;
	public JointMotor2D motor;

	// Use this for initialization
	void Start () {

		hinge = GetComponent<HingeJoint2D>();
		motor = hinge.motor;
	
	}

	// Update is called once per frame
	void Update () {

		angle = hinge.jointAngle;
		reaction = hinge.reactionTorque;

		if (!((angle < maxangle) && (angle > minangle))) {

			force = angle - targetangle;
			motor.motorSpeed = (0-force);
			hinge.motor = motor;
			hinge.useMotor = true;
		} else {
			hinge.useMotor = false;
		}
	}
}
