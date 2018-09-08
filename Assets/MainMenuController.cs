using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour {
	public GameObject centerPanel;
	public List<GameObject> panels;
	// Use this for initialization
	void Start () {
		PlayerPrefs.SetString("profile", "");

		//PlayerPrefs.DeleteAll();
		//PlayerPrefs.SetString("profile0", "");
		//PlayerPrefs.SetString("profile1", "");
		//PlayerPrefs.SetString("profile2", "");
		//PlayerPrefs.SetString("profile3", "");
		//PlayerPrefs.SetString("profile4", "");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void HideAllPanels(){ 
		for(int i = 0; i<panels.Count; i++){
			panels[i].transform.position = new Vector3(1000,1000,0);
		}
	}

	public void ShowPanel(int i){
		HideAllPanels();
		panels[i].transform.position = centerPanel.transform.position;
	}



	void LoadExistingGame(){

	}


}
