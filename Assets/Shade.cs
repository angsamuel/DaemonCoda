using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shade : MonoBehaviour {

	bool fadingToClear = false;
	bool fadingToDark = false;
	Coroutine fadeRoutine;
	float fadeTime = 1.0f;
	float alpha = 1.0f;
	SpriteRenderer sr;

	public void Start(){
		sr = GetComponent<SpriteRenderer>();
	}

	public void FadeToClear(){
		if(fadeRoutine != null){
			StopCoroutine(fadeRoutine);
		}
		fadeRoutine = StartCoroutine(FadeToClearRoutine());

	}

	public void FadeToDark(){
		if(fadeRoutine != null){
			StopCoroutine(fadeRoutine);
		}
		fadeRoutine = StartCoroutine(FadeToDarkRoutine());
	}
	

	IEnumerator FadeToDarkRoutine(){
		float t = 0.0f;
		t = alpha * fadeTime;
		while(t < fadeTime){
			t+= Time.deltaTime;
			alpha = t / fadeTime;
			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
			yield return null;
		}
	}

	IEnumerator FadeToClearRoutine(){
		float t = 0.0f;
		t = (1.0f - alpha) * fadeTime;
		while(t < fadeTime){
			t+= Time.deltaTime;
			alpha = 1.0f  - (t / fadeTime);
			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
			yield return null;
		}
	}
}
