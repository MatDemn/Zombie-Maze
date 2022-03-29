using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour
{
    //public Destroyable dest;
    public float throwForce;
    public GameObject grenadePrefab;
    public Transform pos;
    public Weapon weap;

    private void Start()
    {
        weap = GetComponent<Weapon>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Grenade") && weap.currentGrenades > 0)
        {
            GameObject grenade = Instantiate(grenadePrefab, pos.position, pos.rotation);
            Rigidbody rb = grenade.GetComponent<Rigidbody>();
            rb.AddForce(pos.forward * throwForce, ForceMode.VelocityChange);
            UnityEngine.Debug.Log(pos.forward * throwForce);
        }
    }
}
