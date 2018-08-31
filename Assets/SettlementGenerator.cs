using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettlementGenerator : MonoBehaviour {
	NameWizard nameWizard;

	public List<string> modifiers;
	// Use this for initialization
	void Start () {
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

	public void GenerateSettlement(){
		Settlement s = new Settlement();
		s.name = nameWizard.GenerateVillageName();
		s.size = Random.Range(0,5);
		s.modifier = modifiers[Random.Range(0,modifiers.Count)];
		

	}

	
}
