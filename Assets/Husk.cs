using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Husk : UnitController {
    bool moving = true;
	// Use this for initialization
	void Start () {
        base.Start();
        StartCoroutine(WanderRoutine());
	}
	
	// Update is called once per frame
	void Update () {
        //base.Update();

	}

    IEnumerator WanderRoutine()
    {
        yield return new WaitForSeconds(3);
        while (moving)
        {
            unit.Move(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));

            unit.Stop();
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
        }
    }
}
