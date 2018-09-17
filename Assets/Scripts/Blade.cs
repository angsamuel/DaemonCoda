using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour {
	float bounceCost = 30f;
	public int id;
	Weapon weapon;
	public bool onWeapon = true;

	// Use this for initialization
	void Start () {
		id = Random.Range(-2000000000,2000000000);
		if(onWeapon){
			weapon = transform.parent.GetComponent<Weapon>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Bounce(){
		if(onWeapon){
			weapon.Bounce();
			weapon.Disable(1.5f);
			StartCoroutine(NoDamage());
		}
	}

	IEnumerator NoDamage(){
		transform.tag = "disabled damage source";
		yield return new WaitForSeconds(1f);
		transform.tag = "damage source";
	}

	public void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "shield"){
			Bounce();
		}
	}
}
