using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {
	float staminaCost = 15;
	protected Vector3 myTarget;
	public GameObject plate;
	public GameObject sparks;
	public Unit owner;
	bool rested = true;
	float aimSpeed = 5f;

	// Use this for initialization
	void Start () {
		plate.GetComponent<SpriteRenderer>().color = owner.body.GetComponent<SpriteRenderer>().color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool IsRested(){
		return rested;
	}
	public void Lower(){
		GetComponent<BoxCollider2D>().enabled = false;
		Color c = plate.GetComponent<SpriteRenderer>().color;
		plate.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, 0);
	}
	
	virtual public void Aim(Vector3 target){
		myTarget = target;
			Quaternion nRotation = Quaternion.LookRotation (Vector3.forward, target - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, nRotation, Time.deltaTime * aimSpeed);

			GetComponent<BoxCollider2D>().enabled = true;
			Color c = plate.transform.GetComponent<SpriteRenderer>().color;
			plate.GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, 1);
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "damage source" || other.tag == "disabled damage source"){
			//Debug.Log("USING STAMINA");
			owner.UseStamina(staminaCost);
			Instantiate(sparks, plate.transform);
		}
	}

}
