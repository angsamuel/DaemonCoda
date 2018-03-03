using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPDisplay : MonoBehaviour {
	public List<GameObject> bars;
	// Use this for initialization
	Color hpColor;
	void Start () {
		hpColor = bars [0].GetComponent<Image> ().color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetHP(int hp){
		for(int i = 0; i<bars.Count; i++){
			if (i < hp) {
				bars [i].GetComponent<Image> ().color = hpColor;
			} else {
				bars [i].GetComponent<Image> ().color = Color.clear;
			}
		}
	}
}
