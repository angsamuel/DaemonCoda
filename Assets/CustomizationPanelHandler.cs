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

	public void UpdatePlayerColorInLevel(){
		string profile = PlayerPrefs.GetString("profile");
		PlayerPrefs.SetFloat(profile + "R", red.value);
		PlayerPrefs.SetFloat(profile +"G", green.value);
		PlayerPrefs.SetFloat(profile +"B", blue.value);

		LevelController lc = GameObject.Find("LevelController").GetComponent<LevelController>();
		SpriteRenderer ps = lc.pic.playerUnit.body.GetComponent<SpriteRenderer>();
		SpriteRenderer ss = lc.pic.playerUnit.shield.GetComponent<SpriteRenderer>();

		ps.color = new Color(red.value, green.vaue, blue.value);
		ss.color = new Color(red.value, green.vaue, blue.value);

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
