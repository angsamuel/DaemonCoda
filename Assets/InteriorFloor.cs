using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorFloor : MonoBehaviour {
	public Room room;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D other){
		if(room!=null && other.tag == "player unit"){
			Debug.Log("FADING TO CLEAR");
			room.FadeToClear();
		}
	}


}
