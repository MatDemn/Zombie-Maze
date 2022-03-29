using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour
{
    public float randomRate;
    public float randomTimer;

    public NavMeshAgent agent;
    public Weapon player;

    public Destroyable destrRef;
    // Start is called before the first frame update


    public float attackRadius = 0.5f;
    public float walkRadius = 5.0f;


    public bool dyingSwitch = false;
    public bool stopSwitch = false;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();        
        destrRef = GetComponent<Destroyable>();
        player = destrRef.player;
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.isGameOver && !dyingSwitch)
        {

            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (dist <= attackRadius)
            {
                if (!dyingSwitch) { 
                    FaceTarget();
                    destrRef.attack();
                }
            }
            else if (dist <= walkRadius && destrRef.lastHitTime+destrRef.stunTime < Time.time)
            {
                if (!dyingSwitch) {
                    agent.SetDestination(player.transform.position);
                    FaceTarget();
            
                    destrRef.walk();
                }
            }
            else
            {
                if (!dyingSwitch) { 
                    agent.SetDestination(transform.position);
                    destrRef.idle();
                }
            }

            //randomTimer = 0.0f;
            //}
            if (Input.GetKeyDown(KeyCode.T))
                UnityEngine.Debug.Log("Distance: " + dist);
        }
        else
        {
            agent.enabled = false;
            destrRef.idle();
        }

    }
    void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5.0f);
    }

}
