using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splatter : MonoBehaviour {
	public List<GameObject> stuff;
	int splatterMin = 3;
	int splatterMax = 6;
	float spreadRange = 2.0f;
	public void Start(){
		SplatterOnGround(transform.position);
	}


	public void SplatterOnGround(Vector2 location){
		int spawns = Random.Range(splatterMin, splatterMax + 1);
		for(int i = 0; i<spawns; i++){
			Instantiate(stuff[0], location + new Vector2(Random.Range(-spreadRange,spreadRange),Random.Range(-spreadRange,spreadRange)), Quaternion.identity);
		}
	}
}
