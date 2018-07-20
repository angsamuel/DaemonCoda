using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
	bool ShadowOn = true;
	public List<GameObject> shadeBlocks;
	public GameObject shadeTile;
	public void Start(){
		shadeBlocks = new List<GameObject>();
	}

	public void AddShadow(GameObject shadow){
		shadeBlocks.Add(shadow);
	}

	public void FadeToClear(){
		for(int i = 0; i<shadeBlocks.Count; i++){
			shadeBlocks[i].GetComponent<Shade>().FadeToClear();
		}
	}

	public void FadeToDark(){
		for(int i = 0; i<shadeBlocks.Count; i++){
			shadeBlocks[i].GetComponent<Shade>().FadeToDark();
		}
	}

	public void FillShade(GameObject floor){

	}
}
