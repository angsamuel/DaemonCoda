using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaDisplay : MonoBehaviour {
	public GameObject frontBar;
	public GameObject backBar;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetStamina(float current, float max){
		frontBar.transform.localScale = new Vector3 (current / max, 1, 1); 
	}
}
