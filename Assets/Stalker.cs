using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : UnitController {
	public Color hostile;
	public Color sleeping;

	bool vibrating = true;
	BoxCollider2D[] colls;
	// Use this for initialization
	void Start () {
		base.Start();
		StartCoroutine(VibrationRoutine());
		colls = gameObject.GetComponents<BoxCollider2D>();



		Color c= unit.body.GetComponent<SpriteRenderer>().color;
		unit.body.GetComponent<SpriteRenderer>().color = sleeping;
		vibrating = false;
		colls[1].enabled = false;
		unit.invincible = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		//base.LateUpdate();
		//StartCoroutine(WanderRoutine());
		//unit.AimWeapon(transform.position - new Vector3(0,100,0));
		//unit.MoveToward(playerUnit.transform.position);

		if(!inMachine){
			StartCoroutine(StalkerStateMachine());
		}	
	}
	bool inMachine = false;
	string mode = "sleep";

	float fastSpeed = 25;
	float excitedSpeed = 2.5f;
	float driftSpeed = 1.5f;
	float dangerDistance = 6;
	float excitedDistance = 20;
	float triggerDistance = 5;


	IEnumerator StalkerStateMachine(){
		inMachine = true;
		if(!unit.dead){
			if(mode == "sleep"){
				yield return new WaitForSeconds(.5f);
				unit.InstantAimWeapon(transform.position + new Vector3(0,-100,0));
				if(TargetInSight(playerUnit,triggerDistance)){
					mode = "default";
					vibrating = true;
					unit.body.GetComponent<SpriteRenderer>().color = new Color(hostile.r,hostile.g,hostile.b,.5f);
					yield return new WaitForSeconds(10f);
				}
			}
			if(mode == "default"){
				unit.body.GetComponent<SpriteRenderer>().color = new Color(hostile.r,hostile.g,hostile.b,.5f);
				vibrating = true;
				colls[1].enabled = false;
				unit.invincible = true;
				mode = "drifting";
			}
			else if(mode == "drifting"){
				unit.AimWeapon(transform.position + new Vector3(0,-100,0));
				colls[1].enabled = false;
				unit.speed = driftSpeed;
				unit.MoveToward(playerUnit.transform.position);
				vibrating = true;
				if(Vector3.Distance(transform.position, playerUnit.transform.position) < excitedDistance){
					unit.invincible = false;
					unit.speed = fastSpeed;
					mode = "excited";
				}
			}else if(mode == "excited"){
				unit.speed = excitedSpeed;
				unit.MoveToward(playerUnit.transform.position);
				unit.AimWeapon(transform.position + new Vector3(0,-100,0));
				if(Vector3.Distance(transform.position, playerUnit.transform.position) < dangerDistance){
					unit.invincible = false;
					unit.speed = fastSpeed;
					unit.body.GetComponent<SpriteRenderer>().color = hostile;
					mode = "attack";
				}else if(Vector3.Distance(transform.position, playerUnit.transform.position) > excitedDistance){
					mode = "default";
					Debug.Log("back to default");
				}	
			}else if(mode == "attack"){
				unit.AimWeapon(playerUnit.transform.position);
				if(Vector3.Distance(transform.position, playerUnit.transform.position) <= strikingDistance){
					unit.Stop();
					unit.AttackWithWeapon();
					unit.PauseMovement(1.15f);
					Vector2 telePosition = new Vector2(transform.position.x + Random.Range(-10,10), transform.position.y+ Random.Range(-10,10));
					unit.AimWeapon(telePosition);
					saim = true;
					StartCoroutine(SpecialAim(telePosition));
					yield return new WaitForSeconds(1.15f);
					saim = false;
					if(!unit.dead){
						transform.position = telePosition;
						yield return new WaitForSeconds(.5f);
					}
					mode = "default";
				}else{
					unit.MoveToward(playerUnit.transform.position);
				}
			}
			inMachine = false;
		}else{
			unit.body.GetComponent<SpriteRenderer>().color = hostile;
		}
		
	}

	bool saim = false;
	IEnumerator SpecialAim(Vector2 pos){
		while(saim){
			unit.AimWeapon(pos);
			yield return null;
		}
	}

	IEnumerator VibrationRoutine(){
		while(true){
			if(vibrating){
				Vibrate();
				vibrating = !unit.dead;
			}
			yield return new WaitForSeconds(.025f);
		}
	}

	void Vibrate(){
		transform.Translate(new Vector2(Random.Range(-.15f, .15f), Random.Range(-.15f, .15f)));
	}

}
