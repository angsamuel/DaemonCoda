using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InLevelMenuController : MonoBehaviour {
	bool canEscape = true;
	public List<GameObject> menus;
	bool menusShowing = false;
	bool escapeHit = false;
	PlayerInputController pic;
	// Use this for initialization
	void Start () {
		pic = GameObject.Find("PlayerInputController").GetComponent<PlayerInputController>();
		HideAllMenus();
	}
	
	// Update is called once per frame
	void Update () {
		//check for user show menu input
		if(Input.GetAxisRaw("Escape") != 0 && canEscape){
			canEscape = false;
			if(menusShowing == false){
				ShowMenu(0);
			}else{
				HideAllMenus();
			}

		}else if (Input.GetAxisRaw("Escape") == 0){
			canEscape = true;
		}
	}

	public void HideAllMenus(){
		pic.Enable();
		menusShowing = false;
		for(int i = 0; i<menus.Count; i++){
			menus[i].transform.localScale = new Vector3(0,0,0);
		}
	}

	public void ShowMenu(int index){
		HideAllMenus();
		menusShowing = true;
		menus[index].transform.localScale = new Vector3(1,1,1);
		pic.Disable();

	}
	public void QuitToMain(){
		SceneManager.LoadScene("MainMenu");
	}


}
