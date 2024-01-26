using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{
    [SerializeField] private float targetRotation;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float force;



    void FixedUpdate()
    {
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation, force * Time.fixedDeltaTime));        
    }
}
