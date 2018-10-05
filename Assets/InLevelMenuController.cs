using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InLevelMenuController : MonoBehaviour {

	public List<GameObject> menus;

	// Use this for initialization
	void Start () {
		HideAllMenus();
	}
	
	// Update is called once per frame
	void Update () {
		//check for user show menu input
		if(I)
	}

	public void HideAllMenus(){
		for(int i = 0; i<menus.Count; i++){
			menus[i].transform.localScale = new Vector3(0,0,0);
		}
	}

	public void ShowMenu(int index){
		HideAllMenus();
		menus[index].transform.localScale = new Vector3(1,1,1);
	}


}
