using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    [SerializeField] [Range (-1,1)] float movementFactor;

    private void Start()
    {
        startingPosition = transform.position;
        movementVector = new Vector3(0,0.01f,0);
    }

    private void Update()
    {
        Vector3 offset = movementVector * movementFactor;
        transform.position += offset;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Ground" || collision.gameObject.name == "Up")
        {
            Debug.Log("obstacle hitted ground");
            movementFactor = movementFactor * (-1);
        }
    }
}
