using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettlementGenerator : MonoBehaviour {
	public NameWizard nameWizard;

	public List<string> modifiers;
	// Use this for initialization
	void Start () {
		//nameWizard = GetComponent<NameWizard>();
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
		s.weather = "Clear";
		s.crestSeed = Random.Range(-2000000000, 2000000000);
		s.modifier = modifiers[Random.Range(0,modifiers.Count)];		
		return s;
	}

	
}
