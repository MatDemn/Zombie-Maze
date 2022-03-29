using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverRun : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<EQRef>().reference.GetComponent<Weapon>().RunGameOver();
    }
}
