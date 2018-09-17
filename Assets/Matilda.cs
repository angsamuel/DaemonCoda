using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matilda : UnitController {
    public float backupDistance;

    bool firstAgro = true;
    // Use this for initialization
    void Start() {
        base.Start();
        unit.weapon.blade.GetComponent<SpriteRenderer>().color = unit.body.GetComponent<SpriteRenderer>().color;
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
        unit.AimWeapon(playerUnit.transform.position);
    }
    bool canCastSpell = true;
    public IEnumerator CastSpell(){
        if(canCastSpell){
            canCastSpell=false;
            unit.CastSpell(target.transform.position);
            yield return new WaitForSeconds(unit.health*2);
            canCastSpell = true;
        }
    }

    override public void CustomActions()
    {
        if (!unit.dead && unit.weapon != null)
        {

            if(firstAgro){
                unit.PauseMovement(1.5f);
                unit.PauseAttack(1.5f);
                firstAgro = false;
            }
            if(Vector3.Distance(target.transform.position, transform.position) < 10 && Vector3.Distance(target.transform.position, transform.position) > 5){
                StartCoroutine(CastSpell());
                unit.PauseMovement(1.5f);
            
            }



            unit.weapon.Aim(target.transform.position);
            float distance = Vector3.Distance(unit.transform.position, target.transform.position);
            if (distance < strikingDistance && distance > backupDistance)
            {
                unit.AttackWithWeapon();
                unit.PauseMovement(2f);
                unit.PauseAttack(2f);
                unit.Stop();
            }
            else if (Vector3.Distance(unit.transform.position, target.transform.position) <= backupDistance)
            {
                unit.MoveAway(target.transform.position);
            }
            else if(Vector3.Distance(unit.transform.position, target.transform.position) > strikingDistance)
            {
                unit.MoveToward(target.transform.position);
            }
        }
    }
}
