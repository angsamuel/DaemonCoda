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
		float degreePortion = 360f/unitControllers.Count;
		float offset = Random.Range (-360, 360);
		float radius = 1.5f;
		for(int i = 0; i<unitControllers.Count; i++){
			unitControllers[i].canPatrol = true;
			//update index
			unitControllers[i].NextCheckpoint(squadIndex);
			//unitControllers[i].SetPatrolPosition()

			float xComp = (radius * Mathf.Cos ( ((i*degreePortion) + offset) * Mathf.PI / 180f)) + checkpoints[squadIndex].transform.position.x;
			float yComp = (radius * Mathf.Sin ( ((i*degreePortion) + offset) * Mathf.PI / 180f)) + checkpoints[squadIndex].transform.position.y;
			unitControllers[i].SetPatrolPosition(new Vector3(xComp, yComp, 0));
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
