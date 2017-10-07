using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {
	SpriteRenderer mySprite;
	public float expandTime;
	public float expandSize;
	Color initialColor = Color.white;
	// Use this for initialization
	void Start () {
		mySprite = GetComponent<SpriteRenderer> ();
		StartCoroutine (ExpandRoutine());
	}

	public void SetInitialColor(Color c){
		initialColor = c;
	}
	IEnumerator ExpandRoutine(){
		float t = 0;
		while (t < 1) {
			t += Time.deltaTime / expandTime;
			transform.localScale = new Vector2 (Remap((t/1), 0, 1, 0, expandSize) ,Remap((t/1), 0, 1, 0, expandSize));
			mySprite.color = new Color (initialColor.r, initialColor.g, initialColor.b, 1 - t); 
			yield return null;
		}
		Destroy (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public float Remap (float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}
}
