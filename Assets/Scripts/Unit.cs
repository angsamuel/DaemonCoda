using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
	public GameObject blood;
	public GameObject wave;
	Color waveColor = Color.white;
	public PlayerInputController pic;
	NPC npc;
	public bool dead, inWaterLevel = false;
    public float speed, stamina, staminaMax, staminaRecharge, dashCost; 
    float waveDelay;
	public float dashMultiplier = 1.5f;
	public int health;
	bool invincible, dashLocked, inWater, waveRoutineLock = false;
	bool canTouchAttack = true;
	public Weapon weapon;
	Vector3 colliderPosition;
	Rigidbody2D rb;
	List<Collider2D> damageSources;
	Weapon pickup;
	public TrailRenderer tr;
	public GameObject body;

	void Start(){
		//Time.timeScale = 0.1f;
		StartCoroutine (WaveRoutine ());
		EquipWeapon (weapon);

		damageSources = new List<Collider2D> ();
		rb = gameObject.GetComponent<Rigidbody2D> ();
		staminaMax = stamina;
		StartCoroutine (PickupDeletion ());
		if (tr != null) {
			tr.time = 0.0f;
		}
	}

	IEnumerator PickupDeletion(){
		while(true){
			yield return new WaitForSeconds (.1f);
			pickup = null;
		}
	}

	public void DequipWeapon(){
		if (weapon != null) {
			weapon.StopSwing ();
			weapon.GetComponent<BoxCollider2D> ().enabled = true;
			weapon.equipped = false;
			weapon.transform.parent = null;
			weapon = null;
		}
	}
	public void EquipWeapon(Weapon w){
		if (w != null) {
			w.transform.position = transform.position;
			w.transform.parent = transform;
			weapon = w;
			weapon.equipped = true;
			weapon.GetComponent<BoxCollider2D> ().enabled = false;
		}
	}

	public void Equip(){
		DequipWeapon ();
		if (pickup != null) {
			Debug.Log ("have pickup");
			EquipWeapon (pickup);
		} else {
			Debug.Log ("no pickup");
		}
	}

	IEnumerator WaveRoutine(){
		if (wave != null && (inWater || inWaterLevel) && waveRoutineLock == false) {
			waveRoutineLock = true;
			Wave w = Instantiate (wave, transform.position + (new Vector3(0,-1.2f,0)), Quaternion.identity).GetComponent<Wave>();
			yield return new WaitForSeconds (waveDelay);
			waveRoutineLock = false;
			if (inWater || inWaterLevel) {
				StartCoroutine (WaveRoutine ());
			} 
		}
	}



	public IEnumerator PauseMovement(float time){
		float t = 0;
		while (t < 1) {
			t += Time.deltaTime / time;
			Stop ();
			yield return null;
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
		if (!dead && !dashLocked && stamina > dashCost) {
			stamina -= dashCost;
			dashLocked = true;
			StartCoroutine (DashRoutine ());
			return true;
		}
		return false;
	}

	float trMaxTime = 0.5f;
	float trailEndTime = 0.5f;
	IEnumerator EndTrail(){
		float t = trailEndTime;
		while (t > 0) {
			yield return null;
			t -= Time.deltaTime;
			tr.time = trMaxTime * (t/trailEndTime);
		}
	}

	IEnumerator Spin(float timeToSpin){
		float t = 0;
		int spinDirection = -1;
		if (GetComponent<Rigidbody2D> ().velocity.x < 0) {
			spinDirection = 1;
		}

		while (t < timeToSpin) {
			yield return null;
			float dt = Time.deltaTime;
			t += dt;
			body.transform.eulerAngles = new Vector3 (0, 0, t / timeToSpin * 360 * spinDirection);
		}
		body.transform.eulerAngles = new Vector3 (0, 0, 0);
	}

	public float dashTime = .25f;
	IEnumerator DashRoutine(){
		invincible = true;
		if (tr != null) {
			tr.time = trMaxTime;
		}
		if (tr != null) {
			StartCoroutine (EndTrail ());
		}
		speed = speed * dashMultiplier;
		StartCoroutine(Spin(dashTime));
		yield return new WaitForSeconds (dashTime);
		speed = speed / dashMultiplier;
		rb.velocity = rb.velocity / dashMultiplier;
		dashLocked = false;
		invincible = false;

	}
	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "water") {
			inWater = true;
			StartCoroutine(WaveRoutine());
		}

		if (pic != null && other.gameObject.tag == "weapon") {
			if (other.GetComponent<Weapon>().equipped == false) {
				pickup = other.GetComponent<Weapon> ();
			}
		} //phone message, email
		//voice mail in email, downloaded to listen

	}

	void OnTriggerEnter2D(Collider2D other) 
	{ 
		if (other.gameObject.tag == "damage source") {
			if (!damageSources.Contains (other)) {
				damageSources.Add (other);
				StartCoroutine (RemoveFromDamageSources (other));
				colliderPosition = other.transform.position;
				TakeDamage ();
			}
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
		DequipWeapon ();

		GetComponent<SpriteRenderer> ().color = Color.red;
		body.GetComponent<SpriteRenderer>().color = Color.red;
		dead = true;
		BoxCollider2D[] myColliders = gameObject.GetComponents<BoxCollider2D>();
		foreach(BoxCollider2D bc in myColliders) bc.enabled = false;

		transform.rotation = Quaternion.identity;
		inWater = false;
		inWaterLevel = false;
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
			if (weapon != null && weapon.IsRested ()) {
				stamina += staminaRecharge * Time.deltaTime;
			} else if (weapon == null) {
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

	public Rigidbody2D GetRB(){
		return rb;
	}

	public NPC GetNPC(){
		return npc;
	}
}