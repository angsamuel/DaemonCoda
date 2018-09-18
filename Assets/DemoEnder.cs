using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DemoEnder : MonoBehaviour {
	public LevelController lc;
	public Unit boss;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(lc.playerUnit.weapon != null && lc.playerUnit.weapon.armoryIndex == 7){
			StartCoroutine(EndDemo(2));
		}if(boss.dead){
			StartCoroutine(EndDemo(10));
		}
	}

	IEnumerator EndDemo(int t){
		yield return new WaitForSeconds(t);
		SceneManager.LoadScene("EndDemo");

	}
}
