using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matilda : UnitController {
    public float backupDistance;
    public List<UnitController> homies;
    bool firstAgro = true;
    // Use this for initialization
    void Start() {
        base.Start();
        myHealth = unit.health;
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
            //unit.PauseMovement(.5f);
            unit.CastSpell(target.transform.position);
            yield return new WaitForSeconds(unit.health*2);
            canCastSpell = true;
        }
    }

    public void CastFreeSpell(){
            //unit.PauseMovement(.5f);
        unit.CastSpell(target.transform.position);
    }

    bool ha = false;
    public void ActivateHomies(){
        if(ha == false){
            ha = true;
            for(int i = 0; i<homies.Count; i++){
                homies[i].husked = false;
                homies[i].target = target;
            }
        }   
    }

    bool hr = false;
    IEnumerator HurtRoutine(){
        if(!hr){
            Debug.Log("HR");
            hr = true;
            unit.UnpauseMovement();
            int ss = Random.Range(0,4);
            Vector3 sp = unit.transform.position;
            if(ss == 0){
                sp = unit.transform.position + new Vector3(-1000,0,0);
            }else if(ss == 1){
                sp = unit.transform.position + new Vector3(1000,0,0);
            }else if(ss == 2){
                sp = unit.transform.position + new Vector3(0,1000,0);
            }else if (ss == 3){
                sp = unit.transform.position + new Vector3(0,-1000,0);
            }
            
            unit.Dashes(13);

            for(int i = 0; i<26;i++){
                unit.MoveToward(sp);
                yield return new WaitForSeconds(.25f);
                if(i < 14){
                    CastFreeSpell();
                }
            }
            unit.Stop();
            hr = false;
        }

    }

    int myHealth = 0;

    override public void CustomActions()
    {
        ActivateHomies();
        if(unit.health != myHealth){
            myHealth = unit.health;
            StartCoroutine(HurtRoutine());
        }

        myHealth = unit.health;



        if (!unit.dead && unit.weapon != null && !hr)
        {
            Debug.Log("still happen");


            

            if(firstAgro){
                unit.PauseMovement(1.5f);
                unit.PauseAttack(1.5f);
                firstAgro = false;
            }
            if(Vector3.Distance(target.transform.position, transform.position) < 10 && Vector3.Distance(target.transform.position, transform.position) > 5){
                if(canCastSpell){
                    StartCoroutine(CastSpell());  
                    Debug.Log("casting spell");
                }          
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
