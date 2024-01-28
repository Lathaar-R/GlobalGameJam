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
    private float timer2;
    private int index;
    private bool isShooting;
    private Func<Vector2, float, bool> shootType;
    private Func<float, float> CoolDownCalculation;
    [SerializeField] private float divisor;
    [SerializeField] private float maxSub;
    [SerializeField] private float minCoolDown;
    [SerializeField] private float maxCoolDown;
    [SerializeField] private float coolDown = 3;
    [Header("Probabilidades: Aleatório, Perto, Preciso (Soma deve ser 1)")]
    [SerializeField] private float[] shootProbabilities;
    [SerializeField] private GameObject[] projectilesPrefabs;
    [SerializeField] private float[] projectileProbabilities;
    [SerializeField] private float[] projectileAppearTime;
    [SerializeField] private List<GameObject> projectilePool;
    [SerializeField] private LayerMask wallsLayer;
    [SerializeField] private LayerMask playerLayer;

    
    private void Awake() {
        CoolDownCalculation = (x) => x + Random.Range(x * 0.2f, x * -0.2f);


        GameController.Instance.startGamePlay += OnStartGamePlay;

        GameController.Instance.endGamePlay += OnEndGamePlay;

        GameController.Instance.startFiring += StartFiring;

        GameController.Instance.stopFiring += StopFiring;

        GameController.Instance.changeDificulty += ChangeCoolDown;
    }

    private void StopFiring()
    {
        isShooting = false;
    }

    private void StartFiring()
    {
        isShooting = true;
    }

    private void OnDisable() {

        GameController.Instance.startGamePlay -= OnStartGamePlay;

        GameController.Instance.endGamePlay -= OnEndGamePlay;

        GameController.Instance.startFiring -= StartFiring;

        GameController.Instance.stopFiring -= StopFiring;

        GameController.Instance.changeDificulty -= ChangeCoolDown;
    }

    private void OnStartGamePlay()
    {
        isShooting = true;
    }

    private void OnEndGamePlay()
    {
        this.enabled = false;
    }



    private void Start()
    {
        projectilePool = new List<GameObject>();

        for (int i = 0; i < projectilesPrefabs.Length; i++)
        {
            if(projectileAppearTime[i] > 0) continue;

            index++;
            for (int j = 0; j < projectileProbabilities[i]; j++)
            {
                GameObject projectile = Instantiate(projectilesPrefabs[i % projectilesPrefabs.Length], gameObject.transform.position, Quaternion.identity);
                projectile.SetActive(false);
                projectilePool.Add(projectile);
            }
        }

        timer = CalculateCoolDown();
    }



    private void Update()
    {
        if(!isShooting) return;

        timer -= Time.deltaTime;
        timer2 += Time.deltaTime;

        coolDown -= Mathf.Min(Time.deltaTime * (float)GameController.Instance.GetScore() / divisor, maxSub + 
                                (float)GameController.Instance.GetScore() / divisor * 10);

        coolDown = Mathf.Clamp(coolDown, minCoolDown, maxCoolDown);

        if(timer <= 0)
        {
            timer = CalculateCoolDown();
            Shoot();
        }

        for(int i = index; i < projectilesPrefabs.Length; i++)
        {
            if(timer2 < projectileAppearTime[i]) break;

            index++;

            for (int j = 0; j < projectileProbabilities[i]; j++)
            {
                GameObject projectile = Instantiate(projectilesPrefabs[i % projectilesPrefabs.Length], gameObject.transform.position, Quaternion.identity);
                projectile.SetActive(false);
                projectilePool.Add(projectile);
            } 
        }
    }


    private void Shoot()
    {
        var r = Random.value;

        if(r < shootProbabilities[0])
        {
            shootType = RandomShoot;
        }
        else if(r < shootProbabilities[1])
        {
            shootType = NearShoot;
        }
        else
        {
            shootType = PreciseShoot;
        }
        

        var pList = projectilePool.Where(p => !p.activeSelf).ToList();

        var projectile = pList[Random.Range(0, pList.Count)];

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

        

        var projc = projectile.GetComponent<ProjectileScript>();
        projc.Target = target;
        projectile.transform.position = new Vector3(target.x, Random.Range(-6, -7.5f), 0);

        projectile.SetActive(true);
    }

    private void RandomShoot()
    {
        throw new NotImplementedException();
    }

    private bool RandomShoot(Vector2 target, float radius)
    {
        return Physics2D.OverlapCircle(target, radius, wallsLayer) != null;
    }

    private bool NearShoot(Vector2 target, float radius)
    {
        return Physics2D.OverlapCircle(target, radius * 5, playerLayer) != null;
    }

    private bool PreciseShoot(Vector2 target, float radius)
    {
        return Physics2D.OverlapCircle(target, radius, playerLayer) == null;
    }

    public void ChangeCoolDown(float value)
    {
        coolDown += value;
    }

    public void ChangeCoolDownCalculation(Func<float, float> action)
    {
        CoolDownCalculation = action;
    }

    private float CalculateCoolDown()
    {
        return CoolDownCalculation(coolDown);
    }
}
