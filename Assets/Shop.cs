using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Weapon player;
    public int cost;

    public AudioSource audioSC;
    public AudioClip buySound;
    public AudioClip denySound;
    public string itemName;

    bool playedOnce = false;

    void Start()
    {
        UnityEngine.Debug.Log(itemName);
        audioSC = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (playedOnce) return;

        UnityEngine.Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            if (player.currentScore >= cost)
            {
                if(itemName == "Ammo")
                {
                    audioSC.PlayOneShot(buySound);
                    player.addAmmo(10);
                    player.changeScore(-cost);
                }
                else if(itemName == "Grenade")
                {
                    if (player.currentGrenades != player.maxGrenades)
                    {
                        audioSC.PlayOneShot(buySound);
                        player.addGrenades(1);
                        player.changeScore(-cost);
                    }
                }
                else if(itemName == "Key")
                {
                    audioSC.PlayOneShot(buySound);
                    player.RunGameWin();
                }
                else
                {
                    UnityEngine.Debug.Log("Error, wrong name of the item.");
                }  
            }
            else
            {
                audioSC.PlayOneShot(denySound);
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        playedOnce = false;
    }
}
