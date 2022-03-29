using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float radius = 5.0f;
    public float force = 700.0f;
    public float delay = 3.0f;
    
    public GameObject explosionEffect;

    public float countdown;

    bool isExploded = false;

    public AudioClip boomSound;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0 && !isExploded)
        {
            isExploded = true;
            //Explosion Effect
            GameObject boomParticle = Instantiate(explosionEffect, transform.position, transform.rotation);
            AudioSource aSour = boomParticle.AddComponent<AudioSource>();
            aSour.PlayOneShot(boomSound);
            Destroy(boomParticle, 3f);


            Collider[] collids = Physics.OverlapSphere(transform.position, radius);

            foreach(Collider colid in collids)
            {
                if(colid.tag == "Enemy") { 
                    //Hurt Enemies
                    Destroyable des = colid.transform.parent.GetComponent<Destroyable>();
                    if(des != null)
                    {
                        UnityEngine.Debug.Log("I have des!");
                        des.gotHitGrenade(200);
                    }

                    //Add forces to them
                    Rigidbody rb = colid.transform.parent.GetComponent<Rigidbody>();
                    if(rb != null)
                    {
                        UnityEngine.Debug.Log("I have rigidbody!");
                        rb.AddExplosionForce(force, transform.position, radius);
                    }

                    UnityEngine.Debug.Log(colid.transform.parent.name);
                }
                else if(colid.tag == "Player")
                {
                    //Hurt
                    Weapon weapo = colid.GetComponent<EQRef>().reference.GetComponent<Weapon>();
                    if (weapo != null)
                    {
                        UnityEngine.Debug.Log("I have weapo!");
                        weapo.gotHit(200);
                    }

                    //Add forces
                    Rigidbody rb = colid.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        UnityEngine.Debug.Log("I have rigidbody!");
                        rb.AddExplosionForce(force, transform.position, radius);
                    }

                    UnityEngine.Debug.Log(colid.name);
                }
            }
            Destroy(gameObject);
                //Destroy Grenade
            
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
