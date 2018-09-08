using UnityEngine;
using System.Collections;

public abstract class IHideable : MonoBehaviour {

    public virtual void OnFOVEnter(){

    }
    public virtual void OnFOVLeave(){

    }
    public virtual  IEnumerator FOVEnterRoutine(){
        yield return null;
    }
    public virtual IEnumerator FOVLeaveRoutine(){
        yield return null;
    }
}
