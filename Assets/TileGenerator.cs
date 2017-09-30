using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour {
	public GameObject tile;
	public int width, height;
	Vector3 myPosition;

	// Use this for initialization
	void Start () {
		BuildTiles ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void BuildTiles(){
		for (int w = 0; w < width; w++) {
			for (int h = 0; h < height; h++) {
				GameObject newTile = GameObject.Instantiate (tile, new Vector3 (w*6, h*6, 0) + transform.position, Quaternion.identity);
			}
		}
	}
}
