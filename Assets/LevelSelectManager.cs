using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LevelSelectManager : MonoBehaviour {
	public SettlementGenerator sg;
	List<Settlement> settlements = new List<Settlement>();
	public CrestGenerator cg;
	public Text nameText;
	public Text modifierText;
	public Text weatherText;
	public Text sizeText;
	
	public Text distanceLabel;
	public Text distanceText;

	public Text signText;

	public Text mealPakLabel;
	public Text mealPakText;
	public List<Text> buttonOptions;

	public Color error;
	public Color success; 

	public GameObject travelPanel;
	public GameObject framePanel;
	public GameObject coverPanel;

	public GameObject travelButton;
	public Text notEnoughFoodText;
	public GameObject entirePanel;

	int mealPaks;

	Vector3 travelPosition;
	Vector3 framePosition;
	Vector3 coverPosition;


	// Use this for initialization
	void Start () {
		LoadSettlements();
		mealPaks = PlayerPrefs.GetInt(PlayerPrefs.GetString("profile") + "mealPaks");
	}	
	
	// Update is called once per frame
	void Update () {
		
	}

	int travelSelection = -1;

	void LoadSettlementsFromPlayerPrefs(){
		string profile = PlayerPrefs.GetString("profile");

		for(int i = 0; i<3; i++){
			Settlement newSett = new Settlement();
			newSett.name = PlayerPrefs.GetString(profile + i.ToString() + "name");
			newSett.size = PlayerPrefs.GetInt(profile + i.ToString() + "size");
			newSett.modifier = PlayerPrefs.GetString(profile + i.ToString() + "modifier");
			newSett.weather = PlayerPrefs.GetString(profile + i.ToString() + "weather");
			newSett.crestSeed = PlayerPrefs.GetInt(profile + i.ToString() + "crestSeed");
			newSett.distance = PlayerPrefs.GetInt(profile + i.ToString() + "distance");


			settlements.Add(newSett);
		}
	}


	void LoadSettlements(){
		framePosition = framePanel.transform.position;
		travelPosition = travelPanel.transform.position;
		coverPosition = coverPanel.transform.position;
		framePanel.transform.position = new Vector3(1000,1000,1000);
		travelPanel.transform.position = new Vector3(1000,1000,1000);

		if(PlayerPrefs.GetString(PlayerPrefs.GetString("profile") + "settlements saved")!=""){
			//load settlments from player prefs
			LoadSettlementsFromPlayerPrefs();
		}else{
			//generate 3 new settlements
			GenerateNewSelections();
			PlayerPrefs.SetString(PlayerPrefs.GetString("profile") + "settlements saved", "saved from level select"); 
		}

		for(int i = 0; i<3; i++){
			buttonOptions[i].text = settlements[i].name;
		}
	}

	public void SelectForTravel(int i){
		travelSelection = i;
		travelPanel.transform.position = travelPosition;
	}

	public void CancelSelection(){
		coverPanel.transform.position = coverPosition;
		travelPanel.transform.position = new Vector3(1000,1000,1000);
	}

	public void FinalizeTravel(){
		mealPaks -= s.distance;
		
		//save mealpaks
		PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "mealPaks", mealPaks);
		entirePanel.transform.position = new Vector3(1000,1000,1000);
		StartCoroutine(LoadNextLevel());
		//spawn overlay in


	}

	IEnumerator LoadNextLevel(){
		//add playerprefs

		//s is our settlement
		string profile = PlayerPrefs.GetString("profile");
		PlayerPrefs.SetString(profile + "settlement name", s.name);
		PlayerPrefs.SetInt(profile + "settlement size", s.size);
		PlayerPrefs.SetInt(profile + "settlement distance", s.distance);
		PlayerPrefs.SetString(profile + "settlement modifier", s.modifier);
		PlayerPrefs.SetString(profile + "settlement weather", s.weather);
		PlayerPrefs.SetInt(profile + "settlement crest seed", s.crestSeed);

		PlayerPrefs.SetInt(profile + "mealPaks", mealPaks);

		yield return new WaitForSeconds(.5f);

		SceneManager.LoadScene("Level");

	}

	Settlement s;

	public void FillUI(int slot){
		travelSelection = slot;
		coverPanel.transform.position = new Vector3(1000,1000,1000);
		framePanel.transform.position = framePosition;

		s = settlements[slot];
		nameText.text = s.name;
		modifierText.text = s.modifier;
		weatherText.text = s.weather;
		distanceText.text = s.distance.ToString();
		mealPakText.text = mealPaks.ToString();

		if(s.distance > mealPaks){
			signText.text = ">";
			signText.color = error;
			distanceText.color = error;
			distanceLabel.color = error;
			mealPakText.color = error;
			mealPakLabel.color = error;
		}else if(s.distance < mealPaks){
			signText.text = "<";
			signText.color = success;
			distanceText.color = success;
			distanceLabel.color = success;
			mealPakText.color = success;
			mealPakLabel.color = success;
		}else if(s.distance == mealPaks){
			signText.text = "=";
			signText.color = success;
			distanceText.color = success;
			distanceLabel.color = success;
			mealPakText.color = success;
			mealPakLabel.color = success;
		}

		if(s.distance > mealPaks){
			travelButton.active = false;
		}else{
			travelButton.active = true;
		}

		string size = "";
		if(s.size == 0){
			size = "Colony";
		}else if(s.size == 1){
			size = "Hamlet";
		}else if(s.size == 2){
			size = "Village";
		}else if(s.size == 3){
			size = "Town";
		}else if(s.size == 4){
			size = "City";
		}
		sizeText.text = size;
		cg.GenerateCrest(s.crestSeed);

	}

	void GenerateNewSelections(){
		for(int i = 0; i<3; i++){
			Settlement newSett = sg.GenerateSettlement();
			newSett.distance = Random.Range(i+1, (i+1)*3);
			settlements.Add(newSett);
			SaveSettlement(newSett, i);
		}
	}

	void SaveSettlement(Settlement sett, int i){
		string profile = PlayerPrefs.GetString("profile");
		string slot = i.ToString();
		string front = profile+slot;
		PlayerPrefs.SetString(front+"name", sett.name);
		PlayerPrefs.SetInt(front+"distance",sett.distance);
		PlayerPrefs.SetInt(front+"size", sett.size);
		PlayerPrefs.SetString(front+"modifier", sett.modifier);
		PlayerPrefs.SetString(front+"weather", sett.weather);
		PlayerPrefs.SetInt(front+"crestSeed", sett.crestSeed);
	}

}
