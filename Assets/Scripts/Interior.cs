using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interior : MonoBehaviour {
	public float fadeSpeed;
	public SpriteRenderer roof;
	public SpriteRenderer roofFilling;
	public SpriteRenderer floor;
	public SpriteRenderer background;
	public List<SpriteRenderer> otherRoofObjects;

	Collider2D player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(player!=null){
			if(!this.GetComponent<Collider2D>().IsTouching(player) && background.color.a == 1){
					ExitFade();
					Debug.Log("safteyFade");
					StartCoroutine(ResetSafteyFade());
			}
		}
	}

	void LateUpdate(){

	}

	void EnterFade(){
		StartCoroutine (FadeInSprite (background));
		StartCoroutine (FadeOutSprite (roof));
		StartCoroutine (FadeOutSprite (roofFilling));
		for (int i = 0; i < otherRoofObjects.Count; i++) {
			if (otherRoofObjects [i] != null) {
				StartCoroutine (FadeOutSprite (otherRoofObjects [i]));
			}
		}
	}
	

	void ExitFade(){
		StartCoroutine (FadeOutSprite (background));
		StartCoroutine (FadeInSprite (roof));
		StartCoroutine (FadeInSprite (roofFilling));
		for (int i = 0; i < otherRoofObjects.Count; i++) {
			if (otherRoofObjects [i] != null) {
				StartCoroutine (FadeInSprite (otherRoofObjects [i]));
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) 
	{ 
		if(other.transform.tag == "player unit"){
			EnterFade();
			player = other;
		}
	}

	void OnTriggerExit2D(Collider2D other) 
	{ 
		if(other.transform.tag == "player unit"){
			ExitFade();
		}
	}

	IEnumerator FadeInSprite(SpriteRenderer sr){
		float t = sr.color.a * fadeSpeed;
		while (t < fadeSpeed) {
			t += Time.deltaTime;
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, t/fadeSpeed);
			yield return null;
		}
		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 1f);
	}

	IEnumerator FadeOutSprite(SpriteRenderer sr){
		float t = sr.color.a * fadeSpeed;
		while (t > 0) {
			t -= Time.deltaTime;
			sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, t/fadeSpeed);
			yield return null;
		}
		sr.color = new Color (sr.color.r, sr.color.g, sr.color.b, 0f);
	}

	IEnumerator ResetSafteyFade(){
		float t = 0;
		while (t < fadeSpeed*2) {
			t+=Time.deltaTime;
			yield return null;
		}
		player = null;
	}
}
