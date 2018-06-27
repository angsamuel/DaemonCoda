using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//can manage waypoints
public class LevelController : MonoBehaviour {
	public float alertRadius;
	public List<UnitController> enemyControllers;
    public List<string> teams;

    public Hashtable teamTable;

	void Awake(){
		enemyControllers = new List<UnitController> ();
        teams = new List<string>();
        teamTable = new Hashtable();
        
	}
	// Use this for initialization
	void Start () {
        //StartCoroutine(TableConstructionRoutine());
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

    public IEnumerator TableConstructionRoutine()
    {
        yield return new WaitForSeconds(3);
        BuildTeamTables();
    }

    void BuildTeamTables()
    {
        for(int i = 0; i<enemyControllers.Count; i++)
        {
            if (!teams.Contains(enemyControllers[i].unit.team))
            {
                teams.Add(enemyControllers[i].unit.team);
            }
        }

        List<List<Unit>> unitLists = new List<List<Unit>>();

        for(int i = 0; i<teams.Count; i++)
        {
            unitLists.Add(new List<Unit>());
        }

        for(int i = 0; i<enemyControllers.Count; i++)
        {
            for(int j = 0; j<teams.Count; j++)
            {
                if(enemyControllers[i].unit.team != teams[j])
                {
                    unitLists[j].Add(enemyControllers[i].unit);
                }
            }
        }

        for (int i = 0; i < teams.Count; i++)
        {
            teamTable.Add(teams[i],unitLists[i]);
        }

    }
}
