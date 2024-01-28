using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScript : MonoBehaviour
{
    [SerializeField] private float fadeOutTime;
    private SpriteRenderer spriteRenderer;
    private float time;
    private bool isFading;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        time += Time.deltaTime;

        if (isFading)
        {

            spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(1, 0, time / fadeOutTime));
            if (time > fadeOutTime)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (time > 1)
            {
                time = 0;
                isFading = true;
            }
        }
    }
}
