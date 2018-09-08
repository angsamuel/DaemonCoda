using UnityEngine;
using System.Collections;
public class ReverseHideable : IHideable {
    SpriteRenderer sr;
    public void Start(){
        sr = GetComponent<SpriteRenderer>();
    }
    override public IEnumerator FOVEnterRoutine(){
        yield return null;
        if(sr != null){
            sr.enabled = false;
        }
    }

     override public IEnumerator FOVLeaveRoutine(){
        yield return null;
        if(sr != null){
            sr.enabled = true;
        }
    }

    override public void OnFOVLeave() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr != null){
            sr.enabled = true;
        }
        
        foreach(Transform child in transform){
            sr = child.GetComponent<SpriteRenderer>();
            if(sr != null){
                sr.enabled = true;
            }
            if(child.tag == "weapon"){
                foreach(Transform blade in child){
                    blade.GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }
    }
    override public void OnFOVEnter() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if(sr != null){
            sr.enabled = false;
        }
        foreach(Transform child in transform){
            sr = child.GetComponent<SpriteRenderer>();
            if(sr != null){
                sr.enabled = false;
            }

            if(child.tag == "weapon"){
                foreach(Transform blade in child){
                    blade.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
    }
}
