using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Grapple : MonoBehaviour
{
    public float moveGrappleStep;
    public float grapplePower;
    public float grapplePowerLimit;
    public float grappleDistanceLimit;
    Vector3 grapplePoint;
    public GameObject grappleObject;
    bool canGrapple;
    public Rigidbody rb;
    public InverseKinematics ik;
    public GameObject playerObject;
    public GameObject cameraObject;
    public Spider spider;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GrappleImpulse();
        }

        if (Input.GetMouseButton(0) && canGrapple)
        {
            grappleObject.transform.position = Vector3.MoveTowards(grappleObject.transform.position, grapplePoint, moveGrappleStep);
            grappleObject.GetComponent<LineRenderer>().enabled = true;

            Vector3[] linePositions = { playerObject.transform.position, grappleObject.transform.position };
            grappleObject.GetComponent<LineRenderer>().SetPositions(linePositions);

            if (Vector3.Distance(grappleObject.transform.position, grapplePoint) < 1)
                GrappelConstant();
        }
        else
        {
            grappleObject.GetComponent<LineRenderer>().enabled = false;
        }
    }

    public void GrappelConstant()
    {
        spider.DetachFromGround();

            if (Vector3.Distance(transform.parent.position, grapplePoint) > 3)
            {
                grappleObject.transform.LookAt(playerObject.transform.position, Vector3.up);
                grappleObject.transform.forward = -grappleObject.transform.forward;
                transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, grappleObject.transform.rotation, 5 * Time.deltaTime);
            }

            if (rb.velocity.magnitude < grapplePowerLimit)
                rb.AddExplosionForce(-(grapplePower * Time.deltaTime), grapplePoint, 1000);

    }
    public void GrappleImpulse()
    {
        RaycastHit grappleHit;
        if (Physics.Raycast(cameraObject.transform.position, cameraObject.transform.forward, out grappleHit, grappleDistanceLimit))
        {
            canGrapple = true;
            grappleObject.transform.position = playerObject.transform.position;
            grapplePoint = grappleHit.point;
        }
        else
        {
            canGrapple = false;
        }
    }
}
