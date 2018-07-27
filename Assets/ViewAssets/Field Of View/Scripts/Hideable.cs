using UnityEngine;

public class Hideable : MonoBehaviour, IHideable {

    public void OnFOVEnter() {
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
    public void OnFOVLeave() {
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
