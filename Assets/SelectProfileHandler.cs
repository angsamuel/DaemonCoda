using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectProfileHandler : MonoBehaviour {
	public List<Text> profileTexts;
	public MainMenuController mainMenuController;
	// Use this for initialization
	void Start () {
		for(int i = 0; i<5; i++){
			if(PlayerPrefs.GetString(i.ToString()) == ""){
				profileTexts[i].text = "[empty]";
			}else{
				Debug.Log(PlayerPrefs.GetString(i.ToString()));
				profileTexts[i].text = PlayerPrefs.GetString(i.ToString());
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SelectProfile(int i){
		if(PlayerPrefs.GetString(i.ToString()) == ""){
			//we have an empty profile, go to create character
			PlayerPrefs.SetString("profile", i.ToString());
		}else{
			//we have an old profile, load the save game

		}
	}

}
