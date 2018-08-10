using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour {
		public int id;

	// Use this for initialization
	void Start () {
				id = Random.Range(-2000000000,2000000000);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		Debug.Log ("wow");
	}
}
