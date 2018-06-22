using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : UnitController {
	public List<Unit> squad;
	public List<Vector3> positions;
	bool positionsAssigned, playerSurrounded, campEstablished = false;
	Vector3 playerOriginalPosition;
	public int surroundRange, campDistance, commanderDistance, strikeDistance;
	public GameObject squadMate;

	// Use this for initialization
	void Start () {
		base.Start ();
		squad = new List<Unit> ();
		//spawn in squadMates
		SpawnInSquad();
		positions = new List<Vector3> ();
	}

	void SpawnInSquad(){
		GameObject s0 = GameObject.Instantiate (squadMate, unit.transform);
		s0.transform.localPosition  = new Vector3 (-15, 20, 0);
		squad.Add (s0.GetComponent<Unit>());

		GameObject s1 = GameObject.Instantiate (squadMate, unit.transform);
		s1.transform.localPosition  = new Vector3 (0, 20, 0);
		squad.Add (s1.GetComponent<Unit>());

		GameObject s2 = GameObject.Instantiate (squadMate, unit.transform);
		s2.transform.localPosition  = new Vector3 (15, 20, 0);
		squad.Add (s2.GetComponent<Unit>());

		GameObject s3 = GameObject.Instantiate (squadMate, unit.transform);
		s3.transform.localPosition  = new Vector3 (-15, -20, 0);
		squad.Add (s3.GetComponent<Unit>());

		GameObject s4 = GameObject.Instantiate (squadMate, unit.transform);
		s4.transform.localPosition  = new Vector3 (0, -20, 0);
		squad.Add (s4.GetComponent<Unit>());

		GameObject s5 = GameObject.Instantiate (squadMate, unit.transform);
		s5.transform.localPosition  = new Vector3 (16, -20, 0);
		squad.Add (s5.GetComponent<Unit>());

		GameObject s6 = GameObject.Instantiate (squadMate, unit.transform);
		s6.transform.localPosition  = new Vector3 (-20, -15, 0);
		squad.Add (s6.GetComponent<Unit>());

		GameObject s7 = GameObject.Instantiate (squadMate, unit.transform);
		s7.transform.localPosition  = new Vector3 (-20, 0, 0);
		squad.Add (s7.GetComponent<Unit>());

		GameObject s8 = GameObject.Instantiate (squadMate, unit.transform);
		s8.transform.localPosition  = new Vector3 (-20, 15, 0);
		squad.Add (s8.GetComponent<Unit>());

		GameObject s9 = GameObject.Instantiate (squadMate, unit.transform);
		s9.transform.localPosition  = new Vector3 (20, -15, 0);
		squad.Add (s9.GetComponent<Unit>());

		GameObject s10 = GameObject.Instantiate (squadMate, unit.transform);
		s10.transform.localPosition  = new Vector3 (20, 0, 0);
		squad.Add (s10.GetComponent<Unit>());

		GameObject s11 = GameObject.Instantiate (squadMate, unit.transform);
		s11.transform.localPosition  = new Vector3 (20, 15, 0);
		squad.Add (s11.GetComponent<Unit>());
	}
	
	// Update is called once per frame
	void Update () {
		bool ntr = NeedToRetreat ();
		if (ntr) {
			Retreat ();
		} else if (!playerSeen) {
			CheckForPlayer ();
		} else if (!campEstablished) {
			EstablishCamp ();
		} else if (!positionsAssigned) {
			AssignPositions ();
		} else if (!playerSurrounded) {
			SurroundPlayer ();
		} else {
			ChargePlayer ();
		}

		if (!ntr && campEstablished) {
			MaintainCommanderDistance ();
		}
		AimWeapons ();

	}

	void MoveSquadWithCommander(){
		for (int i = 0; i < squad.Count; i++) {
			squad [i].SetVelocity(unit.GetVelocity());
		}
	}

	void StopSquad(){
		for (int i = 0; i < squad.Count; i++) {
			squad [i].Stop ();
		}
	}
	void AimWeapons(){
		for (int i = 0; i < squad.Count; i++) {
			if (!squad [i].dead && squad[i].weapon != null) {
				squad [i].weapon.Aim (playerUnit.transform.position);
			}
		}
	}
	bool NeedToRetreat(){
		bool unit_alive = false;
		foreach(Unit u in squad){
			if (!u.dead) {
				unit_alive = true;
			}
		}
		return !unit_alive || unit.dead;
	}
	void MaintainCommanderDistance(){
        if (Vector3.Distance(playerUnit.transform.position, unit.transform.position) <= commanderDistance)
        {
            unit.MoveAway(playerUnit.transform.position);
        }
        else
        {
            unit.Stop();
        }
	}


	void AssignPositions(){
		playerOriginalPosition = playerUnit.transform.position;
		float degreePortion = 360f/squad.Count;
		float offset = Random.Range (-360, 360);
		for (float a = 0; a <= 360; a += degreePortion) {
			float xComp = (surroundRange * Mathf.Cos (a + offset * Mathf.PI / 180f)) + playerUnit.transform.position.x;
			float yComp = (surroundRange * Mathf.Sin (a + offset * Mathf.PI / 180f)) + playerUnit.transform.position.y;
			positions.Add (new Vector3 (xComp, yComp, 0));
		}
		positionsAssigned = true;
	}
	void SurroundPlayer(){
		int positionCount = 0;
		for (int i = 0; i < squad.Count; i++) {
			Vector3 goal = positions [i] - (playerOriginalPosition - playerUnit.transform.position);
			if (squad [i].dead) {
				positionCount++;
				squad.RemoveAt (i);
				i--;
			} else if (Vector3.Distance (squad [i].transform.position, goal) < 2 &&!squad[i].dead) {
				positionCount++;
				squad [i].SetVelocity (playerUnit.GetVelocity());
			} else {
				squad [i].MoveToward (goal);
			}
		}
		if (positionCount == squad.Count) {
			playerSurrounded = true;
			StopSquad ();
		}
	}
	void ChargePlayer(){
		for (int i = 0; i < squad.Count; i++) {
			if (Mathf.Abs(Vector3.Distance (squad [i].transform.position, playerUnit.transform.position) - strikeDistance) < .8f) {
				squad [i].AttackWithWeapon ();
				//squad [i].SetVelocity (playerUnit.GetVelocity());
				StartCoroutine(squad[i].PauseMovement(1));
			} else{
				squad [i].MoveToward (playerUnit.transform.position);
			}
		}
	}

	void EstablishCamp(){
		//Debug.Log ("Establishing Camp");
		unit.MoveToward (playerUnit.transform.position);
		MoveSquadWithCommander ();

		for (int i = 0; i < squad.Count; i++) {
			if (squad [i].dead) {
				squad [i].Stop ();
				squad [i].gameObject.transform.parent = null;
				squad.RemoveAt (i);
			}
		}

		if (Vector3.Distance (unit.transform.position, playerUnit.transform.position)<campDistance) {
			campEstablished = true;
			while (unit.transform.childCount > 1) {
				foreach (Transform child in unit.transform) {
					if (child.tag != "body") {
						child.parent = this.gameObject.transform;
						unit.Stop ();
						StopSquad ();
					}
				}
			}
		}
	}
	void Retreat(){
		unit.MoveAway (playerUnit.transform.position);
		for (int i = 0; i < squad.Count; i++) {
			squad [i].MoveAway (playerUnit.transform.position);
			if (Vector3.Distance (squad [i].transform.position, playerUnit.transform.position) > 100) {
				//squad [i].Die ();
				Destroy(squad[i]);
				squad.RemoveAt (i);
			}
		}
		if (Vector3.Distance (unit.transform.position, playerUnit.transform.position) > 100) {
			unit.Die ();
		}
	}
		
}