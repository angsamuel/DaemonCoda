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
	bool dashLock, disabled, canAttack, canPickup = false;

	public GameObject staminaBar;
	public HPDisplay hpDisplay;
	public StaminaDisplay staminaDisplay;
	int hpIndex;

	// Use this for initialization
	void Start () {
		hpIndex = 2;
		redOverlay.SetActive(false);
	}

	void EquipWeapon(){
		playerUnit.Equip ();
	}

	// Update is called once per frame
	void Update () {
		//Dash
		if (!disabled) {
			//weapon pickup
			if (Input.GetAxisRaw ("Pickup") != 0 && canPickup) {
				Debug.Log ("PICKUP");
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
			if (Input.GetAxisRaw ("Attack") != 0 && canAttack) {
				Attack ();
				canAttack = false;
			} else if (Input.GetAxisRaw ("Attack") == 0) {
				canAttack = true;
			}

			//action
			if (Input.GetAxisRaw ("Action") != 0) {
				TalkToNPC ();
				Debug.Log ("wow");
			}

			//playerUnit.weapon.Aim (Camera.main.ScreenToWorldPoint (Input.mousePosition));
			playerUnit.AimWeapon (Camera.main.ScreenToWorldPoint (Input.mousePosition));


			UpdateUI ();

		//	if (hpIndex > playerUnit.health - 1) {
				//healthBar [hpIndex].transform.position = new Vector3 (-1000, -1000, -1000);
				//hpIndex--;
	//		}

			if (playerUnit.dead) {
				SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
			}
		}
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
			Debug.Log ("yes npc");
		} else {
			Debug.Log ("no npc");
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
		Debug.Log ("damage effect called");
		StopCoroutine ("Pulse");
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