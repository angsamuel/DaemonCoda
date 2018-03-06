using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon {
	public GameObject target;
	public GameObject arrowPrefab;
	public override void StartSwing(){
		Shoot (target.transform.position + new Vector3(Random.Range(-.5f,.5f), Random.Range(-.5f,.5f),0));
	}

	void Shoot(Vector3 position){
		Arrow arrow = GameObject.Instantiate (arrowPrefab, transform.position, Quaternion.identity).GetComponent<Arrow> ();
			Quaternion nRotation = Quaternion.LookRotation (Vector3.forward, position - transform.position);
			arrow.myRotation = nRotation;
			Vector3 pos = (position - transform.position);
			pos.Normalize ();
			arrow.SetVelocity (pos/1.2f);
	}
}
