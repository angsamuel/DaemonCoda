using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodCuttingStation : MonoBehaviour {
	public GameObject woodSlice;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CutWood(){
		GameObject newSlice = Instantiate (woodSlice, transform.position, Quaternion.identity);
		newSlice.GetComponent<Rigidbody2D> ().velocity = new Vector3 (-20, 0, 0);
	}

	void OnTriggerEnter2D(Collider2D other) 
	{ 
		if (other.gameObject.tag == "damage source") {
			
			CutWood ();
		} 
	}
}
