using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrestGenerator : MonoBehaviour {
	public List<GameObject> components;
	public List<GameObject> rims;
	// Use this for initialization
	void Start () {

		CreateCrest();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateCrest(){
		int comNum = Random.Range(3, 6);
		Color colorBase = new Color(Random.Range(.25f, 1f), Random.Range(.1f, 1f), Random.Range(.1f, 1f));
		for(int i = 0; i<comNum;i++){
			GameObject newComp = Instantiate(components[Random.Range(0,components.Count)], transform);
			newComp.transform.localPosition = new Vector3(0,0,0);
			float colorOffset = Random.Range(-.2f, .2f);
			newComp.GetComponent<SpriteRenderer>().color = new Color(colorBase.r + colorOffset, colorBase.g  + colorOffset, colorBase.b  + colorOffset);
			
			Vector3 rotation = new Vector3(0,0,1) * (45 * Random.Range(0,8));
			newComp.transform.Rotate(rotation);
			Debug.Log(rotation);
		}
	}
}
