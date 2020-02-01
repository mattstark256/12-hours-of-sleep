using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappler : MonoBehaviour
{
    [SerializeField]
    private float grappleDistance = 5;

    private GrapplePoint[] grapplePoints;

    private bool grappling = false;
    private GrapplePoint currentGrapplePoint;
    private float currentGrappleLength;

    private Rigidbody2D playerRigidBody;

    void Start()
    {
        grapplePoints = FindObjectsOfType<GrapplePoint>();
        Debug.Log(grapplePoints.Length);

        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("looking for grapple");
            GrapplePoint nearestGrapplePoint = null;
            float shortestDistance = 0;
            foreach(GrapplePoint grapplePoint in grapplePoints)
            {
                float distance = Vector3.Distance(transform.position, grapplePoint.transform.position);
                if (nearestGrapplePoint == null || distance < shortestDistance)
                {
                    nearestGrapplePoint = grapplePoint;
                    shortestDistance = distance;
                }
            }

            if (nearestGrapplePoint != null && shortestDistance < grappleDistance)
            {
                // A valid grapple point has been found.
                Debug.Log("connecting to grapple");

                grappling = true;
                currentGrapplePoint = nearestGrapplePoint;
                currentGrappleLength = shortestDistance;
            }
        }

        if (grappling && !Input.GetKey(KeyCode.C))
        {
            // Stopped grappling
            grappling = false;
            currentGrapplePoint = null;
        }

    }

    private void FixedUpdate()
    {
        if (grappling)
        {
            Vector3 vectorToGrapplePoint = currentGrapplePoint.transform.position - transform.position;

            Debug.DrawLine(transform.position, currentGrapplePoint.transform.position, Color.red);


            Vector3 newVelocity = playerRigidBody.velocity;
            if (vectorToGrapplePoint.magnitude > currentGrappleLength)
            {
                // Teleport to keep it within rope range
                gameObject.SetActive(false);
                transform.position = currentGrapplePoint.transform.position - vectorToGrapplePoint.normalized * currentGrappleLength;
                gameObject.SetActive(true);

                // Preserve existing velocity
                Vector3 tangentVector = (Quaternion.Euler(0, 0, 90) * vectorToGrapplePoint).normalized;
                newVelocity = tangentVector * newVelocity.magnitude * Mathf.Sign(Vector3.Dot(tangentVector, newVelocity));

                // Add force along rope
                Vector3 gravityForce = Physics.gravity * playerRigidBody.gravityScale * playerRigidBody.mass;
                float forceAmount = Vector3.Dot(gravityForce, vectorToGrapplePoint.normalized);
                Vector3 forceVector = vectorToGrapplePoint.normalized * -forceAmount;
                newVelocity += (forceVector / playerRigidBody.mass) * Time.fixedDeltaTime;

                playerRigidBody.velocity = newVelocity;

                //Debug.DrawLine(transform.position, transform.position + gravityForce / playerRigidBody.mass * Time.fixedDeltaTime);
                //Debug.DrawLine(transform.position, transform.position + forceVector / playerRigidBody.mass * Time.fixedDeltaTime);
            }
        }
    }
}
