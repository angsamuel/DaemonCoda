using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour {
	public int x;
	public int y;
	MapManager mm;
	// Use this for initialization
	void Start () {
		mm = GameObject.Find("MapManager").GetComponent<MapManager>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnMouseOver(){
		mm.FillFromPiece(this);
	}
}
