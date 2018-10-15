using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour {
	public GameObject centerPanel;
	public List<GameObject> panels;

	public List<GameObject> cursor;
	public List<GameObject> cursorTargets;
	// Use this for initialization
	void Start () {
		PlayerPrefs.SetString("profile", "");
		cursorTargets  = new List<GameObject>(GameObject.FindGameObjectsWithTag("cursor target"));
		StartCoroutine(CursorRoutine());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator CursorRoutine(){
		while(true){
			for(int i = 0; i<cursor.Count; i++){
				cursor[i].transform.position = cursorTargets[Random.Range(0, cursorTargets.Count)].transform.position;
			}
			yield return new WaitForSeconds(.35f);
		}
	}

	void HideAllPanels(){ 
		for(int i = 0; i<panels.Count; i++){
			panels[i].transform.position = new Vector3(1000,1000,0);
		}
	}

	public void ShowPanel(int i){
		HideAllPanels();
		panels[i].transform.position = centerPanel.transform.position;
		if(i != 0){
			Color c= cursor[0].GetComponent<Image>().color;
			for(int j = 0; j<cursor.Count; j++){
				cursor[j].GetComponent<Image>().color = new Color(c.r,c.g,c.b,0);
			}
		}else{
			Color c= cursor[0].GetComponent<Image>().color;
			for(int j = 0; j<cursor.Count; j++){
				cursor[j].GetComponent<Image>().color = new Color(c.r,c.g,c.b,1);
			}
		}
	}

	public void PlayTutorial(){
		SceneManager.LoadScene("Tutorial");
	}

	void LoadExistingGame(){

	}

	public void QuitGame(){
		Application.Quit();
	}


}
