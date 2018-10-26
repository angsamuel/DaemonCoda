using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour {
	public int mapWidth;
	public int mapHeight;

	public GameObject tile;
	public GameObject pathPiece;
	public GameObject destinationPiece;
	public GameObject playerPiece;
	List<GameObject> tiles;
	

	// Use this for initialization
	void Start () {
		InstantiateTiles();
		
		
	}

	void PlaceDestinationPiece(){

	}
	void PlacePlayerPiece(){
		
	}

	void InstantiateTiles(){
		for(int x = 0; x<mapWidth; x++){
			for(int y = 0; y<mapWidth; y++){
				
				GameObject newTile = Instantiate(tile, transform);
				newTile.transform.localPosition = new Vector3(-mapWidth/2,-mapHeight/2,0) + new Vector3(x,y,0);
				tiles.Add(newTile);
			}
		}
	}

	public GameObject GetTile(int x,int y){
		return tiles[(mapWidth * y) + x];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
