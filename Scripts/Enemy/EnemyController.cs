using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour {

    private EnemyAnimation enemy_Anim;
    private NavMeshAgent navAgent;

    private EnemyState enemy_State;
    


    public float walk_Speed = 0.5f;
    public float run_Speed = 4f;

    public float chase_Distance = 7f;
    private float current_Chase_Distance;
    public float attack_Distance = 1.8f;
    public float chase_After_Attack_Distance = 2f;

    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_For_This_Time = 15f;
    private float patrol_Timer;

    public float wait_Before_Attack = 2f;
    private float attack_Timer;

    private Transform target;

    public GameObject attack_Point;

    private EnemyAudio enemy_Audio;

    void Awake() {
        enemy_Anim = GetComponent<EnemyAnimation>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

        enemy_Audio = GetComponentInChildren<EnemyAudio>();

    }

    // Use this for initialization
    void Start () {

        enemy_State = EnemyState.PATROL;

        patrol_Timer = patrol_For_This_Time;

        // when the enemy first gets to the player, it waits this time to attack 
        attack_Timer = wait_Before_Attack;

        // The value of chase distance
        current_Chase_Distance = chase_Distance;

	}
	
	// Update is called once per frame - The enemies states (he can run, patron, or attack or just stop moving)
	void Update () {
		
        if(enemy_State == EnemyState.PATROL) {
            Patrol();
        }

        if(enemy_State == EnemyState.CHASE) {
            Chase();
        }

        if (enemy_State == EnemyState.ATTACK) {
            Attack();
        }

    }

    void Patrol() {

        // Sets nav agent to move
        navAgent.isStopped = false;
        navAgent.speed = walk_Speed;

        // Need to add the patrol timer to check how mutch we can patrol max and min
        patrol_Timer += Time.deltaTime;

        if(patrol_Timer > patrol_For_This_Time) {

            SetNewRandomDestination();

            patrol_Timer = 0f;

        }

        if(navAgent.velocity.sqrMagnitude > 0) {
        
            enemy_Anim.Walk(true);
        
        } else {

            enemy_Anim.Walk(false);

        }

        // Checks the distance between the player and the enemy
        if(Vector3.Distance(transform.position, target.position) <= chase_Distance) 
        {

            enemy_Anim.Walk(false);

            enemy_State = EnemyState.CHASE;

            // Plays the chasing audio
            enemy_Audio.Play_ScreamSound();

        }


    }

    void Chase() {

        // Tells the agent to move
        navAgent.isStopped = false;
        navAgent.speed = run_Speed;

        // Sets the player's position as theenemies next destination when chasing
        navAgent.SetDestination(target.position);

        if (navAgent.velocity.sqrMagnitude > 0) 
        {

            enemy_Anim.Run(true);

        } 
        else 
        {

            enemy_Anim.Run(false);

        }

        // We want the distance when we stop to be the attack distance 
        if(Vector3.Distance(transform.position, target.position) <= attack_Distance) 
        {

            // Stop the animations
            enemy_Anim.Run(false);
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.ATTACK;

            // Reset the chase distance
            if(chase_Distance != current_Chase_Distance) 
            {
                chase_Distance = current_Chase_Distance;
            }

        } 
        else if(Vector3.Distance(transform.position, target.position) > chase_Distance) 
        {
            

            // Stop running
            enemy_Anim.Run(false);

            enemy_State = EnemyState.PATROL;

            // Reset the patrol timer so that the function
            patrol_Timer = patrol_For_This_Time;

            // Reset the chase distance to previous
            if (chase_Distance != current_Chase_Distance) {
                chase_Distance = current_Chase_Distance;
            }


        } 

    } 

    void Attack() 
    {

        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attack_Timer += Time.deltaTime;

        // Enemy has to attack after the waiting time ends
        if(attack_Timer > wait_Before_Attack) {

            enemy_Anim.Attack();

            attack_Timer = 0f;

        }

        // Chases us while the enemy is in front of the player's position
        if(Vector3.Distance(transform.position, target.position) >
           attack_Distance + chase_After_Attack_Distance) 
        {
            enemy_State = EnemyState.CHASE;
        }


    }

    // Sets a new random destination (as an AI, it gives a random destination every time he changes the movement destination)
    void SetNewRandomDestination() {

        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);

        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1);

        navAgent.SetDestination(navHit.position);

    }

    // Enemy turns on the attack point when attackin (attack animation tab)
    void Turn_On_AttackPoint() {
        attack_Point.SetActive(true);
    }

    // Turns of the attack point after attack animation (animation tab)
    void Turn_Off_AttackPoint() {
        if (attack_Point.activeInHierarchy) {
            attack_Point.SetActive(false);
        }
    }

    public EnemyState Enemy_State {
        get; set;
    }

}


































