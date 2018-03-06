using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mill : MonoBehaviour {
	public Rigidbody2D rb;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		rb.angularVelocity = 20f;
	}
}
