using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerInputController : MonoBehaviour {
	public Unit playerUnit;
	public GameObject redOverlay;
	public float pulseTime;
	public Camera mainCamera;
	public bool aim_enabled = true;
	bool dashLock, disabled, canAttack, canPickup, canCastSpell = false;

	public GameObject staminaBar;
	public HPDisplay hpDisplay;
	public StaminaDisplay staminaDisplay;
	int hpIndex;

    public bool enabeled;

	public bool loadColor = true;


	// Use this for initialization
	void Start () {
		hpIndex = 2;
		redOverlay.SetActive(false); 
		StartCoroutine (EndlessEnforcer ());

		//set player color
		string profile = PlayerPrefs.GetString("profile");
		if(loadColor){
			
		}
	}

	void EquipWeapon(){
		playerUnit.Equip ();
	}

	IEnumerator EndlessEnforcer(){
		while (true) {
			if (playerUnit.transform.position.x > 2000) {
				playerUnit.transform.Translate (-200, 0, 0);
			} else if (playerUnit.transform.position.x < -2000) {
				playerUnit.transform.Translate (-200, 0, 0);
			}

			if (playerUnit.transform.position.y > 2000) {
				playerUnit.transform.Translate (0, -200, 0);
			} else if (playerUnit.transform.position.y < -2000) {
				playerUnit.transform.Translate (0, 200, 0);
			}
			yield return new WaitForSeconds (1f);
		}
	} 

	// Update is called once per frame
	bool blocking = false;
	void LateUpdate () {
		if(enabeled){
		//Dash
		if (!disabled && !playerUnit.healing) {
			//weapon pickup
			if (Input.GetAxisRaw ("Pickup") != 0 && canPickup) {
				EquipWeapon ();
				canPickup = false;
			} else if(Input.GetAxisRaw ("Pickup") == 0){
				canPickup = true;
			}
			if (Input.GetAxisRaw ("Dash") != 0 && !dashLock) {
				playerUnit.Dash ();
				dashLock = true;
			} else if (Input.GetAxisRaw ("Dash") == 0) {
				dashLock = false;
			}

			//Movement
			float hMovement = Input.GetAxis ("Horizontal");
			float vMovement = Input.GetAxis ("Vertical");
			playerUnit.Move (hMovement, vMovement);
			mainCamera.transform.position = new Vector3 (playerUnit.transform.position.x, playerUnit.transform.position.y, mainCamera.transform.position.z);
			
			//Weapon aiming
			if (Input.GetAxisRaw ("Attack") != 0 && canAttack && !blocking) {
				Attack ();
				canAttack = false;
			} else if (Input.GetAxisRaw ("Attack") == 0) {
				canAttack = true;
			}

			//action
			if (Input.GetAxisRaw ("Action") != 0) {
				TalkToNPC ();
			}

			//playerUnit.weapon.Aim (Camera.main.ScreenToWorldPoint (Input.mousePosition));
			
			if(Input.GetAxisRaw("RaiseShield") !=  0 && playerUnit.WeaponRested()){
				playerUnit.AimShield (Camera.main.ScreenToWorldPoint (Input.mousePosition));
				blocking = true;
			}else{
				playerUnit.AimWeapon (Camera.main.ScreenToWorldPoint (Input.mousePosition));
				playerUnit.LowerShield ();
				blocking = false;
			}

			if(Input.GetAxisRaw("CastSpell") != 0 && canCastSpell){
				playerUnit.CastSpell(Camera.main.ScreenToWorldPoint (Input.mousePosition));
				canCastSpell = false;
			}else if(Input.GetAxisRaw("CastSpell") == 0){
				canCastSpell = true;
			}

			//healing
			if(Input.GetAxisRaw("Heal") != 0){
				playerUnit.Heal();
			}

		//	if (hpIndex > playerUnit.health - 1) {
				//healthBar [hpIndex].transform.position = new Vector3 (-1000, -1000, -1000);
				//hpIndex--;
	//		}

			if (playerUnit.dead) {
				string profile = PlayerPrefs.GetString("profile");
				PlayerPrefs.SetString(profile + "settlements saved", "");
				PlayerPrefs.SetString("profile" + profile, "");
				PlayerPrefs.DeleteAll();
				SceneManager.LoadScene ("MainMenu");
			}
		}
	}
		UpdateUI ();
	}
		
	public void UpdateUI(){
		hpDisplay.SetHP (playerUnit.health);
		staminaDisplay.SetStamina (playerUnit.stamina, playerUnit.staminaMax);
	}
	void Attack(){
		playerUnit.AttackWithWeapon();
	}

	void TalkToNPC(){
		if (playerUnit.GetNPC () != null) {
			playerUnit.GetNPC ().ActivateDialogue ();
		} else {
			//do nothing I guess
		}
	}

	public void Disable(){
		disabled = true;
		playerUnit.SetVelocity (new Vector2 (0, 0));
	}
	public void Enable(){
		canAttack = false;
		disabled = false;
	}
	public void DamageEffect(){
		StopCoroutine (Pulse());
		StartCoroutine (Pulse());
	}
	IEnumerator Pulse(){
		redOverlay.SetActive(true);
		float t = 0;
		Color startColor = redOverlay.GetComponent<Image> ().color;
		while (t <= 1) {
			yield return null;
			t += Time.deltaTime / pulseTime;
			redOverlay.GetComponent<Image> ().color = new Color(startColor.r, startColor.g, startColor.b, 1 - (t/1)); 
		}
		redOverlay.GetComponent<Image> ().color = startColor;
		redOverlay.SetActive (false);

	}
}