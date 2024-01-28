using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balance : MonoBehaviour
{
    [SerializeField] private float targetRotation;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float force;

    private void Awake()
    {
        GameController.Instance.endGamePlay += OnEndGamePlay;

    }

    private void OnDisable()
    {
        GameController.Instance.endGamePlay -= OnEndGamePlay;
    }

    void FixedUpdate()
    {
        rb.MoveRotation(Mathf.LerpAngle(rb.rotation, targetRotation, force * Time.fixedDeltaTime));        
    }

    private void OnEndGamePlay()
    {
        this.enabled = false;
    }
}
