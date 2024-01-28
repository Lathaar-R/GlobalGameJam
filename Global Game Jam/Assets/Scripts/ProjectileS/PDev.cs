using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PDev : ProjectileScript
{
    [SerializeField] private GameObject[] hitPrefabs;
    protected override void Shoot()
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
            if (hit != null)
            {
                GameController.Instance.DamagePlayer(1);

                gameObject.transform.position = Vector2.down * 100;
                TargetObject.transform.position = Vector2.down * 100;
                gameObject.SetActive(false);

                if (UnityEngine.Random.value < 0.5f)
                    GameController.Instance.PlayAudio("ouch");
                else
                    GameController.Instance.PlayAudio("ouch2");
            }
            if (Mathf.Abs(transform.position.y - target.y) < 0.1f)
            {
                Instantiate(UnityEngine.Random.value < 0.98f ? hitPrefabs[0] : hitPrefabs[1], transform.position, Quaternion.identity);

                gameObject.transform.position = Vector2.down * 100;
                TargetObject.transform.position = Vector2.down * 100;
                gameObject.SetActive(false);

                GameController.Instance.PlayAudio("bigorna");
            }
        }
    }
}
