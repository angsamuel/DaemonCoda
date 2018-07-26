using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crop : MonoBehaviour {
	public Sprite cutSprite, bushelSprite;
	public GameObject bushel;
	bool isCut = false;
	public float flingTime, flingDistance;


	void OnTriggerEnter2D(Collider2D other) 
	{ 
		if (other.gameObject.tag == "damage source") {
			GetCut ();
		} 
	}

	void GetCut(){
		if(!isCut){
			isCut = true;

			GetComponent<SpriteRenderer>().sprite = cutSprite;
				GameObject newBushel = GameObject.Instantiate(bushel, transform);
				newBushel.GetComponent<SpriteRenderer> ().color = GetComponent<SpriteRenderer> ().color;
				newBushel.transform.localScale = new Vector3(1,1,1);
				FlingBushel(newBushel, flingTime);
		}
	}

	public void FlingBushel(GameObject bushel, float timeToMove)
	{
		bushel.transform.localPosition = new Vector3(0,0,0);
		bushel.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-2.0f,2.0f),Random.Range(-10.0f,10.0f));
	}
}
