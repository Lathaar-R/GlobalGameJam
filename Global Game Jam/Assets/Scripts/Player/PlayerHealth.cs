using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int health = 3;
    [SerializeField] private List<Rigidbody2D> limbs;
    [SerializeField] private GameObject limbGroundPrefab;
    [SerializeField] private Balance balanceScript;
    [SerializeField] private WalkAnimation walkAnimationScript;
    [SerializeField] private SpringJoint2D playerMovementScript;
    [SerializeField] private HingeJoint2D[] feetHinges;

    private int index;

    private void Awake()
    {
        GameController.Instance.playerDamage += TakeDamage;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void TakeDamage(int damage)
    {
        Debug.Log("Player took " + damage + " damage");
        health--;

        if (health > 0)
        {
            var limb = limbs[0];
            var limb2 = limbs[1];
            limbs.RemoveAt(0);
            limbs.RemoveAt(0);

            var limbGround = Instantiate(limbGroundPrefab, limb.position + Vector2.down, Quaternion.identity);
            limbGround.layer = LayerMask.NameToLayer("LimbGround" + index);

            var hinge = limb.gameObject.GetComponent<HingeJoint2D>();
            hinge.enabled = false;

            limb.gameObject.transform.parent = null;
            limb.gravityScale = 3;
            float randomRotation = UnityEngine.Random.Range(-60, 60); // Get a random rotation between -20 and 20 degrees
            Quaternion rotation = Quaternion.Euler(0, 0, randomRotation); // Create a rotation using the random rotation
            Vector2 rotatedVector = rotation * Vector2.up * 0.00005f; // Apply the rotation to the vector
            limb.AddForce(rotatedVector);
            limb.AddTorque(0.005f * Random.value > 0.5f ? 1 : -1);
            limb.excludeLayers = LayerMask.GetMask("Player", "Projectile", "LimbGround" + (index + 1) % 3,  "LimbGround" + (index + 2) % 3);
            limb.gameObject.layer = LayerMask.NameToLayer("Limb");

            limb2.gameObject.transform.parent = null;
            limb2.excludeLayers = LayerMask.GetMask("Player", "Projectile", "LimbGround" + (index + 1) % 3,  "LimbGround" + (index + 2) % 3);
            limb2.gravityScale = 3;
            limb2.gameObject.layer = LayerMask.NameToLayer("Limb");

            index++;

            damage--;
            if (damage > 0)
            {
                TakeDamage(damage);
            }
        }
        else
        {
            feetHinges[0].enabled = false;
            feetHinges[1].enabled = false;

            var limbGround = Instantiate(limbGroundPrefab, transform.position + Vector3.down * 2, Quaternion.identity);
            var limbGroundCol = limbGround.GetComponent<Collider2D>();
            // int k = 0;
            // do{
            //     Debug.Log(limbGround.transform.position);
            //     limbGround.transform.position += Vector3.down * 0.1f;
            //     k++;
            //     if(k > 1000)
            //     {
            //         Debug.Log("Erro na movimentação do limbGround");
            //         break;
            //     }
            // }while(limbGroundCol.IsTouchingLayers(LayerMask.GetMask("Player")));

            limbGround.layer = LayerMask.NameToLayer("LimbGround" + index);

            // walkAnimationScript.enabled = false;
            // balanceScript.enabled = false;
            // playerMovementScript.enabled = false;

            var leg = limbs[0];
            var leg2 = limbs[1];

            var leg1 = limbs[2];
            var leg12 = limbs[3];

            var body = limbs[4];

            leg.gravityScale = 3;
            leg2.gravityScale = 3;
            leg1.gravityScale = 3;
            leg12.gravityScale = 3;
            body.gravityScale = 3;

            leg.gameObject.transform.parent = null;
            leg2.gameObject.transform.parent = null;
            leg1.gameObject.transform.parent = null;
            leg12.gameObject.transform.parent = null;
            body.gameObject.transform.parent = null;


            leg.excludeLayers = LayerMask.GetMask("Player", "Projectile", "LimbGround" + (index + 1) % 3,  "LimbGround" + (index + 2) % 3);
            leg2.excludeLayers = LayerMask.GetMask("Player", "Projectile", "LimbGround" + (index + 1) % 3,  "LimbGround" + (index + 2) % 3);
            leg1.excludeLayers = LayerMask.GetMask("Player", "Projectile", "LimbGround" + (index + 1) % 3,  "LimbGround" + (index + 2) % 3);
            leg12.excludeLayers = LayerMask.GetMask("Player", "Projectile", "LimbGround" + (index + 1) % 3,  "LimbGround" + (index + 2) % 3);
            body.excludeLayers = LayerMask.GetMask("Player", "Projectile", "LimbGround" + (index + 1) % 3,  "LimbGround" + (index + 2) % 3);

            GameController.Instance.EndGamePlay();
        }
    }
}
