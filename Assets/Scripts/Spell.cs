using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour{

	public void Start(){

	}

	virtual public void CastSpellOnLocation(Unit caster, Vector2 castPosition){

	}

	virtual public void CastSpellOnTarget(Unit caster, Unit target){

	}

	public Vector3 UBP(Vector3 p1, Vector3 p2){
		if((p1 == p2)){
			new Vector3(0,0);
		}
		return (1f / (p1 - p2).magnitude) * (p1 - p2);
	}
}
