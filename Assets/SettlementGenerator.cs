using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettlementGenerator : MonoBehaviour {
	NameWizard nameWizard;
	public List<string> weathers;
	public List<string> modifiers;
	List<GameObject> crestPieces;
	// Use this for initialization
	void Awake () {
		nameWizard = GetComponent<NameWizard>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//colony
	//hamlet
	//village
	//town
	//city
	//???

	public Settlement GenerateSettlement(){
		Settlement s = new Settlement();
		s.name = nameWizard.GenerateVillageName();
		s.size = Random.Range(0,5);
		s.modifier = modifiers[Random.Range(0,modifiers.Count)];
		s.weather = weathers[Random.Range(0,weathers.Count)];
		s.crestSeed = Random.Range(-2000000000,2000000000);
		return s;
	}
}
