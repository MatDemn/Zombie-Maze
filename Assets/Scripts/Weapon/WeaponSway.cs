using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float amount;
    public float maxAmount;
    public float smoothAmount;

    private Vector3 initialPos;
    // Start is called before the first frame update
    void Start()
    {
        initialPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = -Input.GetAxis("Mouse X") * amount;
        float moveY = -Input.GetAxis("Mouse Y") * amount;

        moveX = Mathf.Clamp(moveX, -maxAmount, maxAmount);
        moveY = Mathf.Clamp(moveY, -maxAmount, maxAmount);

        Vector3 finalPos = new Vector3(moveX, moveY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, 
            finalPos + initialPos, Time.deltaTime * smoothAmount);
    }
}
