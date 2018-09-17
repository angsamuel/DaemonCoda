using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarvationMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(StarvationRoutine());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator StarvationRoutine(){
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene("MainMenu");
	}
}
