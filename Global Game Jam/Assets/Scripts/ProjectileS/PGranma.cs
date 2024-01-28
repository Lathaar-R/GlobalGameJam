using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGranma : ProjectileScript
{
    private GameObject player;

    [SerializeField] private float chaseSpeed = 1;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.Find("HitBox");
    }

    protected override void Shoot()
    {
        time += Time.deltaTime * speed;

        var c = curve.Evaluate(time);
        Vector2 nextPos;

        //Moving the target closer to the player
        target = Vector2.MoveTowards(target, player.transform.position, chaseSpeed * Time.deltaTime);
        targetObject.transform.position = target;

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
            if (hit != null)
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
