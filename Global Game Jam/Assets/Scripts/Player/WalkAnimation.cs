using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAnimation : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform[] foot;
    [SerializeField] private float maxDistance;
    [SerializeField] private float overShoot;
    [SerializeField] private float walkSpeed;
    [SerializeField] private HingeJoint2D[] footPositions;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private SpringJoint2D springJoints;

    private bool isMoving = true;
    bool f;

    private void Awake() {
        GameController.Instance.startGamePlay += OnStartGamePlay; 
        GameController.Instance.endGamePlay += OnEndGamePlay;
    }
    
    private void OnDisable() {
        GameController.Instance.startGamePlay -= OnStartGamePlay;
        GameController.Instance.endGamePlay -= OnEndGamePlay;
    }

    private void OnStartGamePlay()
    {
        isMoving = false;
    }

    private void OnEndGamePlay()
    {
        springJoints.enabled = false;
        this.enabled = false;
    }


    void Update()
    {
        if (!isMoving)
        {
            if (f && Vector2.Distance(foot[0].position, footPositions[0].connectedAnchor) > maxDistance)
            {
                // if (footPositions[0].connectedAnchor.x < foot[0].position.x)
                // {
                //     StartCoroutine(MoveFoot(footPositions[0], foot[0], overShoot, 0.5f, curve));
                // }
                // else
                // {
                    StartCoroutine(MoveFoot(footPositions[0], foot[0], overShoot, 0.5f, curve));
                //}
            }
            else if (!f && Vector2.Distance(foot[1].position, footPositions[1].connectedAnchor) > maxDistance)
            {
                // if (footPositions[1].connectedAnchor.x < foot[1].position.x)
                // {
                //     StartCoroutine(MoveFoot(footPositions[1], foot[1], overShoot, 0.5f, curve));
                // }
                // else
                // {
                    StartCoroutine(MoveFoot(footPositions[1], foot[1], overShoot, 0.5f, curve));
                //}
            }
        }

        // if (Vector2.Distance(foot[1].position, footPositions[1].connectedAnchor) > maxDistance)
        // {
        //     if(footPositions[1].connectedAnchor.x < foot[1].position.x)
        //     {
        //         footPositions[1].connectedAnchor = foot[1].position + Vector3.right * overShoot;
        //     }
        //     else
        //     {
        //         footPositions[1].connectedAnchor = foot[1].position - Vector3.right * overShoot;
        //     }
        // }
    }

    private IEnumerator MoveFoot(HingeJoint2D footPosition, Transform foot, float overShoot, float amplitude, AnimationCurve curve)
    {
        isMoving = true;
        float time = 0;
        f = !f;
        Vector2 startPos = footPosition.connectedAnchor;

        while (time <= walkSpeed)
        {
            float x = Mathf.Lerp(startPos.x, (foot.position + (foot.position - (Vector3)startPos) * overShoot).x, time / walkSpeed); // Linear interpolation between start and target x positions
            float y = Mathf.Lerp(startPos.y, (foot.position + (foot.position - (Vector3)startPos) * overShoot).y, time / walkSpeed); // Use AnimationCurve to control the arc
            y += amplitude * Mathf.Sin(Mathf.PI * curve.Evaluate(time / walkSpeed)); // Use AnimationCurve to control the arc

            footPosition.connectedAnchor = new Vector2(x, y);

            Debug.DrawLine(startPos, footPosition.connectedAnchor, Color.red);

            time += Time.deltaTime; // Increase the time by the time it took to complete the last frame
            yield return new WaitForEndOfFrame(); // Wait for the next frame before continuing the loop
        }

        //footPosition.connectedAnchor = foot.position; // Ensure the final position is the target
        isMoving = false;
    }
}
