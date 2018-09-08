using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		Aim(Camera.main.ScreenToWorldPoint(Input.mousePosition));	
	}

	public void Aim(Vector3 target){
		Quaternion nRotation = Quaternion.LookRotation (Vector3.forward, target - transform.position);
		transform.rotation = nRotation;// Quaternion.Slerp (transform.rotation, nRotation, Time.deltaTime * aimSpeed);

	}
}
