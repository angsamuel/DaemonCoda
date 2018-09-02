using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : UnitController {
    public float backupDistance;

    bool firstAgro = true;
    // Use this for initialization
    void Start() {
        base.Start();
    }

    void Update()
    {
        //base.Update();
        //if(target!=null){
        //    Debug.Log("has target");
        //}
    }

    void LateUpdate(){
        base.LateUpdate();
    }

    override public void CustomActions()
    {
        if (!unit.dead && unit.weapon != null)
        {

            if(firstAgro){
                unit.PauseMovement(1f);
                unit.PauseAttack(1f);
                firstAgro = false;
            }


            unit.weapon.Aim(target.transform.position);
            float distance = Vector3.Distance(unit.transform.position, target.transform.position);
            if (distance < strikingDistance && distance > backupDistance)
            {
                unit.AttackWithWeapon();
                unit.PauseMovement(1.5f);
                unit.PauseAttack(1.5f);
                unit.Stop();
            }
            else if (Vector3.Distance(unit.transform.position, target.transform.position) <= backupDistance)
            {
                unit.MoveAway(target.transform.position);
            }
            else
            {
                unit.MoveToward(target.transform.position);
            }
        }
    }
}
