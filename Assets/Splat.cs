using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splat : MonoBehaviour {
	float minAlpha = 0.25f;
	float maxAlpha = 0.75f;
	float minSize = 0.1f;
	float maxSize = 0.5f;
	float timeToExpand = .25f;
	bool expanding = true;
	SpriteRenderer sr;
	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Random.Range(minAlpha, maxAlpha));
		StartCoroutine(ExpandRoutine());
	}
	
	IEnumerator ExpandRoutine(){
		float size = Random.Range(minSize,maxSize);
		float t = 0;
		while(t < timeToExpand && expanding){
			yield return null;
			t += Time.deltaTime;
			transform.localScale = new Vector2((t/timeToExpand) * size,(t/timeToExpand) * size);
		}
		if(expanding){
			transform.localScale = new Vector2(size,size);
		}
		yield return null;
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "wall"){
			expanding = false;
		}
	}




	// Update is called once per frame
	void Update () {
		
	}
}
