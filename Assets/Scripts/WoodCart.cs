using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodCart : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other){
		other.GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, 0, 0);
	}
}
