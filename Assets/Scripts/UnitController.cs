using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {
	protected Unit playerUnit;
	public Unit unit;
	LevelController levelController;
	public int strikingDistance;
	public int seeingDistance;
	protected bool playerSeen = false;

	// Use this for initialization
	public void Start () {
		levelController = GameObject.Find ("LevelController").GetComponent<LevelController> ();
		levelController.AddEnemyController (this);
		playerUnit = GameObject.Find ("PlayerInputController").GetComponent<PlayerInputController> ().playerUnit;
	}
	
	// Update is called once per frame
	public void Update () {
		CheckForPlayer ();
	}

	protected void CheckForPlayer(){
		if (!playerSeen) {
			if (Vector3.Distance (playerUnit.transform.position, unit.transform.position) < seeingDistance) {
				playerSeen = true;
				levelController.AlertNearbyEnemies (transform.position);

			}
		}
	}

	public void Alert(){
		playerSeen = true;
	}
}
