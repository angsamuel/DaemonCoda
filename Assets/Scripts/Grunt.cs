using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : UnitController {
    public float backupDistance;
	// Use this for initialization
	void Start () {
		base.Start ();
	}



	void Update () {
		base.Update ();
        if (playerSeen)
        {
            if (!unit.dead && unit.weapon != null)
            {
                unit.weapon.Aim(playerUnit.transform.position);
                float distance = Vector3.Distance(unit.transform.position, playerUnit.transform.position);
                if (distance < strikingDistance && distance > backupDistance)
                {
                    unit.AttackWithWeapon();
                    StartCoroutine(unit.PauseMovement(.5f));
                    unit.Stop();
                }
                else if (Vector3.Distance(unit.transform.position, playerUnit.transform.position) <= backupDistance)
                {
                    unit.MoveAway(playerUnit.transform.position);
                    
                }
                else
                {
                    unit.MoveToward(playerUnit.transform.position);
                }
            }
        }
	}
		


}
