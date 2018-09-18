using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMonitor : MonoBehaviour {
	public string name;
	public Text nameDisplayText;
	public UnitController uc;
	// Use this for initialization
	void Start () {
		StartCoroutine(CheckAgro());
	}
	
	// Update is called once per frame
	void LateUpdate () {
		
	}

	IEnumerator CheckAgro(){
		while(true){
			yield return new WaitForSeconds(.25f);
			if(uc.target != null ){
				nameDisplayText.text = name;
			}
		}
	}
}
