using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
	public GameObject blood;
	public GameObject wave;
	Color waveColor = Color.white;
	public PlayerInputController pic;
	NPC npc;
	public bool dead = false;
	public float speed, dashMultiplier, stamina, staminaMax, staminaRecharge, dashCost;
	float waveDelay = 0.5f;
	public int health;
	bool invincible, dashLocked, inWater, waveRoutineLock = false;
	bool canTouchAttack = true;
	public Weapon weapon;
	Vector3 colliderPosition;
	Rigidbody2D rb;
	List<Collider2D> damageSources;

	void Start(){
		damageSources = new List<Collider2D> ();
		rb = gameObject.GetComponent<Rigidbody2D> ();
		staminaMax = stamina;
	}

	IEnumerator WaveRoutine(){
		if (wave != null && inWater && waveRoutineLock == false) {
			waveRoutineLock = true;
			Wave w = Instantiate (wave, transform.position, Quaternion.identity).GetComponent<Wave>();
			yield return new WaitForSeconds (waveDelay);
			waveRoutineLock = false;
			if (inWater) {
				StartCoroutine (WaveRoutine ());
			}
		}
	}

	public void Move(float x, float y){
		if (!dead) {
			rb.velocity = new Vector2(x*speed, y*speed);
		}
	}

	public void MoveToward(Vector3 position){
		if (!dead) {
			rb.velocity = UBP(position, transform.position) * speed;
		}
	}

	public void MoveAway(Vector3 position){
		if (!dead) {
			rb.velocity = UBP(transform.position, position) * speed;
		}
	}

	public bool Dash(){
	   if (!dashLocked && stamina > dashCost) {
			stamina -= dashCost;
			dashLocked = true;
			StartCoroutine (DashRoutine ());
			return true;
		}
		return false;
	}

	IEnumerator DashRoutine(){
		invincible = true;
		speed = speed * 5;
		yield return new WaitForSeconds (.1f);
		speed = speed / 5;
		dashLocked = false;
		invincible = false;
	}

	void OnTriggerEnter2D(Collider2D other) 
	{ 
		if (other.gameObject.tag == "damage source") {
			Debug.Log ("collider enter");
			if (!damageSources.Contains (other)) {
				damageSources.Add (other);
				StartCoroutine (RemoveFromDamageSources (other));
				colliderPosition = other.transform.position;
				TakeDamage ();
			}
		} else if (other.gameObject.tag == "weapon") {
			other.transform.parent = this.transform;
		} else if (other.gameObject.tag == "npc") {
			npc = other.GetComponent<NPC> ();
		} else if (other.gameObject.tag == "water") {
			inWater = true;
			StartCoroutine(WaveRoutine());
		}
	}

	IEnumerator RemoveFromDamageSources(Collider2D o){
		yield return new WaitForSeconds(0.2f);
		damageSources.Remove (o);
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "npc") {
			npc = null;
		}if (other.gameObject.tag == "water") {
			inWater = false;
		}
	}



	public void Die(){
		GetComponent<SpriteRenderer> ().color = Color.red;
		dead = true;
		BoxCollider2D[] myColliders = gameObject.GetComponents<BoxCollider2D>();
		foreach(BoxCollider2D bc in myColliders) bc.enabled = false;

		transform.rotation = Quaternion.identity;
		inWater = false;
		Stop ();
	}
	public void AttackWithWeapon(){
		if (!dead && weapon != null && weapon.IsRested () && stamina > weapon.staminaCost) {
			weapon.StartSwing ();
			stamina -= weapon.staminaCost;
		}
	}
	public void TakeDamage(){
		if (!dead && !invincible) {
			if (pic != null) {
				pic.DamageEffect ();
			}
			health--;
			if(blood!=null){
				Vector3 u = UBP (colliderPosition, transform.position);
				Instantiate (blood, transform.position, Quaternion.Euler (new Vector3 (Random.Range (0, 360), 90, 0)));
			}
			if (health < 1) {
				Die ();
			} else {
				//StartCoroutine (InvisiTimer ());
			}
		}

	}

	public void Stop(){
		rb.velocity = new Vector2(0, 0);
	}
	public void SetVelocity(Vector2 vel){
		rb.velocity = vel;
	}
	public Vector2 GetVelocity(){
		return rb.velocity;
	}

	IEnumerator InvisiTimer(){
		invincible = true;
		yield return new WaitForSeconds(.005f);
		invincible = false;
	}

	void Update(){
		if(!dead){
			transform.rotation = Quaternion.identity;
			if(weapon != null && weapon.IsRested()){
				stamina += staminaRecharge * Time.deltaTime;
			}
			if (stamina > staminaMax) {
				stamina = staminaMax;
			}
		}
	}
	public void TouchAttack(Unit u){
		if (canTouchAttack && !dead && Vector3.Distance(transform.position, u.transform.position) < 1) {
			u.TakeDamage ();
			StartCoroutine (TouchAttackCooldown ());
		}
	}

	IEnumerator TouchAttackCooldown(){
		canTouchAttack = false;
		yield return new WaitForSeconds(1);
		canTouchAttack = true;
	}

	public void AimWeapon(Vector3 position){
		if (weapon != null && !dead) {
			weapon.Aim (position);
		}
	}

	public void DropWeapon(){
		if (weapon != null) {
			weapon.tag = "Untagged";
			weapon.transform.parent = null;
			weapon = null;
		}
	}

	Vector3 UBP(Vector3 p1, Vector3 p2){
		return (1f / (p1 - p2).magnitude) * (p1 - p2);
	}

	public NPC GetNPC(){
		return npc;
	}
}