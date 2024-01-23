using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherScript : MonoBehaviour
{
    [SerializeField] private GameObject[] projectilesPrefabs;
    [SerializeField] private List<GameObject> projectilePool;

    private void Awake() {
        projectilePool = new List<GameObject>();

        for(int i = 0; i < projectilesPrefabs.Length * 10; i++) {
            GameObject projectile = Instantiate(projectilesPrefabs[i % projectilesPrefabs.Length]);
            projectile.SetActive(false);
            projectilePool.Add(projectile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
