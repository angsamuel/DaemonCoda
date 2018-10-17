using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialManager : MonoBehaviour {
	public Text tutorialText;
	public Text notificationText;
	public List<string> messages;
	public GameObject wall;
	public GameObject door;
	public GameObject button;
	public Unit enemy;
	public GameObject medPak;
	public GameObject mealPak;
	int tutorialIndex = 0;
	bool canClick = true;
	// Use this for initialization
	void Start () {
		tutorialText.text = messages[tutorialIndex];
		enemy.gameObject.SetActive(false);
		medPak.SetActive(false);
		mealPak.SetActive(false);
		StartCoroutine(EnableClick());
	}

	IEnumerator EnableClick(){
		canClick = false;
		yield return new WaitForSeconds(3);
		canClick = true;

	}
	
	// Update is called once per frame
	bool canEnter = true;
	void Update () {
		if(Input.GetAxisRaw("Enter") != 0 && canEnter){
			canEnter = false;
			TutorialClick();
		}else if(Input.GetAxisRaw("Enter") == 0){
			canEnter = true;
		}
	}

	public void TutorialClick(){
		if(canClick){
			notificationText.color = Color.white;
			tutorialIndex += 1;
			tutorialText.text = messages[tutorialIndex];
			GameObject myEventSystem = GameObject.Find("EventSystem");
			myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);

			if(tutorialIndex == 11){
				door.SetActive(true);
				wall.SetActive(false);
				canClick = false;
				notificationText.color = Color.clear;
				enemy.gameObject.SetActive(true);
				StartCoroutine(WaitForEnemyToDie());
			}else if(tutorialIndex == 17){
				medPak.SetActive(true);
				mealPak.SetActive(true);
				medPak.GetComponent<SpriteRenderer>().enabled = false;
				mealPak.GetComponent<SpriteRenderer>().enabled = false;
				StartCoroutine(Dumbo());
				canClick = false;
				notificationText.color = Color.clear;
				StartCoroutine(WaitForPakPickup());
			}else if(tutorialIndex == messages.Count-1){
				canClick = false;
				notificationText.color = Color.clear;
				LeaveTutorial();
			}
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
		canClick = true;
		TutorialClick();
	}

	IEnumerator WaitForPakPickup(){
		while(medPak != null || mealPak != null){
			yield return new WaitForSeconds(.5f);
		}
		canClick = true;
		TutorialClick();
	}
}
