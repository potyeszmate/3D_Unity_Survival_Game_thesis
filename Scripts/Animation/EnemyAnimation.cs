using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour {

    private Animator anim;

	void Awake () 
    {
        anim = GetComponent<Animator>();	
	}
	
    // Walk animation turning on - Used in Enemy controllers
    public void Walk(bool walk) 
    {
        // Using the animation tags from the AnimationTags script -> just for the enemies
        anim.SetBool(AnimationTags.WALK_PARAMETER, walk);
    }

    // Run animation turning on - Used in Enemy controllers
    public void Run(bool run) 
    {
        anim.SetBool(AnimationTags.RUN_PARAMETER, run);
    }

    // Attack animation turning on - Used in Enemy controllers
    public void Attack() 
    {
        anim.SetTrigger(AnimationTags.ATTACK_TRIGGER);
    }

    // Dead animation turning on - Used in Enemy controllers
    public void Dead() 
    {
        anim.SetTrigger(AnimationTags.DEAD_TRIGGER);
    }

} 































