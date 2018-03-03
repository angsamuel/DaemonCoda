using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duelist : UnitController {
	// Use this for initialization
	bool specialActionsEnabled = false;
	public float duelistDistanceMax = 20;
	public float duelistDistanceMin = 15;
	bool inSpecialAction = false;
	bool targetSeen = false;
	void Start () {
		base.Start ();
	}

	IEnumerator SelectSpecialAction(){
		yield return new WaitForSeconds (.15f);
			
			inSpecialAction = true;
			unit.Stop ();
			float secondsToWait = 1;
			int selection = Random.Range (0, 3);
			Debug.Log (selection);
			switch (selection) {
			case 0: //dodge move
				StartCoroutine (DashRandom ());
				secondsToWait = 1.15f;
				break;
			case 1: //swing at player 
				unit.AttackWithWeapon ();
				secondsToWait = .94f;
				break;
			case 2: //approach player and swing (berserker behavior
				unit.Dash ();
				Charge ();
				secondsToWait = 1.5f;
				break;
			default:
				break;
			}
			yield return new WaitForSeconds (secondsToWait);
			inSpecialAction = false;
			unit.Stop ();
			yield return new WaitForSeconds (.15f);

		StartCoroutine(SelectSpecialAction ());
	}

	void Charge(){
		unit.MoveToward (playerUnit.transform.position);
		unit.SetVelocity (unit.GetVelocity ());
	}

	IEnumerator DashRandom(){
		Vector2 dashLocation = new Vector2 (transform.position.x + Random.Range (-100.0f, 100.0f), transform.position.y + Random.Range (-100.0f, 100.0f));
		unit.MoveToward (dashLocation);
		yield return new WaitForSeconds (.5f);
		unit.Dash ();
		unit.MoveToward (dashLocation);
	}
	void MaintainDistance(){
		if (Vector3.Distance (playerUnit.transform.position, unit.transform.position) <= duelistDistanceMin) {
			unit.MoveAway (playerUnit.transform.position);
		} else if (Vector3.Distance (playerUnit.transform.position, unit.transform.position) >= duelistDistanceMax) {
			unit.MoveToward (playerUnit.transform.position);
		} else {
			unit.Stop ();
		}
	}
	// Update is called once per frame
	void Update () {
		base.Update ();


		if (!unit.dead) {
			if (playerSeen && !specialActionsEnabled) {
				StartCoroutine(SelectSpecialAction ());
				specialActionsEnabled = true;
			}
			if (!inSpecialAction && playerSeen) {
				MaintainDistance ();
			}
				unit.weapon.Aim (playerUnit.transform.position);
			
			if (Vector3.Distance (unit.transform.position, playerUnit.transform.position) < strikingDistance) {
				unit.AttackWithWeapon ();
				if (!inSpecialAction) {
					unit.Stop ();
				}
			}
		}
	}
}
