using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// The same function as the EnemyController.cs. Difference: In chasing method

public enum FriendlyEnemyState {
    PATROL,
    CHASE,
    ATTACK
}

public class FriendlyEnemyController : MonoBehaviour {

    private EnemyAnimation enemy_Anim;
    private NavMeshAgent navAgent;

    private FriendlyEnemyState firendlyEnemy_State;
    //private PlayerStat playerStat;

    //private EnemyAudio enemyAudio;


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

    void Awake() 
    {
        enemy_Anim = GetComponent<EnemyAnimation>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

        enemy_Audio = GetComponentInChildren<EnemyAudio>();

    }

    // Use this for initialization
    void Start () 
    {

        firendlyEnemy_State = FriendlyEnemyState.PATROL;

        patrol_Timer = patrol_For_This_Time;

        // when the enemy first gets to the player
        // attack right away
        attack_Timer = wait_Before_Attack;

        // memorize the value of chase distance
        // so that we can put it back
        current_Chase_Distance = chase_Distance;

	}
	
	// Update is called once per frame
	void Update () 
    {
		
        if(firendlyEnemy_State == FriendlyEnemyState.PATROL) {
            Patrol();
        }

        if(firendlyEnemy_State == FriendlyEnemyState.CHASE) {
            Chase();
        }

    }

    void Patrol() 
    {

        
        navAgent.isStopped = false;
        navAgent.speed = walk_Speed;

        
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

        
        if(Vector3.Distance(transform.position, target.position) <= chase_Distance) {

            enemy_Anim.Walk(false);

            firendlyEnemy_State = FriendlyEnemyState.CHASE;

            enemy_Audio.Play_ScreamSound();

        }


    } 

    void Chase() 
    {

        
        navAgent.isStopped = false;
        navAgent.speed = run_Speed;

        // When the player is near, the enemy runs away from the enemy to the opposite direction of the enemy's position
        Vector3 directToPlayer = transform.position - GameObject.FindWithTag(Tags.PLAYER_TAG).transform.position;
        Vector3 new_position = transform.position + (directToPlayer);
        
        navAgent.SetDestination(new_position);
        

        if (navAgent.velocity.sqrMagnitude > 0)
         {

            enemy_Anim.Run(true);

        } else 
        {

            enemy_Anim.Run(false);

        }

        float runawayDistance = chase_Distance * 4;

        if(Vector3.Distance(transform.position, target.position) > runawayDistance ) {
            
            enemy_Anim.Run(false);

            firendlyEnemy_State = FriendlyEnemyState.PATROL;

            patrol_Timer = patrol_For_This_Time;

            
            if (chase_Distance != current_Chase_Distance) {
                chase_Distance = current_Chase_Distance;
            }


        }
    }

    void SetNewRandomDestination() 
    {

        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);

        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1);

        navAgent.SetDestination(navHit.position);
    }

    public FriendlyEnemyState friendlyEnemy_State 
    {
        get; set;
    } 

}






































