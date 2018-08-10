using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {
    public bool targetEnabled = true;
	protected Unit playerUnit;
	public Unit unit;
	LevelController levelController;
	public float strikingDistance;
	public int seeingDistance;
	protected bool playerSeen = false;

    protected List<Vector2> breadCrumbs;


    PatrolRoute pr;
    public int prIndex;

    public Unit target;
    public bool husked = false;
	// Use this for initialization
	public void Start () {
        breadCrumbs = new List<Vector2>();
		levelController = GameObject.Find ("LevelController").GetComponent<LevelController> ();
        if(!husked){
		    levelController.AddEnemyController (this);
        }
		playerUnit = GameObject.Find ("PlayerInputController").GetComponent<PlayerInputController> ().playerUnit;
        StartCoroutine(BreadCrumb());
        StartCoroutine(BreadCrumbCleanup());
        StartCoroutine(ProximityCheck());
	}
	
	// Update is called once per frame

    public void SetTeam(string t){
        unit.team = t;
    }

    

    [HideInInspector] public bool canPatrol = false;
    public void AssignToPatrol(PatrolRoute patrolRoute, int patrolIndex){
        pr = patrolRoute;
        pr.unitControllers.Add(this);
        canPatrol = true;
        NextCheckpoint(patrolIndex);
    }

    public bool HasPatrolRoute(){
        return(pr != null);
    }

    public void NextCheckpoint(int checkPointIndex){
        prIndex = checkPointIndex;
        if(pr != null){
            patrolPosition = pr.checkpoints[prIndex].transform.position;// + new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
        }
    }   

    public void SetPatrolPosition(Vector3 pp){
        patrolPosition = pp;
    }

    Vector3 patrolPosition;
    void  Patrol(){
            unit.AimWeapon(pr.checkpoints[prIndex].transform.position);
            if(canPatrol && pr != null){
                //get new patrol position

                unit.MoveToward(patrolPosition);
                //Debug.Log(patrolPosition + ", " + pr.checkpoints[prIndex].transform.position);
                unit.AimWeapon(pr.checkpoints[prIndex].transform.position);
                //if reached position, let route know
                if(Vector3.Distance(unit.transform.position, patrolPosition) <= 0.25f){
                    unit.Stop();
                    pr.CheckIn();
                    canPatrol = false;
            } 
        }
    }


	public void LateUpdate () {
        if(pr != null && target != null){
            for(int s = 0; s<pr.unitControllers.Count; s++){
                pr.unitControllers[s].target = target;
            }
        }

        if(!unit.dead && target != null && unit.weapon != null){
            unit.weapon.Aim(target.transform.position);
        }

        if(pr!= null && target == null){
            Patrol();
        }


        if (!unit.dead && scanEnabeled)
        {
            //CheckForPlayer ();
            if(pr == null && !canSeeTarget){
                StartCoroutine(WanderRoutine());
            }
            if(!husked){
                LockTarget();
            }
        }


        if (canSeeTarget)
        {
            if (target != null)
            {
                CustomActions();
            }
        }
        else
        {
            if (target != null && breadCrumbs.Count > 0)
            {
                for (int i = 0; i < breadCrumbs.Count - 1; i++)
                {
                    if (Vector3.Distance(unit.transform.position, target.transform.position) < Vector3.Distance(breadCrumbs[0], target.transform.position))
                    {
                        breadCrumbs.RemoveAt(0);
                    }
                }
                unit.MoveToward(breadCrumbs[0]);
                if (Vector3.Distance(unit.transform.position, breadCrumbs[0]) < 0.1f)
                {
                    breadCrumbs.RemoveAt(0);
                }
            }
        }

        if(target == null)
        {
            if(!wandering){
                //unit.Stop(.25f);
            }
        }
        else if(target.dead && !wandering)
        {
            unit.Stop(.25f);
        }
    }

    bool proximityCheckEnabeled = true;
    float proximityWaitTime = 1.5f;

    bool scanEnabeled = false;

    IEnumerator ProximityCheck()
    {
        
        while (proximityCheckEnabeled)
        {
            
            
            scanEnabeled = false;

            // for (int i = 0; i<levelController.teams.Count; i++)
            // {
            //     if(levelController.teams[i] == unit.team)
            //     {
            //             List<Unit> possibleTargets = levelController.teamTable[levelController.teams[i]] as List<Unit>;
            //             for(int k = 0; k < possibleTargets.Count; k++)
            //             {
            //                 if( Vector3.Distance(possibleTargets[k].transform.position, unit.transform.position) < scanRange * 3)
            //                 {
            //                     scanEnabeled = true;
            //                     Debug.Log("enabling scan");
            //                 }
            //             }


            //     }
            // }



            //check player
            if(scanEnabeled == false)
            {
                if (Vector3.Distance(playerUnit.transform.position, unit.transform.position) < scanRange * 4f)
                {
                    scanEnabeled = true;
                    //Debug.Log("enabling scan");
                }
            }

            yield return new WaitForSeconds(proximityWaitTime + Random.Range(-1.0f, 1.0f));

        }
    }


    public virtual void CustomActions()
    {

    }

    int crumbLimit = 5;
   
    IEnumerator BreadCrumb()
    {
        while (true)
        {
            if (target != null && canSeeTarget)
            {

                if (breadCrumbs.Count < 1 || Vector2.Distance(target.transform.position, breadCrumbs[breadCrumbs.Count - 1]) > 0.1f)
                {
                    breadCrumbs.Add(target.transform.position);
                }

               
                if (breadCrumbs.Count > 200)
                {
                    breadCrumbs.RemoveAt(0);
                }
            }
            
            yield return new WaitForSeconds(.05f);
        }
    }

    IEnumerator BreadCrumbCleanup()
    {
        while (true)
        {
            //Debug.Log(breadCrumbs.Count);
            yield return new WaitForSeconds(0.25f);
            if (breadCrumbs.Count > 0)
            {
                breadCrumbs.RemoveAt(0);
            }else if(target != null && !canSeeTarget){
                target = null;
                pr = null;
            }  
        }
    }

	protected void CheckForPlayer(){
		if (!playerSeen) {
			if (Vector3.Distance (playerUnit.transform.position, unit.transform.position) < seeingDistance) {
				playerSeen = true;
				levelController.AlertNearbyEnemies (transform.position, unit.team, levelController.GetPlayerUnit());
			}
		}
	}

	public void Alert(){
		playerSeen = true;
	}

    float linecastOffsetY = 0.395f;
    float linecastOffsetX = .145f;


    public bool scanning = false;
    public bool canSeeTarget = false;

    bool LinecastTarget(float offsetX, float offsetY){
        List<RaycastHit2D> hits;
         hits = new List<RaycastHit2D>(Physics2D.LinecastAll(unit.transform.position + new Vector3(offsetX, offsetY), target.transform.position + new Vector3(offsetX, offsetY)));
        bool rayReachedTarget = false;
         for (int i = 0; i < hits.Count; i++)
            {
                if (hits[i].transform.tag == "weapon" || hits[i].transform.tag == "damage source" ||  hits[i].transform.tag == "floor" ||  hits[i].transform.tag == "Untagged"
                ||  hits[i].transform.tag == "med pak"
                ||  hits[i].transform.tag == "meal pak"
                ||  hits[i].transform.tag == "street")
                {
                    hits.RemoveAt(i);
                    i = i - 1;
                }
                else if (hits[i].transform.tag == "unit" && hits[i].transform.GetComponent<Unit>().team == unit.team)
                { 
                    hits.RemoveAt(i);
                    i = i - 1;
                }

            }

            if (hits.Count > 0 && hits[0].transform.gameObject.GetComponent<Unit>() == target)
            { 
                Debug.DrawLine(unit.transform.position + new Vector3(offsetX, offsetY), target.transform.position + new Vector3(offsetX, offsetY), Color.green);
                rayReachedTarget = true;
            }
            else
            {
                Debug.DrawLine(unit.transform.position + new Vector3(offsetX, offsetY), target.transform.position + new Vector3(offsetX, offsetY), Color.red);
                rayReachedTarget = false;
            }

            return rayReachedTarget;
        }
        
        

    void LockTarget(){
        
        canSeeTarget = false;
        if(targetEnabled && target != null && !scanning && !target.dead)
        {

          canSeeTarget = LinecastTarget(linecastOffsetX, linecastOffsetY) && LinecastTarget(-linecastOffsetX, linecastOffsetY) && LinecastTarget(linecastOffsetX, -linecastOffsetY) && LinecastTarget(-linecastOffsetX, -linecastOffsetY);


        }else if(targetEnabled && !scanning)
        {
            if(!scanning){
                StartCoroutine(ScanRoutine());
            }
        }
    }
    
    float scanRange = 6;
    IEnumerator ScanRoutine()
    {
        if(!scanning){
            scanning = true;

            float nudge = Random.Range(0.0f, 45f);
            
            for (int i = 0; i < 8; i++)
            {

                //yield return new WaitForSeconds(.04f);
                float scanX = unit.transform.position.x + scanRange * Mathf.Cos(Mathf.Deg2Rad * (i+nudge) * 45);
                float scanY = unit.transform.position.y + scanRange * Mathf.Sin(Mathf.Deg2Rad * (i+nudge) * 45);
                Vector2 scanPos = new Vector2(scanX, scanY);

            
                
                //Debug.DrawLine(unit.transform.position, scanPos, Color.green);
                //find target


                List<RaycastHit2D> hits;
                List<Unit> possibleTargets = new List<Unit>();

                hits = new List<RaycastHit2D>(Physics2D.LinecastAll(unit.transform.position, scanPos));

                //remove hits that target couldn't hide behind;
                for (int h = 0; h < hits.Count; h++)
                {
                    //remove not valid blockers
                    if (hits[h].transform.tag == "weapon" || hits[h].transform.tag == "damage source" || hits[h].transform.tag == "floor" || hits[h].transform.tag == "Untagged" || hits[h].transform.tag == "street")
                    {
                        hits.RemoveAt(h);
                        h = h - 1;
                    }
                    else if ( (hits[h].transform.tag == "unit" || hits[h].transform.tag == "player unit") && hits[h].transform.GetComponent<Unit>().team == unit.team)
                    {
                        hits.RemoveAt(h);
                        h = h - 1;
                    }
                }


                if (hits.Count > 0 && (hits[0].transform.tag == "unit" || hits[0].transform.tag == "player unit"))
                {
                    //Debug.Log("spotted " + hits.Count);
                    
                    if(hits[0].transform.GetComponent<Unit>().team != unit.team && !hits[0].transform.GetComponent<Unit>().dead)
                    {
                        //add hostile target to units
                        possibleTargets.Add(hits[0].transform.GetComponent<Unit>());
                        Debug.DrawLine(unit.transform.position, scanPos, Color.green);
                    }
                }
                else
                {
                    Debug.DrawLine(unit.transform.position, scanPos, Color.red);
                }


                //find closest reachable target
                int minIndex = 0;
                float minDist = scanRange;

                if(possibleTargets.Count > 0){
                    target = possibleTargets[0];
                    canSeeTarget = true;
                    if(pr != null){
                        for(int s = 0; s<pr.unitControllers.Count; s++){
                            pr.unitControllers[s].target = target;
                            pr.unitControllers[s].canSeeTarget = true;
                        }
                    }

                }
                
            }
            yield return new WaitForSeconds(Random.Range(.15f, .35f));
            scanning = false;
        }
    }
    bool wandering = false;
    Vector3 wanderPos;
    Vector3 aimPos;
    IEnumerator WanderRoutine()
    {
        yield return null;
        if(!wandering){
            wandering = true;
            wanderPos = transform.position + new Vector3(Random.Range(-100,100), Random.Range(-100,100));
            aimPos = transform.position + new Vector3(Random.Range(-100,100), Random.Range(-100,100));
            unit.MoveToward(wanderPos);
            
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));

            unit.Stop();
            yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
            wandering = false;
        }else if(target == null){
            unit.AimWeapon(aimPos);
        }
    }
}
