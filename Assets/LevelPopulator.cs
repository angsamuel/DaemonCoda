using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//populates level with units
public class LevelPopulator : MonoBehaviour {
	public List<GameObject> enemies;
	public List<float> enemyProbs;
	public LevelGenerator lg;
	public float enemySpawnChance = 0.01f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Populate(){
		for(int x = 0; x<lg.level_grid_size; x++){
			for(int y = 0; y<lg.level_grid_size; y++){
				//if tile is appropriate, spawn unit
				if(lg.GetTileTag(x,y) == "street" || lg.GetTileTag(x,y) == "floor"){
					if(Random.Range(0.0f, 1.0f) < enemySpawnChance){
						//choose enemy according to weighted prob
						float currentWeight = 0.0f;
						float roll = Random.Range(0.0f, 1.0f);
						for(int i = 0; i<enemies.Count; i++){
							currentWeight+=enemyProbs[i];
							if(currentWeight > roll){
								GameObject newEn = Instantiate(enemies[i], transform);
								newEn.transform.position = lg.levelGrid[x,y].transform.position;
							}
						}
					}
				}
			}
		}
	}


	public void CreateSquads(){

	}

}
