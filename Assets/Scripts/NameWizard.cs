using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameWizard : MonoBehaviour
{
	string[] villageNounArr;
	string[] namesArr;
	string[] adjectiveArr;
	string[] maleNamesArr;
	string[] femaleNamesArr;
	string[] lastNamesArr;
	string[] villageEndingsArr;


	void Awake()
	{
		loadWordLists();
	}
	// Use this for initialization
	void Start()
	{
		for(int i = 0; i<20; i++){
			//Debug.Log(GenerateVillageName());
		}

	}

	// Update is called once per frame
	void Update()
	{

	}

	public string RandomName()
	{
		return namesArr[Random.Range(0, namesArr.Length)];
	}

	public string RandomAdjective()
	{
		return adjectiveArr[Random.Range(0, adjectiveArr.Length)];
	}
	public string RandomVillageNoun()
	{
		return villageNounArr[Random.Range(0, villageNounArr.Length)];
	}
	public string RandomMaleName()
	{
		return maleNamesArr[Random.Range(0, maleNamesArr.Length)];
	}
	public string RandomFemaleName()
	{
		return femaleNamesArr[Random.Range(0, femaleNamesArr.Length)];
	}
	public string RandomLastName()
	{
		return lastNamesArr[Random.Range(0, lastNamesArr.Length)];
	}
	public string RandomVillageEnding(){
		return villageEndingsArr[Random.Range(0, villageEndingsArr.Length)];
	}

	private void loadWordLists()
	{
		TextAsset villageNounsAsset = Resources.Load("Words/village_nouns") as TextAsset;
		TextAsset nounsAsset = Resources.Load("Words/nouns") as TextAsset;
		TextAsset adjectivesAsset = Resources.Load("Words/adj") as TextAsset;
		TextAsset namesAsset = Resources.Load("Words/all_names") as TextAsset;
		TextAsset maleNamesAsset = Resources.Load("Words/male_names") as TextAsset;
		TextAsset femaleNamesAsset = Resources.Load("Words/female_names") as TextAsset;
		TextAsset lastNamesAsset = Resources.Load("Words/last_names") as TextAsset;
		TextAsset villageEndingsAsset = Resources.Load("Words/village_endings") as TextAsset;

		char[] archDelim = new char[] { '\r', '\n' };

		villageNounArr = villageNounsAsset.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
		adjectiveArr = adjectivesAsset.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
		namesArr = namesAsset.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
		maleNamesArr = maleNamesAsset.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
		femaleNamesArr = femaleNamesAsset.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
		lastNamesArr = lastNamesAsset.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
		villageEndingsArr = villageEndingsAsset.text.Split(archDelim, System.StringSplitOptions.RemoveEmptyEntries);
	}

	private void destroyWordLists()
	{
		namesArr = new string[0];
		adjectiveArr = new string[0];
		villageNounArr = new string[0];
		maleNamesArr = new string[0];
		femaleNamesArr = new string[0];
		lastNamesArr = new string[0];
		villageEndingsArr = new string[0];
		Resources.UnloadUnusedAssets();
	}

	public string GenerateVillageName()
	{
		int f = Random.Range(0, 10);
		string name = "";
		switch (f)
		{
		case 0:
			name = RandomAdjective() + RandomVillageEnding();
			break;
		case 1:
			name = RandomName() + RandomVillageEnding();
			break;
		case 2:
			name = RandomName() + "\'s " + RandomAdjective() + " " + RandomVillageNoun();
			break;
		case 3:
			name = RandomName() + "\'s " + RandomVillageNoun();
			break;
		case 4:
			name = RandomName();
			break;
		case 5:
			name = RandomVillageNoun() + " " + RandomAdjective();
			break;
		case 6:
			name = RandomVillageNoun() + " of " + RandomAdjective();
			break;
		case 7:
			name = RandomVillageNoun() + " of " + RandomName();
			break;
		case 8:
			name = RandomAdjective() + " and " + RandomAdjective();
			break;
		case 9:
			name = RandomName() + " " + RandomName();
			break;
		default:
			name = "";
			break;
		}
		int putAdjInFront = Random.Range(0,2);
		if(putAdjInFront == 0){
			name = RandomAdjective() + " " + name;
		}
		int putTheInFront = Random.Range(0,2);
		if(putTheInFront == 0){
			name = "The " + name;
		}

		return name;
	}

}
