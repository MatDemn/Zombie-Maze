using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Destroyable : MonoBehaviour
{
    public static int zombieKillCount = 0;

    public Weapon player;

    public int health = 100;
    public int points = 20;
    public GameObject drop;

    public AudioSource audioComp;

    public AudioClip liveSound;
    public AudioClip walkSound;
    public AudioClip attackSound;
    public AudioClip dieSound;
    public AudioClip hitSound;

    public Animator anim;

    public bool isDying = false;
    public bool isHurting = false;
    public bool isAttacking = false;
    public bool isWalking = false;
    public bool isIdle = true;

    public bool changeSc = false;

    public float hurtTimer = 0.0f; // trzyma czas aktualnej animacji
    public float hurtRate = 1.0f; // ile trwa pojedyncza animacja

    //public float attackTimer = 0.0f; // trzyma czas aktualnej animacji ataku
    //public float attackRate = 1.0f; // ile trwa animacja ataku

    public float dropTimer = 0.0f;
    public float dropRate = 1.0f;

    // ile punktow jest za zabicie
    public int score = 200;

    public float lastHitTime = 0.0f;
    public float stunTime = 2.0f;

    public GameObject blood;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<EQRef>().reference.GetComponent<Weapon>();
        audioComp = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player.isGameOver)
        {
            if (health <= 0)
            {

                //Play Die Sound
                //Play Die Animation
                if (!isDying)
                {
                    MakeItDie();
                }
                else
                {
                    if (dropTimer < dropRate)
                        dropTimer += Time.deltaTime;
                }
                //DropReward();
            }
            if (hurtTimer < hurtRate)
                hurtTimer += Time.deltaTime;

        }
        else
        {
            idle();
            GetComponent<Navigator>().agent.enabled = false;
        }
    }

    public void gotHit(int amount, RaycastHit hit)
    {
        health -= amount;
        GameObject hitParticleEffect;
        if (health <= 0)
        { 
            hitParticleEffect = Instantiate(blood, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
            Destroy(hitParticleEffect, 1f);
            MakeItDie();
        }

        if (isHurting || hurtTimer < hurtRate || isDying)
        {
            return;
        }
        else
        {
            lastHitTime = Time.time;
            isHurting = true;
            hurtTimer = 0.0f;
            audioComp.clip = liveSound;
            audioComp.Play(0);
            audioComp.PlayOneShot(hitSound);
            anim.CrossFadeInFixedTime("Zombie_gotHit", 0.1f);
            
        }
        hitParticleEffect = Instantiate(blood, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
        Destroy(hitParticleEffect, 1f);
    }

    public void gotHitGrenade(int amount)
    {
        health -= amount;
        if (isHurting || hurtTimer < hurtRate || isDying)
        {
            return;
        }
        if (health <= 0)
        {  
            MakeItDie();
        }
        else
        {
            lastHitTime = Time.time;
            isHurting = true;
            hurtTimer = 0.0f;
            //audioComp.PlayOneShot(hitSound);
            anim.CrossFadeInFixedTime("Zombie_gotHit", 0.1f);

        }
    }

    public void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        isHurting = info.IsName("Zombie_gotHit");
        isIdle = info.IsName("idle");
    }

    void MakeItDie()
    {
        if (isDying)
            return;
        else
        {
            Collider[] colChildren = gameObject.GetComponentsInChildren<Collider>();
            foreach(Collider comp in colChildren)
            {
                comp.enabled = false;
            }
            GetComponent<Navigator>().dyingSwitch = true;
            isDying = true;
            zombieKillCount++;
            anim.Play("fallingback");
            audioComp.Stop();
            audioComp.PlayOneShot(dieSound);
            
            if(!changeSc)
            {
                changeSc = true;
                player.changeScore(score);
            }
            
            Destroy(gameObject, 4f);
        }
    }

    public void DropReward()
    {
        if (dropTimer >= dropRate)
        {
            dropTimer = 0;
            Instantiate(drop, gameObject.transform.position + new Vector3(0.0f, 1.0f, 0.0f), gameObject.transform.rotation);
        }
    }

    public void attack()
    {
        if (!isAttacking) {
            ResetState();
            isAttacking = true;
            audioComp.Stop();
            audioComp.PlayOneShot(attackSound);
            anim.SetBool("Attack", true);
        }
    }

    public void walk()
    {
        if (isWalking)
            return;
       
       ResetState();
       isWalking = true;
       audioComp.clip = walkSound;
       audioComp.Play(0);
            
       anim.SetBool("Walking", true);      
    }

    public void idle()
    {
        if (isIdle) return;

        ResetState();
        isIdle = true;
        //audioComp.clip = liveSound;
        //audioComp.Play(0);
        
        
    }

    public void ResetState()
    {
        isIdle = false;
        isHurting = false;
        isAttacking = false;
        isWalking = false;
        anim.SetBool("Attack", false);
        anim.SetBool("Walking", false);
    }
    
    public void makePain(int amount)
    {
        player.gotHit(amount);
    }

    
}
