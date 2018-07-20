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

    public Unit target;

	// Use this for initialization
	public void Start () {
        breadCrumbs = new List<Vector2>();
		levelController = GameObject.Find ("LevelController").GetComponent<LevelController> ();
		levelController.AddEnemyController (this);
		playerUnit = GameObject.Find ("PlayerInputController").GetComponent<PlayerInputController> ().playerUnit;
        StartCoroutine(BreadCrumb());
        StartCoroutine(BreadCrumbCleanup());
        StartCoroutine(ProximityCheck());
	}
	
	// Update is called once per frame
	public void Update () {
        if (!unit.dead && scanEnabeled)
        {
            //CheckForPlayer ();
            LockTarget();
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
            unit.Stop(.25f);
        }
        else if(target.dead)
        {
            unit.Stop(.25f);
        }
    }

    bool proximityCheckEnabeled = true;
    float proximityWaitTime = 3.0f;

    bool scanEnabeled = false;

    IEnumerator ProximityCheck()
    {
        
        while (proximityCheckEnabeled)
        {
            
            yield return new WaitForSeconds(proximityWaitTime + Random.Range(-1.0f, 1.0f));
            scanEnabeled = false;

            for (int i = 0; i<levelController.teams.Count; i++)
            {
                if(levelController.teams[i] == unit.team)
                {
                        List<Unit> possibleTargets = levelController.teamTable[levelController.teams[i]] as List<Unit>;
                        for(int k = 0; k < possibleTargets.Count; k++)
                        {
                            if( Vector3.Distance(possibleTargets[k].transform.position, unit.transform.position) < scanRange * 3)
                            {
                                scanEnabeled = true;
                                Debug.Log("enabling scan");
                            }
                        }


                }
            }

            //check player
            if(scanEnabeled == false)
            {
                if (Vector3.Distance(playerUnit.transform.position, unit.transform.position) < scanRange * 3)
                {
                    scanEnabeled = true;
                    Debug.Log("enabling scan");
                }
            }




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
            yield return new WaitForSeconds(0.5f);
            if (breadCrumbs.Count > 0)
            {
                breadCrumbs.RemoveAt(0);
            }
           
        }
    }

	protected void CheckForPlayer(){
		if (!playerSeen) {
			if (Vector3.Distance (playerUnit.transform.position, unit.transform.position) < seeingDistance) {
				playerSeen = true;
				levelController.AlertNearbyEnemies (transform.position);

			}
		}
	}

	public void Alert(){
		playerSeen = true;
	}

    float linecastOffsetY = 0.395f;
    float linecastOffsetX = .145f;


    public bool scanning = false;
    protected bool canSeeTarget = false;
    public void LockTarget()
    {
        canSeeTarget = false;
        if(targetEnabled && target != null && !scanning && !target.dead)
        {
            List<RaycastHit2D> hitsAbove;
            List<RaycastHit2D> hits;
            List<RaycastHit2D> hitsBelow;

            hitsAbove = new List<RaycastHit2D>(Physics2D.LinecastAll(unit.transform.position + new Vector3(linecastOffsetX, linecastOffsetY), target.transform.position + new Vector3(linecastOffsetX, linecastOffsetY)));
            hits = new List<RaycastHit2D>(Physics2D.LinecastAll(unit.transform.position, target.transform.position));
            hitsBelow = new List<RaycastHit2D>(Physics2D.LinecastAll(unit.transform.position - new Vector3(linecastOffsetX, linecastOffsetY), target.transform.position - new Vector3(linecastOffsetX, linecastOffsetY)));


            for (int i = 0; i < hitsAbove.Count; i++)
            {
                if (hitsAbove[i].transform.tag == "weapon" || hitsAbove[i].transform.tag == "damage source" ||  hitsAbove[i].transform.tag == "floor" ||  hitsAbove[i].transform.tag == "Untagged")
                {
                    hitsAbove.RemoveAt(i);
                    i = i - 1;
                }

                else if (hitsAbove[i].transform.tag == "unit" && hitsAbove[i].transform.GetComponent<Unit>().team == unit.team)
                {
                    hitsAbove.RemoveAt(i);
                    i = i - 1;
                }
            }

            for (int i = 0; i < hits.Count; i++)
            {
                
                

                if (hits[i].transform.tag == "weapon" || hits[i].transform.tag == "damage source" ||  hits[i].transform.tag == "floor" ||  hits[i].transform.tag == "Untagged")
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
            Debug.Log(hits.Count);

            for (int i = 0; i < hitsBelow.Count; i++)
            {
                if (hitsBelow[i].transform.tag == "weapon" || hitsBelow[i].transform.tag == "damage source" ||  hitsBelow[i].transform.tag == "floor" ||  hitsBelow[i].transform.tag == "Untagged")
                {
                    hitsBelow.RemoveAt(i);
                    i = i - 1;
                }
                else if (hitsBelow[i].transform.tag == "unit" && hitsBelow[i].transform.GetComponent<Unit>().team == unit.team)
                {
                   hitsBelow.RemoveAt(i);
                   i = i - 1;
                }
            }

            if (hitsAbove.Count > 0 && hitsAbove[0].transform.gameObject.GetComponent<Unit>() == target
                && hits.Count > 0 && hits[0].transform.gameObject.GetComponent<Unit>() == target
                && hitsBelow.Count > 0 && hitsBelow[0].transform.gameObject.GetComponent<Unit>() == target)
            { 
                Debug.DrawLine(unit.transform.position + new Vector3(linecastOffsetX, linecastOffsetY), target.transform.position + new Vector3(linecastOffsetX, linecastOffsetY), Color.green);
                Debug.DrawLine(unit.transform.position, target.transform.position, Color.green);
                Debug.DrawLine(unit.transform.position - new Vector3(linecastOffsetX, linecastOffsetY), target.transform.position - new Vector3(linecastOffsetX, linecastOffsetY), Color.green);
                canSeeTarget = true;
            }
            else
            {
                Debug.DrawLine(unit.transform.position + new Vector3(linecastOffsetX, linecastOffsetY), target.transform.position + new Vector3(linecastOffsetX, linecastOffsetY), Color.red);
                Debug.DrawLine(unit.transform.position, target.transform.position, Color.red);
                Debug.DrawLine(unit.transform.position - new Vector3(linecastOffsetX, linecastOffsetY), target.transform.position - new Vector3(linecastOffsetX, linecastOffsetY), Color.red);
                canSeeTarget = false;

                if (breadCrumbs.Count == 0)
                {
                    target = null;
                }
            }


        }
        else if(targetEnabled && !scanning)
        {
            scanning = true;
            StartCoroutine(ScanRoutine());
        }
    }


    float scanRange = 10;
    float scanTimer = .002f;
    IEnumerator ScanRoutine()
    {


        float nudge = Random.Range(0.0f, 37.0f);
        
        for (int i = 0; i < 37; i++)
        {

            //yield return new WaitForSeconds(.04f);
            float scanX = unit.transform.position.x + scanRange * Mathf.Cos(Mathf.Deg2Rad * (i+nudge) * 10);
            float scanY = unit.transform.position.y + scanRange * Mathf.Sin(Mathf.Deg2Rad * (i+nudge) * 10);
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
                if (hits[h].transform.tag == "weapon" || hits[h].transform.tag == "damage source" || hits[h].transform.tag == "floor" || hits[h].transform.tag == "Untagged")
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
            for (int j = 0; j < possibleTargets.Count; j++)
            {
                float dist = Vector3.Distance(unit.transform.position, possibleTargets[j].transform.position);
                if (dist < minDist)
                {
                    minIndex = j;
                    minDist = dist;
                    target = possibleTargets[j];
                }
            }

            if (i == 36 && target == null && scanEnabeled)
            {
                i = 0;
                nudge = Random.Range(0.0f, 37.0f);
                yield return new WaitForSeconds(.2f);
            }
            
        }

        scanning = false;
    }

}
