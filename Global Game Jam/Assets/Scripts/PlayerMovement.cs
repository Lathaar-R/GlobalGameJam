using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variaveis
    private Rigidbody2D rb;

    //Serializadas 
    [SerializeField] private float acceleration = 3;
    [SerializeField] private float maxVelocity = 10;
    [SerializeField] private float distanceToStop = 1;
    [SerializeField] private float velPow = 0.9f;

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
            rb.velocity *= Mathf.Clamp01(distance / distanceToStop);
        else
        {
            var vel = Mathf.Pow((direction * acceleration).magnitude, velPow) * direction.normalized;
            vel = vel.magnitude < maxVelocity ? vel : direction.normalized * maxVelocity;

            rb.velocity = vel;

        }

        if(Keyboard.current.spaceKey.isPressed)
            rb.AddForce(Vector2.one * 10f, ForceMode2D.Impulse);
    }
}
