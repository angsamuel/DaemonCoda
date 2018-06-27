using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duelist : UnitController {
	// Use this for initialization
	bool specialActionsEnabled = false;
	public float duelistDistanceMax = 12;
	public float duelistDistanceMin = 8;
	bool inSpecialAction = false;
	bool targetSeen = false;
	void Start () {
		base.Start ();
	}

	IEnumerator SelectSpecialAction(){
        inSpecialAction = true;
	    yield return null;
		if (!unit.dead && target != null) {
            Debug.Log("not dead");
			
			inSpecialAction = true;
			unit.Stop ();
			float secondsToWait = 1;
			int selection = Random.Range (0, 5);
			//Debug.Log (selection);
			switch (selection) {
			case 0: //dodge move
				StartCoroutine (DashRandom ());
				secondsToWait = .1f;
				break;
			case 1: //swing at player 
				unit.AttackWithWeapon ();
				secondsToWait = .1f;
				break;
			case 3: //approach player and swing (berserker behavior)
				unit.Dash ();
				Charge ();
				secondsToWait = 1f;
				break;
            case 4: //approach player and swing (berserker behavior)
                unit.Dash();
                Charge();
                secondsToWait = 1f;
                break;
            default:
			break;
			}
			yield return new WaitForSeconds (secondsToWait);
			inSpecialAction = false;
			unit.Stop ();
			yield return new WaitForSeconds (.25f);
		}
	}

	void Charge(){
		unit.MoveToward (target.transform.position);
		unit.SetVelocity (unit.GetVelocity ());
	}

	IEnumerator DashRandom(){
		Vector2 dashLocation = new Vector2 (transform.position.x + Random.Range (-100.0f, 100.0f), transform.position.y + Random.Range (-100.0f, 100.0f));
		unit.MoveToward (dashLocation);
		yield return new WaitForSeconds (.1f);
		unit.Dash ();
		unit.MoveToward (dashLocation);
	}
	void MaintainDistance(){
		if (Vector3.Distance (target.transform.position, unit.transform.position) <= duelistDistanceMin) {
			unit.MoveAway (target.transform.position);
		} else if (Vector3.Distance (target.transform.position, unit.transform.position) >= duelistDistanceMax) {
			unit.MoveToward (target.transform.position);
		} else {
			unit.Stop ();
		}
	}
	// Update is called once per frame
	void Update () {
		base.Update ();
	}

    override public void CustomActions()
    {
        if (!unit.dead && target != null)
        {
            if (target != null && !inSpecialAction)
            {
                StartCoroutine(SelectSpecialAction());
            }
            if (!inSpecialAction && target != null)
            {
                MaintainDistance();
            }
            unit.weapon.Aim(target.transform.position);

            if (Vector3.Distance(unit.transform.position, target.transform.position) < strikingDistance)
            {
                unit.AttackWithWeapon();
                if (!inSpecialAction)
                {
                    unit.Stop();
                }
            }
        }
    }
}
