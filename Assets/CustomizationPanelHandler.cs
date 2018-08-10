using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationPanelHandler : MonoBehaviour {
	public Scrollbar red;
	public Scrollbar green;
	public Scrollbar blue;
	public Text nameText;
	public Image playerImage;
	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
		playerImage.color = new Color(red.value, green.value, blue.value, 1);
	}


	public void StartNewGame(){
		PlayerPrefs.SetFloat(PlayerPrefs.GetString("profile") + "R", red.value);
		PlayerPrefs.SetFloat(PlayerPrefs.GetString("profile") +"G", green.value);
		PlayerPrefs.SetFloat(PlayerPrefs.GetString("profile") +"B", blue.value);
		PlayerPrefs.SetString(PlayerPrefs.GetString("profile") +"name", nameText.text);
	}
}
