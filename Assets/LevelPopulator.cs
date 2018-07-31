using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//populates level with units
public class LevelPopulator : MonoBehaviour {
	public List<GameObject> enemies;
	public List<float> enemyProbs;
	public GameObject patrolUnit;
	public LevelGenerator lg;
	public float enemySpawnChance = 0.005f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Populate(){
		for(int x = 5; x<lg.level_grid_size - 5; x++){
			for(int y = 5; y<lg.level_grid_size - 5; y++){
				//if tile is appropriate, spawn unit
				if(lg.GetTileTag(x,y) == "street" || lg.GetTileTag(x,y) == "floor"){
					if(Random.Range(0.0f, 1.0f) < enemySpawnChance){
						//choose enemy according to weighted prob
						float currentWeight = 0.0f;
						float roll = Random.Range(0.0f, 1.0f);
						for(int i = 0; i<enemies.Count; i++){
							currentWeight+=enemyProbs[i];
							if(currentWeight > roll){
								roll = 2;
								GameObject newEn = Instantiate(enemies[i], transform);
								newEn.transform.position = lg.levelGrid[x,y].transform.position + new Vector3(Random.Range(-0.25f, 0.25f),Random.Range(-0.25f, 0.25f));
							}
						}
					}
				}
			}
		}
		CreateSquads();
	}


	public void CreateSquads(){
		for(int i = 0; i<lg.patrolRoutes.Count; i++){

			if(lg.patrolRoutes[i].checkpoints.Count > 2){

				int startCheckPoint = 0;

				int squadMembers = Random.Range(3, 7);
				//spawn squad members, give each their index, and patrolRoute
				for(int j = 0; j<squadMembers; j++){
					UnitController newSquaddie = Instantiate(patrolUnit, lg.patrolRoutes[i].checkpoints[startCheckPoint].transform.position, Quaternion.identity).GetComponent<UnitController>();
					newSquaddie.transform.Translate(new Vector3(Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f)));
					newSquaddie.AssignToPatrol(lg.patrolRoutes[i], startCheckPoint);
				}


			}
		}
	}

}
