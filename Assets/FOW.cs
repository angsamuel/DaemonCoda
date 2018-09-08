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
		if(targetsInViewRadius[i] != null){
			Transform target = targetsInViewRadius[i].transform;
			bool isInFOV = false;

			//apply effect to IHideable
			IHideable hideable = target.GetComponent<IHideable>();
			

			//check if hideable should be hidden or not
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			float dstToTarget = Vector3.Distance(transform.position, target.position);
			if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
				isInFOV = true;
			}
			
			if (hideable != null) {
				if (isInFOV) {
					StartCoroutine(hideable.FOVEnterRoutine());
					//Debug.Log("ENTER");
				} else {
					StartCoroutine(hideable.FOVLeaveRoutine());
				}
			}
		}
	}


	IEnumerator FindVisibleTargets() {
		routineReady = false;
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);
		
        Physics2D.autoSyncTransforms = false;
		
        /* check normal field of view */
		int kmax = 2;
		for(int k = 0; k<kmax; k++){
			for (int i = (targetsInViewRadius.Length * k)/kmax; i < (targetsInViewRadius.Length * (k+1) )/kmax; i++) {
            	FVT(i, ref targetsInViewRadius);
        	}
			yield return new WaitForSeconds(0.02f);
		}

        Physics2D.autoSyncTransforms = true;
		
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
