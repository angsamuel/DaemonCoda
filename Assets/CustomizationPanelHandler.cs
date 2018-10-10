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
	SpriteRenderer playerSprite;
	SpriteRenderer shieldSprite;
	LevelController lc;
	// Use this for initialization
	void Start () {
		lc = GameObject.Find("LevelController").GetComponent<LevelController>();
		playerSprite = lc.playerUnit.body.GetComponent<SpriteRenderer>();
		shieldSprite = lc.playerUnit.shield.plate.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		playerImage.color = new Color(red.value, green.value, blue.value, 1);

	}

	public void UpdatePlayerColorInLevel(){
		string profile = PlayerPrefs.GetString("profile");
		PlayerPrefs.SetFloat(profile + "R", red.value);
		PlayerPrefs.SetFloat(profile +"G", green.value);
		PlayerPrefs.SetFloat(profile +"B", blue.value);

		playerSprite.color = new Color(red.value, green.value, blue.value);
		shieldSprite.color = new Color(red.value, green.value, blue.value);

	}
	public void GrabPlayerColor(){
		red.value = playerSprite.color.r;
		green.value = playerSprite.color.g;
		blue.value = playerSprite.color.b;
	}





	public void StartNewGame(){

		string profile = PlayerPrefs.GetString("profile");

		PlayerPrefs.SetFloat(profile + "R", red.value);
		PlayerPrefs.SetFloat(profile +"G", green.value);
		PlayerPrefs.SetFloat(profile +"B", blue.value);

		PlayerPrefs.SetString("profile" + profile, nameText.text);
		PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "mealPaks", 3);
		PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "distance to capital", 9);
		PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "health", 3);
		PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "armoryIndex",1);
		//Debug.Log(PlayerPrefs.GetString("profile"));
		SceneManager.LoadScene("LevelSelect");	
	}
}
