using UnityEngine;
using System.Collections;

public class Hideable : IHideable {
    SpriteRenderer sr;
    bool entered = true;
    public void Start(){
        sr = GetComponent<SpriteRenderer>();
    }

    override public void OnFOVEnter() {
        //SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr != null){
            sr.enabled = true;
        }

        if(gameObject.tag == "weapon"){
            GetComponent<Weapon>().blade.GetComponent<SpriteRenderer>().enabled = false;
        }
        
    }

    override public IEnumerator FOVEnterRoutine(){
            entered = true;
            if(sr != null){
                sr.enabled = true;
            }
            if(gameObject.tag == "weapon"){
                GetComponent<Weapon>().blade.GetComponent<SpriteRenderer>().enabled = true;
            }else if(gameObject.tag == "unit"){
                GetComponent<Unit>().body.GetComponent<SpriteRenderer>().enabled = true;
                Weapon w = GetComponent<Unit>().weapon;
                if(w!=null){
                    w.blade.GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        yield return null;
    }

     override public IEnumerator FOVLeaveRoutine(){
            if(sr != null){
                sr.enabled = false;
            }
            if(gameObject.tag == "weapon"){
                GetComponent<Weapon>().blade.GetComponent<SpriteRenderer>().enabled = false;
            }else if(gameObject.tag == "unit"){
                GetComponent<Unit>().body.GetComponent<SpriteRenderer>().enabled = false;
                 Weapon w = GetComponent<Unit>().weapon;
                if(w!=null){
                    w.blade.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        yield return null;
    }

    override public void OnFOVLeave() {
        //SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr != null){
            sr.enabled = false;
        }
        if(tag == "weapon"){
            foreach(Transform child in transform){
                SpriteRenderer src = child.GetComponent<SpriteRenderer>();
                if(src != null){
                    src.enabled = false;
                }
            } 
        }
    }
}
