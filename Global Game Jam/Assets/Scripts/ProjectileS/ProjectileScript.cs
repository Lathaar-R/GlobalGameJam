using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileScript : MonoBehaviour
{
    protected Vector2 target;
    protected Vector2 startPos;
    protected Vector2 startScale;
    protected Vector2 startTargetScale;
    protected Collider2D collider;
    protected SpriteRenderer targetSpriteRenderer;
    protected float targetScaleProportion;
    protected GameObject targetObject;
    protected float arcDir;

    [SerializeField] GameObject targetObjectPrefab;
    [SerializeField] protected float speed = 0.7f;
    [SerializeField] protected float height = 2;
    [SerializeField] protected float growScale = 1.2f;
    [SerializeField] protected float rotationSpeed = 0.4f;
    [SerializeField] protected float targetScalling = 1;
    [SerializeField] protected float distanceToDamage = 0.5f;
    [SerializeField] protected ContactFilter2D contactFilter;
    [SerializeField] protected GameObject hitPrefab;
    [SerializeField] protected AnimationCurve curve;

    protected float time;

    public Vector2 Target { get => target; set => target = value; }
    public Collider2D Collider => collider;
    public float Speed { get => speed; set => speed = value; }
    public GameObject TargetObject => targetObject;

    protected virtual void Awake()
    {
        collider = GetComponent<Collider2D>();

        startPos = transform.position;
        startScale = transform.localScale;

        arcDir = Mathf.Sign(target.x);


    }

    protected virtual void OnEnable()
    {
        time = 0;

        if (targetObject == null)
        {
            targetObject = Instantiate(targetObjectPrefab);
            targetObject.transform.position = Vector2.down * 100;
        }
        else
        {
            startTargetScale = targetObject.transform.localScale;
            targetObject.transform.position = target;
            //Debug.Log(Target);
            targetScaleProportion = targetObject.transform.localScale.x / targetObject.transform.localScale.y;
            targetObject.transform.localScale = Vector2.zero;   
        }

        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Shoot();
    }

    protected virtual void Shoot()
    {
        time += Time.deltaTime * speed;

        var c = curve.Evaluate(time);
        Vector2 nextPos;

        nextPos.y = Mathf.Lerp(startPos.y, target.y, c);
        nextPos.x = target.x + (Mathf.Sin((float)Math.PI * c) * height * arcDir);

        transform.position = nextPos;

        transform.localScale = (Vector3)startScale + (Mathf.Sin((float)Math.PI * c) * growScale) * Vector3.one;

        Vector2 nextScale = Vector2.zero;
        if (c < 0.5f)
        {
            nextScale.y = Mathf.Sin((float)Math.PI * c) * targetScalling;
        }
        else
        {
            nextScale.y = startTargetScale.y + (Mathf.Sin((float)Math.PI * c) * (targetScalling - startTargetScale.y));
        }

        nextScale.x = nextScale.y * targetScaleProportion;

        targetObject.transform.localScale = nextScale;

        transform.Rotate(Vector3.forward, 360 * Time.deltaTime * rotationSpeed);

        if (Mathf.Abs(transform.position.y - target.y) < distanceToDamage)
        {
            var hit = Physics2D.OverlapCircle(transform.position, Mathf.Max(collider.bounds.extents.x, collider.bounds.extents.y), contactFilter.layerMask);
            if(hit != null)
            {
                GameController.Instance.DamagePlayer(1);

                gameObject.transform.position = Vector2.down * 100;
                TargetObject.transform.position = Vector2.down * 100;
                gameObject.SetActive(false);
            }
            if (Mathf.Abs(transform.position.y - target.y) < 0.1f)
            {
                Instantiate(hitPrefab, transform.position, Quaternion.identity);

                gameObject.transform.position = Vector2.down * 100;
                TargetObject.transform.position = Vector2.down * 100;
                gameObject.SetActive(false);
            }
        }
    }
}
