using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {
    public LevelController levelController;

	// Use this for initialization
	void Start () {
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnMouseOver()
    {
        //if mouse is over gameobject with script attached
        //levelController.pic
    }

    private void OnMouseExit()
    {
        //if the mouse exists the gameObject
    }



}
