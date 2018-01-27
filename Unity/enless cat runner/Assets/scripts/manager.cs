using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class manager : MonoBehaviour {

    public string runAnimation = "Run";

    public string jumpAnimation = "Jump";

    public string dieAnimation = "Die";

    enum State { WALKING, JUMPING, DEAD };

    private State state = State.WALKING;

    private UnityArmatureComponent armatureComponent;
    private DragonBones.AnimationState aimState = null;

    // Use this for initialization
    void Start () {
        armatureComponent = GetComponent<UnityArmatureComponent>();
        //armatureComponent.animation.FadeIn("goat_walk_anim", 0.25f, -1);
        armatureComponent.animation.FadeIn("goat_sleep_idle_anim", 0.25f, -1);
    }
	
	// Update is called once per frame
	void Update () {
        if (aimState != null && aimState.weight > 0)
        {
            aimState.weight = Mathf.Lerp(aimState.weight, 0, 0.2f);
        }

    }

    //

    public void Walk(float speed)
    {
        
    }

    public void Jump()
    {

    }

}
