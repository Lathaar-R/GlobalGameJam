using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variaveis
    private Rigidbody2D rb;
    private float curMinVelocity;

    //Serializadas 
    [SerializeField] private float acceleration = 3;
    [SerializeField] private float maxVelocity = 5;
    [SerializeField] private float minVelocity = 1f;
    [SerializeField] private float distanceToStop = 0.8f;
    [SerializeField] private float velPow = 0.85f;

    //Propriedades



    #endregion


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

    }


    void FixedUpdate()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 direction = (mousePosition - rb.position);
        float distance = direction.magnitude;

        if (distance < distanceToStop)
        {
            rb.velocity *= Mathf.Clamp01(distance / distanceToStop);
            curMinVelocity = 0;
        }
        else
        {
            Debug.DrawRay(rb.position, direction, Color.red);
            direction -= direction.normalized * distanceToStop;
            Debug.DrawRay(rb.position, direction, Color.blue);

            var vel = Mathf.Pow((direction * acceleration).magnitude, velPow) * direction.normalized;
            vel = vel.magnitude < maxVelocity ? (vel.magnitude < curMinVelocity ? direction.normalized * curMinVelocity : vel) : direction.normalized * maxVelocity;

            rb.velocity = vel;

            if(rb.velocity.magnitude > minVelocity) curMinVelocity = minVelocity;
        }


    }
}
