using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {
	public GameObject centerPanel;
	public List<GameObject> panels;
	// Use this for initialization
	void Start () {
		PlayerPrefs.SetString("profile", "");
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
}
