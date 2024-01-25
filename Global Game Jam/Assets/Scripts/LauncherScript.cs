using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LauncherScript : MonoBehaviour
{
    private float timer;
    private Func<Vector2, float, bool> shootType;
    [SerializeField] private float coolDown = 1;
    [SerializeField] private GameObject[] projectilesPrefabs;
    [SerializeField] private float[] projectileProbabilities;
    [SerializeField] private List<GameObject> projectilePool;
    [SerializeField] private LayerMask wallsLayer;
    [SerializeField] private LayerMask playerLayer;



    private void Start()
    {
        projectilePool = new List<GameObject>();

        for (int i = 0; i < projectilesPrefabs.Length; i++)
        {
            for (int j = 0; j < projectileProbabilities[i]; j++)
            {
                GameObject projectile = Instantiate(projectilesPrefabs[i % projectilesPrefabs.Length], gameObject.transform);
                projectile.SetActive(false);
                projectilePool.Add(projectile);
            }
        }

        timer = CalculateCoolDown();
    }



    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = CalculateCoolDown();
            Shoot();
        }
    }


    private void Shoot()
    {
        switch (Random.Range(0, 2))
        {
            case 0:
                shootType = RandomShoot;
                break;

            case 1:
                shootType = PreciseShoot;
                break;
            default:
                shootType = RandomShoot;
                break;
        }

        var pList = projectilePool.Where(p => !p.activeSelf).ToList();

        var projectile = pList[Random.Range(0, pList.Count)];

        projectile.SetActive(true);
        Collider2D pc = projectile.GetComponent<Collider2D>();
        Vector2 target;

        int i = 0;
        do
        {
            Camera cam = Camera.main; // Get the main camera

            float camHeight = 2f * cam.orthographicSize; // Calculate the height of the camera's view in world units
            float camWidth = camHeight * cam.aspect; // Calculate the width of the camera's view in world units

            Vector3 camBottomLeft = new Vector3(-camWidth * 0.5f, -camHeight * 0.5f, 0); // Calculate the bottom-left corner of the camera's view in world units
            Vector3 camTopRight = new Vector3(camWidth * 0.5f, camHeight * 0.5f, 0); // Calculate the top-right corner of the camera's view in world units

            target.x = Random.Range(camBottomLeft.x, camTopRight.x);
            target.y = Random.Range(camBottomLeft.y, camTopRight.y);

            i++;
            if (i > 1000)
            {
                Debug.Log("Erro no lançamento");
                break;
            }


        } while (shootType(target, Mathf.Max(pc.bounds.extents.x, pc.bounds.extents.y)));

        projectile.GetComponent<ProjectileScript>().Target = target;
        projectile.transform.position = new Vector3(target.x, Random.Range(-6, -7.5f), 0);


    }

    private void RandomShoot()
    {
        throw new NotImplementedException();
    }

    private bool RandomShoot(Vector2 target, float radius)
    {
        return Physics2D.OverlapCircle(target, radius, wallsLayer) != null;
    }

    private bool PreciseShoot(Vector2 target, float radius)
    {
        return Physics2D.OverlapCircle(target, radius, playerLayer) == null;
    }


    private float CalculateCoolDown()
    {
        return Random.Range(coolDown * 0.5f, coolDown * 1.5f);
    }
}
