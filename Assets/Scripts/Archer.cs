﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : UnitController {
	public GameObject ArrowPrefab;
	public int attackRangeMin, attackRangeMax;
	bool choosePosition, inPosition, shooting, choosing = false;
	Vector3 position;
	public float repositionRestTime;

	// Use this for initialization
	void Start () {
		//playerUnit = GameObject.Find ("PlayerInputController").GetComponent<PlayerInputController> ().playerUnit;
		base.Start();
	}
	//choose random spot
	//shoot volley
	//repeat
	
	// Update is called once per frame

	void FixedUpdate () {
		base.FixedUpdate ();
		if(playerSeen){
			if (!choosePosition && !choosing) {
				StartCoroutine(ChoosePosition ());
			}else if(!inPosition && !choosing){
				MoveToPosition();
			}else if (!shooting && !choosing){
				StartCoroutine(FireVolley());
			}
		}	
		unit.AimWeapon (playerUnit.transform.position);
	}

	IEnumerator FireVolley(){
		shooting = true;
		unit.Stop ();
		int shots = Random.Range (1, 5);
		for (int i = 0; i < shots; i++) {
			unit.AttackWithWeapon (); //weapon.StartSwing ();
			//Shoot (playerUnit.transform.position + new Vector3(Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f)));
			yield return new WaitForSeconds(0.3f);
		}
		choosePosition = false;
		inPosition = false;
		shooting = false;
	}

	IEnumerator ChoosePosition(){
		choosing = true;
		yield return new WaitForSeconds (repositionRestTime);
		int negX = Random.Range (-1, 1);
		int negY = Random.Range (-1, 1);
		if(negX < 0){negX = -1;}else {negX = 1;}
		if(negY < 0){negY = -1;}else {negY = 1;}

		float xComp = playerUnit.transform.position.x + Random.Range (attackRangeMin,attackRangeMax) * negX; 
		float yComp = playerUnit.transform.position.y + Random.Range(attackRangeMin,attackRangeMax) * negY;
		position = new Vector3 (xComp, yComp, 0);
		choosing = false;
		choosePosition = true;
	}
		
	void Shoot(Vector3 position){
		if (!unit.dead) {
			Arrow arrow = GameObject.Instantiate (ArrowPrefab, unit.transform.position, Quaternion.identity).GetComponent<Arrow> ();
			Quaternion nRotation = Quaternion.LookRotation (Vector3.forward, position - unit.transform.position);
			arrow.myRotation = nRotation;
			Vector3 pos = (position - unit.transform.position);
			pos.Normalize ();
			arrow.SetVelocity (pos);
		}
	}

	void MoveToPosition(){
		unit.MoveToward (position);
		if (Vector3.Distance (unit.transform.position, position) < 1) {
			inPosition = true;
		}
	}
}
