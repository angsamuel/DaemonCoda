using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrestGenerator : MonoBehaviour {
	public List<GameObject> components;
	public List<GameObject> rims;


	List<GameObject> activePieces;
	// Use this for initialization
	void Awake () {
		activePieces = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GenerateCrest(int seed){

		if(activePieces.Count > 0){
			for(int i = 0; i<activePieces.Count; i++){
				Destroy(activePieces[i]);
			}
		}

		Random.seed = seed;

		Color colorBase = new Color(Random.Range(.25f, 1f), Random.Range(.1f, 1f), Random.Range(.1f, 1f));

		GameObject rim = Instantiate(rims[Random.Range(0,rims.Count)], transform);
		
		activePieces.Add(rim);
		
		float colorOffsetRim = Random.Range(-.2f, .2f);
		rim.GetComponent<SpriteRenderer>().color = new Color(colorBase.r + colorOffsetRim, colorBase.g  + colorOffsetRim, colorBase.b  + colorOffsetRim);
		Vector3 rimRotation = new Vector3(0,0,1) * (45 * Random.Range(0,8));
		rim.transform.Rotate(rimRotation);

		int comNum = Random.Range(3, 5);


		for(int i = 0; i<comNum;i++){
			GameObject newComp = Instantiate(components[Random.Range(0,components.Count)], transform);
			activePieces.Add(newComp);
			newComp.transform.localPosition = new Vector3(0,0,0);
			float colorOffset = Random.Range(-.2f, .2f);
			newComp.GetComponent<SpriteRenderer>().color = new Color(colorBase.r + colorOffset, colorBase.g  + colorOffset, colorBase.b  + colorOffset);
			
			Vector3 rotation = new Vector3(0,0,1) * (45 * Random.Range(0,8));
			newComp.transform.Rotate(rotation);

		}
		Random.seed = System.Environment.TickCount;
	}
}
