using UnityEngine;
using System.Collections;

public class Hideable : IHideable {
    SpriteRenderer sr;
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
        yield return null;
        if(sr != null){
            sr.enabled = true;
        }
        if(gameObject.tag == "weapon"){
            GetComponent<Weapon>().blade.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

     override public IEnumerator FOVLeaveRoutine(){
        yield return null;
        if(sr != null){
            sr.enabled = false;
        }
        if(gameObject.tag == "weapon"){
            GetComponent<Weapon>().blade.GetComponent<SpriteRenderer>().enabled = false;
        }
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
