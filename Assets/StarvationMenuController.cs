using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StarvationMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(StarvationRoutine());
		PlayerPrefs.DeleteAll();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator StarvationRoutine(){
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene("MainMenu");
	}
}
