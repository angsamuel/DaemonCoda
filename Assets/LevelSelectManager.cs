using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


	int mealPaks;

	Vector3 travelPosition;
	Vector3 framePosition;
	Vector3 coverPosition;


	// Use this for initialization
	void Start () {
		LoadSettlements();
		PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "mealPaks", 3);
		mealPaks = PlayerPrefs.GetInt(PlayerPrefs.GetString("profile") + "mealPaks");
	}	
	
	// Update is called once per frame
	void Update () {
		
	}

	int travelSelection = -1;

	void LoadSettlements(){
		framePosition = framePanel.transform.position;
		travelPosition = travelPanel.transform.position;
		coverPosition = coverPanel.transform.position;
		framePanel.transform.position = new Vector3(1000,1000,1000);
		travelPanel.transform.position = new Vector3(1000,1000,1000);

		if(PlayerPrefs.GetString(PlayerPrefs.GetString("profile") + "settlements saved")!=""){
			//load villages in player prefs
		}else{
			//generate 3 new villages
			GenerateNewSelections();
			for(int i = 0; i<3; i++){
				buttonOptions[i].text = settlements[i].name;
			}
			
			//FillUI(0);
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


	public void FillUI(int slot){
		travelSelection = slot;
		coverPanel.transform.position = new Vector3(1000,1000,1000);
		framePanel.transform.position = framePosition;

		Settlement s = settlements[slot];
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
			Settlement s = sg.GenerateSettlement();
			settlements.Add(s);
			SaveSettlement(s, i);
			s.distance = Random.Range(i+1, (i+1)*3);
		}
	}

	void SaveSettlement(Settlement s, int i){
		string profile = PlayerPrefs.GetString("profile");
		string slot = i.ToString();
		string front = profile+slot;
		PlayerPrefs.SetString(front+"name", s.name);
		PlayerPrefs.SetInt(front+"size", s.size);
		PlayerPrefs.SetString(front+"modifier", s.modifier);
		PlayerPrefs.SetString(front+"weather", s.weather);
		PlayerPrefs.SetInt(front+"crestSeed", s.crestSeed);
	}

}
