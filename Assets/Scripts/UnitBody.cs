using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBody : MonoBehaviour {
	Animator animator;
	SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	public void Run(){
		animator.SetInteger ("State", 1);
	}

	public void Idle(){
		animator.SetInteger ("State", 0);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
