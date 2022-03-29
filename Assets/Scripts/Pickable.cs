using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    public AudioClip sound;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject refer = other.gameObject.GetComponent<EQRef>().reference;
            refer.GetComponent<Weapon>().addAmmo(10);
            refer.GetComponent<AudioSource>().PlayOneShot(sound);
            Destroy(gameObject);
        }
    }
}
