using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour {
	public List<GameObject> checkpoints;
	public List<UnitController> unitControllers;
	public int squadIndex;
	public int squadDirection = 1;

	//when all uni
	public int checkPointCount = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//tell all units to move on
	 IEnumerator IssueCommandsDelay(){
		yield return new WaitForSeconds(1);
		checkPointCount = 0;
		for(int i = 0; i<unitControllers.Count; i++){
				unitControllers[i].canPatrol = true;
				//update index
				unitControllers[i].NextCheckpoint(squadIndex);
		}
	}
	public void CheckIn(){
		checkPointCount += 1;
		if(checkPointCount >= unitControllers.Count){
			//tell units to proceed to next checkpoint
			
			if(squadIndex == 0){
					squadDirection = 1;
			}else if(squadIndex == checkpoints.Count - 1){
					squadDirection = -1;
			}
			squadIndex += squadDirection;

			StartCoroutine(IssueCommandsDelay());

			
		}
	}
}
