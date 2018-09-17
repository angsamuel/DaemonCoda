using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinPetal : Spell {
	public GameObject petal;
	int count = 10; 
	// Use this for initialization
	void Start () {
		
	}
	
	override public void CastSpellOnLocation(Unit caster, Vector2 castPosition){
		for(int i = 0; i<count; i++){
			castPosition = new Vector3(castPosition.x, castPosition.y, 0);

			Vector2 spawnPos = Vector2.Perpendicular(UBP(caster.transform.position, castPosition));

			if(Random.Range(0,2) == 1){
				spawnPos = -spawnPos;
			}

			spawnPos *= Random.Range(2.0f, 5.0f);

			spawnPos -= UBP(caster.transform.position, castPosition) * 1.5f;

			
			GameObject newPetal = Instantiate(petal,transform.position, Quaternion.identity);
			newPetal.transform.Translate(spawnPos);
			newPetal.GetComponent<RinPetalProjectile>().Launch(castPosition);
		}
	}

	override public void CastSpellOnTarget(Unit caster, Unit target){

	}

	Vector2 UBP(Vector2 p1, Vector2 p2){
		if((p1 == p2)){
			new Vector3(0,0);
		}
		return (1f / (p1 - p2).magnitude) * (p1 - p2);
	}
}
