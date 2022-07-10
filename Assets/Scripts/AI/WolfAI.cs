using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WolfAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject target;

    public float turnSpeed;

    public float chaseSpeed;
    public float idleSpeed;

    public States states;
    public enum States
    {
        chase,
        stalk,
        leap,
        circle,
        flee,
        idle
    }

    [Range(0, 10)]
    public int decreaseRayCasts;
    int raycastCalculations;

    public float distance;

    RaycastHit hitInfo1;
    RaycastHit hitInfo2;
    RaycastHit hitInfo3;
    RaycastHit hitInfo4;
    private void LateUpdate()
    {

        Vector3 rotation = Math.GetRot(hitInfo1.point, hitInfo2.point, hitInfo3.point, hitInfo4.point);

        var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.GetChild(0).eulerAngles = new Vector3(0, Quaternion.Slerp(transform.GetChild(0).rotation, targetRotation, 5 * Time.deltaTime).eulerAngles.y, 0);

        transform.localEulerAngles = new Vector3(rotation.x * -1, 0, rotation.z);

        if (decreaseRayCasts != 0)
        {
            raycastCalculations++;
            if (raycastCalculations == decreaseRayCasts)
                raycastCalculations = 0;
            if (raycastCalculations == 0)
            {
                Physics.Raycast(transform.position + new Vector3(-.5f, 1, .5f), -transform.up, out hitInfo1, 5, 9);
                Physics.Raycast(transform.position + new Vector3(.5f, 1, .5f), -transform.up, out hitInfo2, 5, 9);
                Physics.Raycast(transform.position + new Vector3(-0.5f, 1, -.5f), -transform.up, out hitInfo3, 5, 9);
                Physics.Raycast(transform.position + new Vector3(0.5f, 1, -.5f), -transform.up, out hitInfo4, 5, 9);
            }
        }

        
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance < 10)
            states = States.chase;
        else
            states = States.idle;

        



        if (states == States.chase)
        {
            agent.SetDestination(target.transform.position);
        }
        else if (states == States.circle)
        {

        }
        else if (states == States.flee)
        {

        }
        else if (states == States.idle)
        {

        }
        else if (states == States.leap)
        {

        }
        else if (states == States.stalk)
        {

        }
    }
}
