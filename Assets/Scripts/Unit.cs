using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public string team;
	public int armoryIndex;
	public GameObject blood;
	public SpriteRenderer healOverlay;
	public GameObject wave;
	Color waveColor = Color.white;
	public PlayerInputController pic;
	NPC npc;
    public bool dead = false;
    bool inWaterLevel = false;
    public float speed, stamina, staminaMax, staminaRecharge, dashCost; 
	float staminaDelayTime = 1.5f;
	float staminaDepletionDelayTime = 3.0f;
    float waveDelay;
	public float dashMultiplier = 1.5f;
	public int health;
	[HideInInspector] public bool invincible, dashLocked, inWater, waveRoutineLock = false;
	bool canTouchAttack = true;
	public Weapon weapon;
	public Shield shield;
	Vector3 colliderPosition;
	Rigidbody2D rb;
	List<int> damageSources;
	Weapon pickup;
	public TrailRenderer tr;
	public GameObject body;
	SpriteRenderer sr;

    float scanRange = 3.0f;

    Coroutine staminaDelayRoutine;
    bool canRecharge = true;

    public bool isPlayerUnit;
    int medPaks = 0;
    int mealPaks = 0;

    void Start(){
		sr = body.GetComponent<SpriteRenderer>();
		damageSources = new List<int> ();
		//Time.timeScale = 0.1f;
		StartCoroutine (WaveRoutine ());
		EquipWeapon (weapon);

		
		rb = gameObject.GetComponent<Rigidbody2D> ();
		staminaMax = stamina;
		StartCoroutine (PickupDeletion ());
		if (tr != null) {
			tr.time = 0.0f;
		}

		//spawn new weapon for playerUnit
		if(weapon == null){
			//spawn weapon according to index
			if(isPlayerUnit){
				armoryIndex = PlayerPrefs.GetInt(PlayerPrefs.GetString("profile") + "armoryIndex");
				medPaks = PlayerPrefs.GetInt(PlayerPrefs.GetString("profile"));
			}
			if(armoryIndex != -1){
				Armory armory = GameObject.Find("Armory").GetComponent<Armory>();
				GameObject newWeapon = Instantiate(armory.weapons[armoryIndex], transform);
				EquipWeapon(newWeapon.GetComponent<Weapon>());
			}
		}


		if(weapon != null && isPlayerUnit){
			PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "armoryIndex", weapon.armoryIndex);
		}else if(isPlayerUnit){
			PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "armoryIndex", -1);
		}

		if(isPlayerUnit){
			mealPaks = PlayerPrefs.GetInt(PlayerPrefs.GetString("profile") + "mealPaks");
			medPaks = PlayerPrefs.GetInt(PlayerPrefs.GetString("profile") + "medPaks");
		}
	}
	public bool WeaponRested(){
		return (weapon == null || weapon.rested == true);
	}

    public void UseStamina(float cost)
    {
        stamina -= cost;
        if(stamina < 0)
        {
            stamina = 0;
        }

        if (staminaDelayRoutine != null)
        {
            StopCoroutine(staminaDelayRoutine);
        }

        staminaDelayRoutine = StartCoroutine(DelayStamina());
    }

    IEnumerator DelayStamina()
    {
        canRecharge = false;
		if(stamina < 1){
			yield return new WaitForSeconds(staminaDepletionDelayTime);
		}else{
        	yield return new WaitForSeconds(staminaDelayTime);
		}
        canRecharge = true;
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
			//Debug.Log ("have pickup");
			EquipWeapon (pickup);
			if(isPlayerUnit){
				PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "armoryIndex", pickup.armoryIndex);
			}


		} else {
			//Debug.Log ("no pickup");
			if(isPlayerUnit){
				PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "armoryIndex", -1);
			}
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

    void SpriteCheck()
    {
        if (!dead)
        {
            if (rb.velocity.x > 0)
            {
                body.transform.localScale = new Vector2(-Mathf.Abs(body.transform.localScale.x), body.transform.localScale.y);
            }
            else if (rb.velocity.x < 0)
            {
                body.transform.localScale = new Vector2(Mathf.Abs(body.transform.localScale.x), body.transform.localScale.y);
            }
        }
    }

	bool canMove = true;
	public void PauseMovement(float time){
		if(canMove){
			StartCoroutine(PauseMovementRoutine(time));
		}
	}
	IEnumerator PauseMovementRoutine(float time){
		float t = 0;
		canMove = false;
		Stop();
		yield return new WaitForSeconds(time);
		canMove = true;
	}

	public void Move(float x, float y){
		if (!dead && canMove) {
			rb.velocity = new Vector2(x*speed, y*speed);
		}

        
	}

	public void MoveToward(Vector3 position){
		if (!dead && canMove) {
			rb.velocity = UBP(position, transform.position) * speed;
		}
	}

	public void MoveAway(Vector3 position){
		if (!dead && rb!=null && canMove) {
			if(transform.position != position){
				rb.velocity = UBP(transform.position, position) * speed;
			}
		}
	}

	public bool Dash(){
		if (!dead && !dashLocked && stamina > 0) {
            UseStamina(dashCost);
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

	float dashTime = .5f;
	IEnumerator DashRoutine(){
		invincible = true;
		if (tr != null) {
			tr.time = trMaxTime;
		}
		if (tr != null) {
			StartCoroutine (EndTrail ());
		}
		//speed = speed * dashMultiplier;
		StartCoroutine(Spin(dashTime));
		yield return new WaitForSeconds (dashTime);
		//speed = speed / dashMultiplier;
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
		} 
	}


    void OnTriggerEnter2D(Collider2D other) 
	{

        //pickup medPaks, foodPaks, muns
        if (isPlayerUnit)
        {
            if(other.gameObject.tag == "med pak" && canPickupMedPak)
            {
                Destroy(other.gameObject);
				StartCoroutine(GetMedPakTimer());
                medPaks += 1;
				PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "medPaks", medPaks);
            }else if(other.gameObject.tag == "meal pak" && canPickupMealPak)
            {
                Destroy(other.gameObject);
				StartCoroutine(GetMealPakTimer());
                mealPaks += 1;
				PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "mealPaks", mealPaks);
            }
        }



		if (other.gameObject.tag == "damage source") {
			if (damageSources != null && !damageSources.Contains (other.gameObject.GetComponent<Blade>().id)) {
				damageSources.Add (other.gameObject.GetComponent<Blade>().id);
				StartCoroutine (RemoveFromDamageSources (other.gameObject.GetComponent<Blade>().id));
				colliderPosition = other.transform.position;
				if(weapon == null || weapon.gameObject.transform.GetChild(0).GetComponent<Blade>().id != other.gameObject.GetComponent<Blade>().id){
					TakeDamage ();
				}
			}
		} else if (other.gameObject.tag == "npc") {
			npc = other.GetComponent<NPC> ();
		} else if (other.gameObject.tag == "water") {
			inWater = true;
			StartCoroutine(WaveRoutine());
		}
	}

	IEnumerator RemoveFromDamageSources(int o){
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

		sr.color = Color.red;
		dead = true;
		BoxCollider2D[] myColliders = gameObject.GetComponents<BoxCollider2D>();

        GetComponent<Rigidbody2D>().freezeRotation = true;
        GetComponent<Rigidbody2D>().velocity *= 0;

        transform.rotation = Quaternion.identity;
		inWater = false;
		inWaterLevel = false;
		Stop ();
	}
	bool canAttack = true;
	Coroutine par;
	public void PauseAttack(float s){
		if(canAttack == true){
			par = StartCoroutine(PauseAttackRoutine(s));
		}
	}
	IEnumerator PauseAttackRoutine(float s){
		canAttack = false;
		yield return new WaitForSeconds(s);
		canAttack = true;
	}

	public void AttackWithWeapon(){
		//Debug.Log(canAttack);
		if (!dead && weapon != null && weapon.IsRested () && stamina > 0 && canAttack) {
			weapon.StartSwing ();
            UseStamina(weapon.staminaCost);
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

    public void Stop(float delay)
    {
        StartCoroutine(DelayedStop(delay));
    }

    IEnumerator DelayedStop(float delay)
    {
        yield return new WaitForSeconds(delay);
        Stop();
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
		if(weapon!= null){
			weapon.transform.position = transform.position;
		}

		if(!dead){

            SpriteCheck();
			transform.rotation = Quaternion.identity;
            RechargeStamina();
		}
	}

    void RechargeStamina()
    {
        if (canRecharge)
        {
            if (weapon != null && weapon.IsRested() && (shield == null || shield.IsRested()))
            {
                stamina += staminaRecharge * Time.deltaTime;
            }
            else if (weapon == null)
            {
                stamina += staminaRecharge * Time.deltaTime;
            }
        }
        if (stamina > staminaMax)
        {
            stamina = staminaMax;
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

	public void InstantAimWeapon(Vector3 position){
		if (weapon != null && !dead) {
			weapon.InstantAim (position);
		}
	}

	public void AimShield(Vector3 position){
		if (shield != null && !dead && stamina > 0) {		
			shield.Aim (position);
		}else{
			LowerShield();
		}
	}

	public void LowerShield(){
		shield.Lower();
	}

	public void DropWeapon(){
		if (weapon != null) {
			weapon.tag = "Untagged";
			weapon.transform.parent = null;
			weapon = null;
		}
	}

	Vector3 UBP(Vector3 p1, Vector3 p2){
		if((p1 == p2)){
			new Vector3(0,0);
		}
		return (1f / (p1 - p2).magnitude) * (p1 - p2);
	}

	public Rigidbody2D GetRB(){
		return rb;
	}

	public NPC GetNPC(){
		return npc;
	}

    public void RaycastToTarget(GameObject target) 
    {
        /*
        Debug.DrawLine(transform.position + new Vector3(2,2,2),new Vector3(0,0,0));
        if (Physics2D.Linecast(transform.position + new Vector3(2, 2, 2), new Vector3(0, 0, 0)))
        {
            Debug.Log("spotted!");
        }
        */

        

       //RaycastHit2D[] hits;
        List<RaycastHit2D> hits;

        hits = new List<RaycastHit2D>(Physics2D.LinecastAll(transform.position, target.transform.position));
        for(int i = 0; i<hits.Count; i++)
        {
            if(hits[i].transform.tag == "weapon" || hits[i].transform.tag == "damage source")
            {
                hits.RemoveAt(i);
                i = i - 1;
            }
        }
        

        
       if(hits.Count > 2 && hits[2].transform.gameObject == target)
       {
           //Debug.Log("spotted " + hits.Count);
           Debug.DrawLine(transform.position, target.transform.position, Color.green);
        }
        else
        {
            Debug.DrawLine(transform.position, target.transform.position, Color.red);
        }
    }

	bool canPickupMedPak = true;
	IEnumerator GetMedPakTimer(){
		canPickupMedPak = false;
		yield return new WaitForSeconds(.1f);
		canPickupMedPak = true;
	}

	bool canPickupMealPak = true;
	IEnumerator GetMealPakTimer(){
		canPickupMealPak = false;
		yield return new WaitForSeconds(.1f);
		canPickupMealPak = true;
	}

	public int GetMedPaks(){
		return medPaks;
	}

	public int GetMealPaks(){
		return mealPaks;
	}

	public bool healing = false;
	float healTime = 2.0f;
	public void Heal(){
		if(!healing && medPaks > 0 && health < 3){
			rb.velocity = new Vector3(0,0,0);
			StartCoroutine(HealRoutine());
		}
	}

	IEnumerator HealRoutine(){
		healing = true;
		healOverlay.enabled = true;
		healOverlay.transform.localScale = new Vector2(2.5f, 2.5f);
		float timeLeft = healTime;
		while(timeLeft > 0){
			timeLeft -= Time.deltaTime;
			healOverlay.transform.localScale = new Vector2(timeLeft / healTime, timeLeft / healTime) * 2.5f;
			yield return null;
		}
		healing = false;
		if(health < 3){
			health += 1;
			medPaks -= 1;
		}
		healOverlay.enabled = false;
	}


}