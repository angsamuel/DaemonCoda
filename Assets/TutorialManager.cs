using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialManager : MonoBehaviour {
	public Text tutorialText;
	public Text buttonText;
	public List<string> messages;
	public GameObject wall;
	public GameObject door;
	public GameObject button;
	public Unit enemy;
	public GameObject medPak;
	public GameObject mealPak;
	int tutorialIndex = 0;
	// Use this for initialization
	void Start () {
		tutorialText.text = messages[tutorialIndex];
		enemy.gameObject.SetActive(false);
		medPak.SetActive(false);
		mealPak.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TutorialClick(){
		Debug.Log("clicc");
		tutorialIndex += 1;
		tutorialText.text = messages[tutorialIndex];
		GameObject myEventSystem = GameObject.Find("EventSystem");
     	myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
		button.SetActive(true);

		if(tutorialIndex == 11){
			door.SetActive(true);
			wall.SetActive(false);
			button.SetActive(false);
			enemy.gameObject.SetActive(true);
			StartCoroutine(WaitForEnemyToDie());
		}else if(tutorialIndex == 17){
			medPak.SetActive(true);
			mealPak.SetActive(true);
			medPak.GetComponent<SpriteRenderer>().enabled = false;
			mealPak.GetComponent<SpriteRenderer>().enabled = false;
			StartCoroutine(Dumbo());
			button.SetActive(false);
			StartCoroutine(WaitForPakPickup());
		}else if(tutorialIndex == messages.Count-1){
			button.SetActive(false);
			LeaveTutorial();
		}
	}

	IEnumerator Dumbo(){
		yield return new WaitForSeconds(.2f);
		medPak.GetComponent<SpriteRenderer>().enabled = true;
		mealPak.GetComponent<SpriteRenderer>().enabled = true;
	}

	void LeaveTutorial(){
		
	}

	IEnumerator WaitForEnemyToDie(){
		while(!enemy.dead){
			yield return new WaitForSeconds(.5f);
		}
		TutorialClick();
	}

	IEnumerator WaitForPakPickup(){
		while(medPak != null || mealPak != null){
			yield return new WaitForSeconds(.5f);
		}
		TutorialClick();
	}
}
