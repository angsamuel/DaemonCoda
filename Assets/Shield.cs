using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
	protected Vector3 myTarget;
	bool rested = true;
	float aimSpeed = 5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool IsRested(){
		return rested;
	}

	
	virtual public void Aim(Vector3 target){
		myTarget = target;
		if (IsRested()) {
			Quaternion nRotation = Quaternion.LookRotation (Vector3.forward, target - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, nRotation, Time.deltaTime * aimSpeed);
		}
	}

}
