using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//can manage waypoints
public class LevelController : MonoBehaviour {
	public float alertRadius;
	public List<UnitController> enemyControllers;
	void Awake(){
		enemyControllers = new List<UnitController> ();
	}
	// Use this for initialization
	void Start () {
		
	}

	public void AddEnemyController(UnitController u){
		enemyControllers.Add (u);
	}

	public void AlertNearbyEnemies(Vector3 position){
		for (int i = 0; i < enemyControllers.Count; i++) {
			if (Vector3.Distance (position, enemyControllers[i].unit.transform.position) < alertRadius) {
				enemyControllers [i].Alert ();
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
