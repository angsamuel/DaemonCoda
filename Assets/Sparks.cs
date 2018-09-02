using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sparks : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(DestroySelf());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator DestroySelf(){
		yield return new WaitForSeconds(2);
		Destroy(gameObject);
	}
}
