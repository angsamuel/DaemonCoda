using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {
	bool ShadowOn = true;
	public List<GameObject> shadeBlocks;
	public List<Vector2> positions;
	public GameObject shadeTile;

	float fadeTime = 0.5f;
	int fadeDirection = 1;
	float alpha = 1.0f;
	float minAlpha = 0.0f;
	float fadeIncrement = 0.01f;
	bool inRoom;
	public int x;

	void Awake(){
		shadeBlocks = new List<GameObject>();
		positions = new List<Vector2>();

	}

	void Start(){
		StartCoroutine(OutOfRoomRoutine());
		StartCoroutine(Fade());
	}

	IEnumerator OutOfRoomRoutine(){
		while(true){
			yield return new WaitForSeconds(0.25f);
			FadeToDark();
		}

	}

	IEnumerator Fade(){
		while(true){
			//Debug.Log( "blocks " + shadeBlocks.Count);
			yield return new WaitForSeconds(fadeIncrement * fadeTime);

			if(alpha <= 1.0f && alpha > minAlpha && fadeDirection == -1){
				for(int i = 0; i<shadeBlocks.Count; i++){
					Color c = shadeBlocks[i].GetComponent<SpriteRenderer>().color;
					shadeBlocks[i].GetComponent<SpriteRenderer>().color = new Color(c.r,c.g,c.b,alpha);
				}
			}

			else if(alpha >= minAlpha && alpha < 1.0f && fadeDirection == 1){
				
				for(int i = 0; i<shadeBlocks.Count; i++){
					Color c = shadeBlocks[i].GetComponent<SpriteRenderer>().color;
					shadeBlocks[i].GetComponent<SpriteRenderer>().color = new Color(c.r,c.g,c.b,alpha);
				}
			}

			
			
			alpha += fadeIncrement * fadeDirection;
			
			if(alpha > 1.0f){
				alpha = 1.0f;
			}
			if(alpha < minAlpha){
				alpha = minAlpha;
			}

			
		}
	}
	int fadingToClear = 0;
	public void FadeToClear(){
		fadeDirection = -1;
		fadingToClear += 1;
		if(fadingToClear > 3){
			fadingToClear = 3;
		}
	}

	public void FadeToDark(){
		if(fadingToClear < 1){
			fadeDirection = 1;
		}else{
			fadingToClear -=1;
			if(fadingToClear < 0){
				fadingToClear = 0;
			}
		}
	}


	public void AddShadow(GameObject shadow){
		if(!positions.Contains(shadow.transform.position)){
			shadeBlocks.Add(shadow);
			positions.Add(shadow.transform.position);
		}else{
			Destroy(shadow);
		}
		x++;
	}

	public void FadeToClearOld(){
		for(int i = 0; i<shadeBlocks.Count; i++){
			shadeBlocks[i].GetComponent<Shade>().FadeToClear();
		}
	}

	public void FadeToDarkOld(){
		for(int i = 0; i<shadeBlocks.Count; i++){
			shadeBlocks[i].GetComponent<Shade>().FadeToDark();
		}
	}

	
}
