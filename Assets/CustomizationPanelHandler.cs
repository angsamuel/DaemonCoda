using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

		string profile = PlayerPrefs.GetString("profile");

		PlayerPrefs.SetFloat(profile + "R", red.value);
		PlayerPrefs.SetFloat(profile +"G", green.value);
		PlayerPrefs.SetFloat(profile +"B", blue.value);
		Debug.Log(PlayerPrefs.GetString("Profile"));

		PlayerPrefs.SetString("profile" + profile, nameText.text);
		PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "mealPaks", 3);

		//Debug.Log(PlayerPrefs.GetString("profile"));
		SceneManager.LoadScene("LevelSelect");	
	}
}
