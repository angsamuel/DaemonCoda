using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinPetalProjectile : MonoBehaviour {
	public Rigidbody2D rb;
	public SpriteRenderer sr;
	public BoxCollider2D bc;
	public GameObject explosion;
	float powerUpTime = .7f;
	float wait1Time = 0.3f;
	float wait2Time = .3f;
	float speed = 15;
	// Use this for initialization
	void Start () {
		//Launch(new Vector3(Random.Range(-3.0f,3.0f),Random.Range(-3.0f,3.0f),0));
		e = explosion;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Launch(Vector3 targetLocation){
		StartCoroutine(LaunchRoutine(targetLocation));
	}

	IEnumerator Timer(){
		yield return new WaitForSeconds(4);
		Destroy(this.gameObject);
	}

	IEnumerator LaunchRoutine(Vector3 targetLocation){
		Color c = sr.color;
		sr.color = new Color(c.r, c.g, c.b, 0);

		targetLocation += new Vector3(Random.Range(-.5f, .5f), Random.Range(-.5f,.5f),0);

		yield return new WaitForSeconds(wait1Time);
		Quaternion nRotation = Quaternion.LookRotation (Vector3.forward, targetLocation - transform.position);
		transform.rotation = nRotation;
		float t = 0;
		while(t < powerUpTime){
			t+=Time.deltaTime;
			sr.color = new Color(c.r, c.g, c.b, t/powerUpTime);
			yield return null;
		}
		sr.color = new Color(c.r, c.g, c.b, 1.0f);
		bc.enabled = true;
		yield return new WaitForSeconds(wait2Time);
		Vector3 tLocation = new Vector3(targetLocation.x, targetLocation.y, 0);
		
		rb.velocity = UBP(tLocation, transform.position) * speed;
		//Debug.Log(UBP(targetLocation, transform.position).magnitude * speed);
		//Debug.Log((UBP(targetLocation, transform.position) * speed).magnitude );
		//Debug.Log(UBP(targetLocation, transform.position));
		//Debug.Log(UBP(targetLocation, transform.position) * speed);
		StartCoroutine(Timer());
	}

	Vector3 UBP(Vector3 p1, Vector3 p2){
		if((p1 == p2)){
			new Vector3(0,0);
		}
		return (1f / (p1 - p2).magnitude) * (p1 - p2);
	}
	bool hasExploded = false;
	public void Explode(){
		if(!hasExploded && explosion != null){
			hasExploded = true;
			if(explosion != null){
				Instantiate(explosion,transform);
			}
			explosion = null;
			if(bc != null){
				bc.enabled = false;
			}
			sr.enabled = false;
			rb.velocity = new Vector3(0,0,0);
			Debug.Log("done exploding");
		}
	}
	bool canReflect = true;
	IEnumerator ReflectCooldown(){
		canReflect = false;
		yield return new WaitForSeconds(1f);
		canReflect = true;

	}
	GameObject e;
	void Reflect(){
		if(e!= null){
			e= null;
			canReflect = false;
			StartCoroutine(ReflectCooldown());
			rb.velocity = -rb.velocity;
			transform.Rotate(new Vector3(0,0,180));
			Debug.Log("REFLECTION");
			e=explosion;
		}
	}

	public void OnTriggerEnter2D(Collider2D other){	
		if(other.tag == "shield" || other.tag == "wall"){
			Debug.Log("exploding");
			Explode();
		}
		if(other.tag == "damage source" && canReflect){
			//StartCoroutine(ReflectCooldown());
				//Reflect();
		}
	}
}
