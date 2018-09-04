using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectProfileHandler : MonoBehaviour {
	public List<Text> profileTexts;
	public MainMenuController mainMenuController;
	// Use this for initialization
	void Start () {
		for(int i = 0; i<5; i++){
			if(PlayerPrefs.GetString("profile" + i.ToString()) == ""){
				profileTexts[i].text = "[empty]";
			}else{
				profileTexts[i].text = PlayerPrefs.GetString("profile" + i.ToString());
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SelectProfile(int i){
		if(PlayerPrefs.GetString("profile" + i.ToString()) == ""){
			//make new profile
			PlayerPrefs.SetString("profile" + i.ToString(), "nameless");
			PlayerPrefs.SetString("profile", i.ToString());
			
			mainMenuController.ShowPanel(1);
		}else{
			//load selection screen with current profile
			PlayerPrefs.SetString("profile", i.ToString());
			SceneManager.LoadScene("LevelSelect");
		}
	}

}
