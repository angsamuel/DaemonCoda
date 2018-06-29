using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MealPakPanel : MonoBehaviour {

	public Unit playerUnit;
	public Text numText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		numText.text = playerUnit.GetMealPaks().ToString() + "x";
	}
}
