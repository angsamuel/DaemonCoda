using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOW : MonoBehaviour {
	public bool hasPeripheralVision = true;
	public float viewRadiusPeripheralVision = 10f;
	public float viewRadius = 5;
	public float viewAngle = 135; 
	public LayerMask obstacleMask, targetMask;
	Collider2D[] playerInRadius;
	public List<Transform> visiblePlayer = new List<Transform>();

	void FixedUpdate(){
		//FindVisiblePlayer();
		
		if(routineReady){
				StartCoroutine(FindVisibleTargets());
		}
	}
	bool routineReady = true;


	void FVT(int i, ref Collider2D[] targetsInViewRadius){
		Transform target = targetsInViewRadius[i].transform;
        bool isInFOV = false;

        //check if hideable should be hidden or not
         Vector3 dirToTarget = (target.position - transform.position).normalized;
        if (Vector3.Angle(transform.right, dirToTarget) < viewAngle / 2) {
            float dstToTarget = Vector3.Distance(transform.position, target.position);
            if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
               isInFOV = true;
            }
        } else if (hasPeripheralVision) {
            float dstToTarget = Vector3.Distance(transform.position, target.position);
            // here we have to check the distance to the target since the peripheral vision may have a different radius than the normal field of view
             if (dstToTarget < viewRadiusPeripheralVision && !Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                    isInFOV = true;
            }
        }

        //apply effect to IHideable
        IHideable hideable = target.GetComponent<IHideable>();
        if (hideable != null) {
            if (isInFOV) {
                StartCoroutine(target.GetComponent<IHideable>().FOVEnterRoutine());
            } else {
             	StartCoroutine(target.GetComponent<IHideable>().FOVLeaveRoutine());
            }
         }
	}


	IEnumerator FindVisibleTargets() {
		routineReady = false;
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
		
        Physics2D.autoSyncTransforms = false;
		
        /* check normal field of view */
		int kmax = 4;
		for(int k = 0; k<kmax; k++){
			for (int i = (targetsInViewRadius.Length * k)/4; i < (targetsInViewRadius.Length * (k+1) )/4; i++) {
            	FVT(i, ref targetsInViewRadius);
        	}
		}

        Physics2D.autoSyncTransforms = true;
		yield return null;
		routineReady = true;

    }

	void FindVisiblePlayer(){
		playerInRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius);

		for(int i = 0; i < playerInRadius.Length; i++){
			Transform player = playerInRadius[i].transform;
			Vector2 dirPlayer = new Vector2(player.position.x - transform.position.x, player.position.y - transform.position.y);
			if(Vector2.Angle(dirPlayer, transform.right) < viewAngle / 2){
				float distancePlayer = Vector2.Distance(transform.position, player.position);
				if(!Physics2D.Raycast(transform.position, dirPlayer, distancePlayer, obstacleMask)){
					//visiblePlayer.Add(player);visiblePlayer.Add(player);
					//Debug.Log("woweee");
				}
			}
		}
	}


	public Vector2 DirFromAngle(float angleDeg, bool global){
		if(!global){
			angleDeg +=transform.eulerAngles.z;
		}
		return new Vector2(Mathf.Sin(angleDeg * Mathf.Deg2Rad), Mathf.Cos(angleDeg * Mathf.Deg2Rad));
	}
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
