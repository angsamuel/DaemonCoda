using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour {
	float fadeInTime = 3.0f;
	AudioSource audioSource;
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
		StartCoroutine(StartMusic());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator StartMusic(){
		float t = 0;
		while(t < fadeInTime){
			audioSource.volume = t/fadeInTime;
			t+= Time.deltaTime;
			yield return null;
		}
		audioSource.volume = 1;
		yield return null;
	}
}
