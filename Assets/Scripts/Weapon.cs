using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	public GameObject blade;
	Rigidbody2D rb;
	public int armoryIndex;
	public BoxCollider2D bc;
	public float timeToSwing = 1;
	public float speed, staminaCost;
	[HideInInspector] public bool rested;
	protected float swingTracker = 0;
	protected int direction = 1;
	protected float lethalbuffer;
	public float aimSpeed;
	protected Vector3 myTarget;
	public bool equipped = false;
	// Use this for initialization
	protected void Start () {
		rested = true;
		if(timeToSwing <= 0){
			timeToSwing = 1;
		}
		rb = GetComponent<Rigidbody2D>();
		if(bc!=null){bc.enabled = false;}
		if(aimSpeed == 0){
			aimSpeed = 5;
		}
	}

	public bool IsRested(){
		return rested;
	}

	public virtual void StartSwing(){
		rested = false;
	}

	Coroutine swingRoutine;
	bool swinging = false;
	// public virtual void StartSwingNew(){
	// 	if(!swinging && !disabled){
	// 		swingRoutine = StartCoroutine(SwingRoutine());
	// 	}
	// }
		
	// IEnumerator SwingRoutine(){
	// 	swinging = true;
		
	// 	//yield return new WaitForSeconds(timeToSwing/4);

	// 	float t = 0;
	// 	while(t<timeToSwing/4){
	// 		yield return null;
	// 		t+=Time.deltaTime;
	// 		rb.angularVelocity = (360 / timeToSwing) * (t / (timeToSwing/4));
	// 	}

	// 	bc.enabled = true;
	// 	rb.angularVelocity = 360 / timeToSwing;

	// 	yield return new WaitForSeconds(timeToSwing/2);

	// 	bc.enabled = false;

	// 	t = timeToSwing/4;
	// 	while(t>0){
	// 		yield return null;
	// 		t-=Time.deltaTime;
	// 		rb.angularVelocity = (360 / timeToSwing) * (t / (timeToSwing/4));
	// 	}
		
	// 	swinging = false;
	// 	rb.angularVelocity = 0;
	// }

	public void StopSwing(){
		rested = true;
		swingTracker = 0;

		//new swing
		//rb.angularVelocity = 0;
		swinging = false;
		bc.enabled = false;
	}
	
	// Update is called once per frame
	virtual protected void Update () {
		AttackCheck ();
	}
	bool disabled = false;
	public void Disable(float seconds){
		if(disabled = false){
			StartCoroutine(DisableRoutine(seconds));
		}
	}

	IEnumerator DisableRoutine(float seconds){
		disabled = true;
		yield return new WaitForSeconds(seconds);
		disabled = false;
	}

	virtual protected void AttackCheck(){
		if (!rested && !disabled) {
			if (swingTracker > 90) {
				if(bc!=null){bc.enabled = true;}

			}
			if (swingTracker > 270) {
				if(bc!=null){bc.enabled = false;}

			}

			float swingTemp = Time.deltaTime * speed;
			swingTemp *= (.1f + Mathf.Pow(Mathf.Abs(Mathf.Sin (0.5f * (swingTracker*Mathf.PI / 180f))),1));

			transform.RotateAround (transform.position, new Vector3 (0, 0, 1), swingTemp*direction);
			swingTracker += swingTemp;
			if (Mathf.Abs (swingTracker) > 360) {
				rested = true;
				swingTracker = 0;
				ChangeDirections ();
			}
		} else {
			//if(bc!=null){bc.enabled = false;}
		}
	}

	public void Bounce(){
		swingTracker = 0;
		rested = true;
		ChangeDirections();
	}

	virtual public void ChangeDirections(){
		direction = -direction;

	}

	virtual public void Aim(Vector3 target){
		myTarget = target;
		if (IsRested() && !swinging) {
			Quaternion nRotation = Quaternion.LookRotation (Vector3.forward, target - transform.position);
			transform.rotation = Quaternion.Slerp (transform.rotation, nRotation, Time.deltaTime * aimSpeed);
		}
	}

	virtual public void InstantAim(Vector3 target){
		myTarget = target;
		if (IsRested() && !swinging) {
			Quaternion nRotation = Quaternion.LookRotation (Vector3.forward, target - transform.position);
			transform.rotation = nRotation;
		}
	}


	virtual public void Strike(){

	}
}
	