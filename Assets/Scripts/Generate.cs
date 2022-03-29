using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public float timerForNewDrop;
    public float newDropRate;

    public GameObject zombiePrefab;
    public GameObject ammoPrefab;
    public GameObject coinPrefab;

    public int zombieCount = 50;
    public int currentZombieCount = 0;

    public Weapon weap;
    // Start is called before the first frame update
    void Start()
    {
        weap = GetComponent<EQRef>().reference.GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            float randX = 0;
            float randZ = 0;
            while ((randX == 0 && randZ == 0) || Physics.CheckSphere(new Vector3(randX, 0.5f, randZ), 0.5f) || Vector3.Distance(transform.position, new Vector3(randX, 0.5f, randZ)) < 10.0f)
            {
                randX = Random.Range(-30.0f, 30.0f);
                randZ = Random.Range(-30.0f, 30.0f);
            }
            Instantiate(zombiePrefab, new Vector3(randX, 0.0f, randZ), Quaternion.identity);
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            weap.changeScore(1000);
        }
        
        if (timerForNewDrop < newDropRate)
            timerForNewDrop += Time.deltaTime;
        else
        {
            float randX = 0;
            float randZ = 0;
            while( (randX == 0 && randZ == 0) || Physics.CheckSphere(new Vector3(randX, 0.5f, randZ), 0.4f))
            { 
                randX = Random.Range(-30.0f, 30.0f);
                randZ = Random.Range(-30.0f, 30.0f);
            }
            float randChance = Random.Range(0.0f, 1.0f);

            if (randChance < 0.7f && currentZombieCount < zombieCount)
            {
                currentZombieCount++;
                Instantiate(zombiePrefab, new Vector3(randX, 0.0f, randZ), Quaternion.identity);
            }
            else if(randChance < 0.9f)
                Instantiate(ammoPrefab, new Vector3(randX, 0.0f, randZ), Quaternion.identity);
            else
                Instantiate(coinPrefab, new Vector3(randX, 0.0f, randZ), Quaternion.identity);
            timerForNewDrop = 0.0f;
        }
    }
}
