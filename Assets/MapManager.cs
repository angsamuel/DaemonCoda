using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour {
	public int mapWidth;
	public int mapHeight;

	public GameObject tile;
	public GameObject pathPiece;
	public GameObject destinationPiece;
	public GameObject playerPiece;
	public GameObject settlementPiece;
	List<GameObject> tiles;
	List<GameObject> pieces;
	Color playerColor;

	public CrestGenerator cg;

	public SettlementGenerator sg;

	// Use this for initialization
	void Start () {
		string profile = PlayerPrefs.GetString("profile");
		float r = PlayerPrefs.GetFloat(profile + "R");
		float g = PlayerPrefs.GetFloat(profile + "G");
		float b = PlayerPrefs.GetFloat(profile + "B");
		playerColor = new Color(r,g,b);
		GenerateWorld();
	}
	//seed for the world
	void GenerateWorld(){
		int seed = PlayerPrefs.GetInt(PlayerPrefs.GetString("profile") + "world seed");
		if(seed == 0){
			while(seed ==0){
				seed = Random.Range(int.MinValue, int.MaxValue);
			}
		}

		Random.seed = seed;


		InstantiateTiles();
		PlacePiece(destinationPiece, mapWidth/2,mapHeight-1);
		GameObject pp = PlacePiece(playerPiece, mapWidth/2, 0);
		pp.GetComponent<Image>().color = playerColor;
		//generate settlements
		PlopSettlements();
	}

	void PlopSettlements(){
		for(int y = 1; y<mapHeight-1; y++){
			int perRow = Random.Range(1,3);
			for(int i = 0; i<perRow; i++){
				int x = Random.Range(0, mapWidth);
				SettlementPiece sp = PlacePiece(settlementPiece, x,y).GetComponent<SettlementPiece>();
				Color newColor = new Color(Random.Range(.5f, 1f), Random.Range(.5f, 1f), Random.Range(.5f, 1f));
				sp.SetValues(x,y,sg.GenerateSettlement(), newColor);
							

			}
		}
	}

	GameObject PlacePiece(GameObject piece, int x, int y){
		pieces[(mapHeight * x) + y] = Instantiate(piece, GetTile(x,y).transform);
		return pieces[(mapHeight * x) + y];
		
	}


	void InstantiateTiles(){
		tiles = new List<GameObject>();
		pieces = new List<GameObject>();
		
		for(int x = 0; x<mapWidth; x++){
			for(int y = 0; y<mapHeight; y++){
				
				GameObject newTile = Instantiate(tile, transform);
				newTile.transform.localPosition = new Vector3(-mapWidth/2,-mapHeight/2,0) + new Vector3(x,y,0);
				newTile.GetComponent<MapTile>().x = x;
				newTile.GetComponent<MapTile>().y = y;
				tiles.Add(newTile);
				pieces.Add(null);
			}
		}
	}

	public GameObject GetTile(int x,int y){
		return tiles[(mapHeight * x) + y];
	}

	public GameObject GetPiece(int x, int y){
		return pieces[(mapHeight * x) + y];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FillFromPiece(MapTile mt){
		Debug.Log("filling!");
		GameObject piece = GetPiece(mt.x, mt.y);
		if(piece == null){
			Debug.Log("null");
		}else if(piece.GetComponent<SettlementPiece>()!=null){
			cg.GenerateCrest(piece.GetComponent<SettlementPiece>().settlement.crestSeed, piece.GetComponent<SettlementPiece>().color);
		}
	}
}
