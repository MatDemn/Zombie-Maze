using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPickup : MonoBehaviour
{
    public AudioClip sound;
    public int scoreAmount;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameObject refer = other.gameObject.GetComponent<EQRef>().reference;
            refer.GetComponent<Weapon>().changeScore(scoreAmount);
            refer.GetComponent<AudioSource>().PlayOneShot(sound);
            Destroy(gameObject);
        }
    }
}
