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
		//Debug.Log("action");
		if (!unit.dead && target != null) {
			
			inSpecialAction = true;
			unit.Stop ();
			float secondsToWait = 2;
			int selection = Random.Range (0, 4);

			if(selection == 0){
				StartCoroutine (DashRandom ());
				secondsToWait = 1f;
			}else{
				Charge();
				unit.Dash();
				secondsToWait = 2f;
			}
			yield return new WaitForSeconds (secondsToWait);
			unit.Stop ();
			yield return new WaitForSeconds (.25f);
			inSpecialAction = false;

			
		}
	}

	void Charge(){
		unit.MoveToward (target.transform.position);
		unit.SetVelocity (unit.GetVelocity ());
	}

	IEnumerator DashRandom(){
		Vector2 dashLocation = new Vector2 (transform.position.x + Random.Range (-100.0f, 100.0f), transform.position.y + Random.Range (-100.0f, 100.0f));
		unit.MoveToward (dashLocation);
		yield return new WaitForSeconds (.4f);
		unit.Dash ();
		unit.MoveToward (dashLocation);
	}
	bool MaintainDistance(){
		if (Vector3.Distance (target.transform.position, unit.transform.position) <= duelistDistanceMin) {
			unit.MoveAway (target.transform.position);
			return false;
		} else if (Vector3.Distance (target.transform.position, unit.transform.position) >= duelistDistanceMax) {
			unit.MoveToward (target.transform.position);
			return false;
		} else {
			unit.Stop ();
			return true;
		}
	}
	// Update is called once per frame
	void LateUpdate () {
		base.LateUpdate ();
	}
	float weaponCooldown = 2f;

	bool canAttack = true;

	IEnumerator WeaponCooldown(){
		canAttack = false;
		yield return new WaitForSeconds(weaponCooldown);
		canAttack = true;
	}

    override public void CustomActions()
    {
        if (!unit.dead && target != null)
        {
			
            if (target != null && !inSpecialAction && MaintainDistance())
            {
                StartCoroutine(SelectSpecialAction());
            }
            // if (!inSpecialAction && target != null)
            // {
            //     MaintainDistance();
            // }
           

            if (Vector3.Distance(unit.transform.position, target.transform.position) < strikingDistance)
            {
				if(canAttack){
					unit.AttackWithWeapon();
					StartCoroutine(WeaponCooldown());
				}
                if (!inSpecialAction)
                {
                    //unit.Stop();
                }
            }
        }
    }
}
