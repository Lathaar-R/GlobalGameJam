using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Vector2 target;
    private Vector2 startPos;
    private Vector2 startScale;
    private Vector2 startTargetScale;
    private Collider2D collider;
    private SpriteRenderer targetSpriteRenderer;
    private float targetScaleProportion;
    private GameObject targetObject;
    
    [SerializeField] GameObject targetObjectPrefab;
    [SerializeField] private float speed = 1;
    [SerializeField] private float height = 5; 
    [SerializeField] private float growScale = 5; 
    [SerializeField] private float targetScalling = 1; 
    [SerializeField] private float distanceToDamage = 0; 
    [SerializeField] private AnimationCurve curve;
    private float time;

    public Vector2 Target { get => target; set => target = value; }
    public Collider2D Collider => collider;
    public float Speed { get => speed; set => speed = value; }

    private void Awake() {
        collider = GetComponent<Collider2D>();
    
        startPos = transform.position;
        startScale = transform.localScale;
    }

    private void Start() {
        targetObject = Instantiate(targetObjectPrefab);

        startTargetScale = targetObject.transform.localScale;
        targetObject.transform.position = target;
        targetScaleProportion = targetObject.transform.localScale.x / targetObject.transform.localScale.y;

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime * speed;

        var c = curve.Evaluate(time);
        Vector2 nextPos;

        nextPos.y = Mathf.Lerp(startPos.y, target.y, c);
        nextPos.x = target.x + (Mathf.Sin((float)Math.PI * c) * height * Mathf.Sign(target.x));

        transform.position = nextPos;

        transform.localScale = (Vector3)startScale + (Mathf.Sin((float)Math.PI * c) * growScale) * Vector3.one;

        Vector2 nextScale = Vector2.zero;
        if(c < 0.5f)
        {
            nextScale.y = Mathf.Sin((float)Math.PI * c) * targetScalling;
        } 
        else
        {
            nextScale.y = startTargetScale.y + (Mathf.Sin((float)Math.PI * c) * (targetScalling - startTargetScale.y));
        }

        nextScale.x = nextScale.y * targetScaleProportion;

        targetObject.transform.localScale = nextScale;


        // if(transform.position <= distanceToDamage)
        // {
        //     if(phy)
        // }
    }
}
