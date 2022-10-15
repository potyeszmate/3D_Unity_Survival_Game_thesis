using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour {

    private EnemyAnimation enemy_Anim;
    private NavMeshAgent navAgent;
    private EnemyController enemy_Controller;
    private PlayerStat playerStat;

    private FriendlyEnemyController friendlyEnemy_Controller;

    GameObject thisAnimal;
    public Transform meat;

    public AudioSource audioSource;
    public AudioClip[] getDmgSfx;
    public AudioClip[] playerGetDmgSfx;

    public AudioClip playerdeathSfx;

    public AudioClip playerHeartBeatSfx;
    public AudioClip playerHungerSfx;

    public PlayerStat playerStats;


    public float health;

    public bool is_Player, is_Cannibal,is_Bear,is_Wolf,is_Boar,is_Deer,is_ironAge,is_Chicken,is_Rabbit;

    private bool is_Dead;

    private EnemyAudio enemyAudio;

    private void Start() 
    {
        thisAnimal = transform.gameObject;
    }
	void Awake () {

	    
        if(is_Cannibal || is_Bear || is_Wolf || is_Boar) {
            enemy_Anim = GetComponent<EnemyAnimation>();
            enemy_Controller = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            // get enemy audio
            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }

        if(is_Deer || is_ironAge || is_Chicken || is_Rabbit) {
            enemy_Anim = GetComponent<EnemyAnimation>();
            friendlyEnemy_Controller = GetComponent<FriendlyEnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            // get enemy audio
            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }

        if(is_Player) {
            playerStat = GetComponent<PlayerStat>();
        }

	}
    private void Update() 
    {
        if(is_Player) 
        {
            if (health < 0)
                health = 0;

            if (playerStat.hunger <= 0 || playerStat.thirst <= 0 || playerStat.energy <= 0)
            {
                if(health < 0)
                    playerStat.Display_HealthStats(0);
                else
                {
                    health -= Time.deltaTime * 0.5f;
                    playerStat.Display_HealthStats(health); 
                }
                            
            }

            

            if(health <= 0) 
            {

                GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);

                for (int i = 0; i < enemies.Length; i++) {

                

                }

                is_Dead = true;

                if(tag == Tags.PLAYER_TAG) 
                {
                    
                    Invoke("RestartGame", 2f);

                } 
                else 
                {

                    Invoke("TurnOffGameObject", 3f);

                }
            }

        }    
    }
    
	
    public void ApplyDamage(float damage) 
    {

        // if we died don't execute the rest of the code
        if (is_Dead)
            return;

        health -= damage;

        if(is_Player) 
        {
            // show the stats(display the health UI value)
            if(health >= 0)
            {
                playerStat.Display_HealthStats(health);
            //audio get dmg
                PlayPlayerHitSound();
            }
            
        }

        if(is_Cannibal || is_Bear || is_Wolf || is_Boar) 
        {

            if(enemy_Controller.Enemy_State == EnemyState.PATROL) 
            {
                enemy_Controller.chase_Distance = 60f;
            }

            if(!is_Wolf)
                PlayHitSound();

        }

        if(is_Deer || is_Chicken || is_ironAge || is_Rabbit) 
        {

            if(friendlyEnemy_Controller.friendlyEnemy_State == FriendlyEnemyState.PATROL) 
            {
                friendlyEnemy_Controller.chase_Distance = 60f;
            }

            PlayHitSound();
        }


        if(health <= 0f) 
        {
            PlayerDied();

            is_Dead = true;
        }


    } // apply damage

    void PlayerDied() {

        if(is_Cannibal) 
        {
            // Ha meghal a kannibál
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Animator>().enabled = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 0.4f);

            enemy_Controller.enabled = false;
            navAgent.enabled = false;
            enemy_Anim.enabled = false;

            StartCoroutine(DeadSound());

            // EnemyManager spawn more enemies
            EnemyManager.instance.EnemyDied("Cannibal");
        }

        if(is_Bear) {

            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemy_Controller.enabled = false;  

            enemy_Anim.Dead();

            StartCoroutine(DeadSound());
            StartCoroutine(DestroyAnimal());

            // EnemyManager spawn more enemies
            EnemyManager.instance.EnemyDied("Bear");
            

        }

        if(is_Wolf) {

            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemy_Controller.enabled = false;

            enemy_Anim.Dead();

            StartCoroutine(DeadSound());
            StartCoroutine(DestroyAnimal());

            // EnemyManager spawn more enemies
            EnemyManager.instance.EnemyDied("Wolf");
        }

        if(is_Boar) {

            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemy_Controller.enabled = false;

            enemy_Anim.Dead();

            StartCoroutine(DeadSound());
            StartCoroutine(DestroyAnimal());

            // EnemyManager spawn more enemies
            EnemyManager.instance.EnemyDied("Boar");
        }

        if(is_Deer) {

            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            friendlyEnemy_Controller.enabled = false;
            
            enemy_Anim.Dead();

            StartCoroutine(DeadSound());
            StartCoroutine(DestroyAnimal());

            // EnemyManager spawn more enemies
            EnemyManager.instance.EnemyDied("Deer");
        }

        if(is_ironAge) {

            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            friendlyEnemy_Controller.enabled = false;

            enemy_Anim.Dead();

            StartCoroutine(DeadSound());
            StartCoroutine(DestroyAnimal());

            // EnemyManager spawn more enemies
            EnemyManager.instance.EnemyDied("ironAge");
        }

        if(is_Chicken) {

            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            friendlyEnemy_Controller.enabled = false;

            enemy_Anim.Dead();

            StartCoroutine(DeadSound());
            StartCoroutine(DestroyAnimal());

            // EnemyManager spawn more enemies
            EnemyManager.instance.EnemyDied("Chicken");
        }

        if(is_Rabbit) {

            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            friendlyEnemy_Controller.enabled = false;

            enemy_Anim.Dead();

            StartCoroutine(DeadSound());
            StartCoroutine(DestroyAnimal());

            // EnemyManager spawn more enemies
            EnemyManager.instance.EnemyDied("Rabbit");
        }

        if(tag == Tags.PLAYER_TAG) {

            Invoke("RestartGame", 2f);

        } else {

            Invoke("TurnOffGameObject", 3f);

        }
           
    } // player died

    void RestartGame() 
    {
        
        SceneManager.LoadScene(3);
        Time.timeScale = 1;

    }

    void TurnOffGameObject() 
    {
        gameObject.SetActive(false);
    }

    public void NoFoodOrDrinkTakeDamage()
    {
        health -= Time.deltaTime * 1f;
    }

    IEnumerator DeadSound() 
    {
        yield return new WaitForSeconds(0.2f);
        enemyAudio.Play_DeadSound();
    }

    IEnumerator DestroyAnimal()
    {
        yield return new WaitForSeconds(2.6f);

        Instantiate(meat, thisAnimal.transform.position, thisAnimal.transform.rotation);

    }

    private void PlayHitSound()
    {
        audioSource.clip = getDmgSfx[Random.Range(0, getDmgSfx.Length)];
        audioSource.Play();
    }

    private void PlayPlayerHitSound()
    {
        audioSource.clip = playerGetDmgSfx[Random.Range(0, getDmgSfx.Length)];
        audioSource.Play();
    }

    IEnumerator PlayPlayerHeartBeattSound()
    {
        yield return new WaitForSeconds(3);
        audioSource.clip = playerHeartBeatSfx;
        audioSource.Play();
    }
    IEnumerator PlayPlayerHungerSound()
    {
        yield return new WaitForSeconds(3);
        audioSource.clip = playerHungerSfx;
        audioSource.Play();
    }

    private void PlayPlayerDeathSound()
    {
        audioSource.clip = playerdeathSfx;
        audioSource.Play();

    }



}









































