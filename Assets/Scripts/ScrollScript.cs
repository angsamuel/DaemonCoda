using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScript : MonoBehaviour {

	public float speed = 0f;
	public Rigidbody2D tracker;
	public GameObject unit;
	// Update is called once per frame
	void Update () {
		//gameObject.GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 (Time.time, 0f);//tracker.velocity / 100;		
		gameObject.GetComponent<Renderer>().material.mainTextureOffset = (unit.transform.position / 100) * speed;
	}
}
