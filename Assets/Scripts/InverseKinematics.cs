using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]

public class InverseKinematics : MonoBehaviour
{
    [Header("Armature Objects")]
    public Transform[] upperLeg;
    public Transform[] lowerLeg;
    public Transform[] foot;
    public Transform[] knee;
    public Transform[] target;
    public Transform[] goalPositions;
    public Transform[] goal;
    public Vector3[] footRaycastPosition;
    public Transform goalContainer;

    [Space(20)]
    public Vector3 uppperArm_OffsetRotation;
    public Vector3 forearm_OffsetRotation;
    public Vector3 hand_OffsetRotation;

    [Space(20)]
    public float bodyOffset;
    public float footReturnStep;
    public float footReturnDistance;

    Vector3 averagedFootPosition;

    [Header("Performance")]
    [Space(20)]
    [Range(0,10)]
    public int decreaseIKCalculations;
    [Range(0, 10)]
    public int decreaseRayCasts;
    public bool decreaseLegAnimations;

    [Header("Debug")]
    [Space(20)]
    public bool ikIsActive;
    public bool lockMovement;
    public float distanceFromGround;
    public Vector3 lastRecordedPostion;
    public Quaternion lastRecordedRotation;

    float angle;
    float _upperLeg_Length;
    float _lowerLeg_Length;
    float arm_Length;
    float targetDistance;
    float adyacent;
    int ikCalculations;
    int raycastCalculations;

    // Use this for initialization

    void Start()
    {
        footRaycastPosition[0] = goal[0].position;
        footRaycastPosition[1] = goal[1].position;

        for (int i = 0; i < target.Length; i++)
        {
            StartCoroutine(SetPositions(i));
        }
    }

    private void LateUpdate()
    {
        goalContainer.position = transform.position;
        goalContainer.eulerAngles = new Vector3(goalContainer.eulerAngles.x, transform.eulerAngles.y, goalContainer.eulerAngles.x);

        float[] distances = new float[8];
        RaycastHit hitInfo;

        if (decreaseRayCasts != 0)
        {
            raycastCalculations++;
            if (raycastCalculations == decreaseRayCasts)
                raycastCalculations = 0;
            if (raycastCalculations == 0)
                for (int i = 0; i < target.Length; i++)
                    if (Physics.Raycast(goalPositions[i].position, -transform.up * 40, out hitInfo))
                        if (hitInfo.transform.GetComponent<Rigidbody>() == null)
                        {
                            distances[i] = hitInfo.distance;
                            goal[i].position = hitInfo.point;
                        }
        }
        else
            for (int i = 0; i < target.Length; i++)
                if (Physics.Raycast(goalPositions[i].position, -transform.up * 40, out hitInfo))
                    if (hitInfo.transform.GetComponent<Rigidbody>() == null)
                    {
                        distances[i] = hitInfo.distance;
                        goal[i].position = hitInfo.point;
                    }


        for (int i = 0; i < target.Length; i++)
        {
            Debug.DrawRay(goalPositions[i].position, -transform.up * 4);
        }

        for (int i = 0; i < target.Length; i++)
        {
            distanceFromGround += distances[i];
        }
        distanceFromGround /= target.Length; 

        if (distanceFromGround > 4)
        {
            lastRecordedPostion = transform.position;
            lastRecordedRotation = transform.rotation;
        }

        


        float[] distance = new float[target.Length];
        for (int i = 0; i < target.Length; i++)
        {
            distance[i] = Vector3.Distance(goal[i].position, target[i].position);
        }

        for (int i = 0; i < target.Length; i += 2)
        {
            if (distance[i] > footReturnDistance)
                StartCoroutine(SetPositions(i));

            if (distance[i + 1] > footReturnDistance && distance[i] < footReturnDistance / 2 + .2 && distance[i] > footReturnDistance / 2 - .2)
                StartCoroutine(SetPositions(i + 1));
        }

        //set variables of sideways tilt of body based on leg height difference

        if (lockMovement)
        {
            Vector3 rotationOfBody = Math.GetRot(target[0].position, target[1].position, target[6].position, target[7].position);
            //set tilt of player
            transform.localEulerAngles = new Vector3(rotationOfBody.x, transform.eulerAngles.y, rotationOfBody.z);
        }

            //Change player height based on averaged height of legs.
            averagedFootPosition = new Vector3();
            for (int i = 0; i < target.Length; i++)
            {
                averagedFootPosition += target[i].position;
            }
            averagedFootPosition /= target.Length;

        if (lockMovement)
            transform.position = new Vector3(transform.position.x, averagedFootPosition.y + bodyOffset, transform.position.z);


        if (ikIsActive)
        {
            if (decreaseIKCalculations != 0)
            {
                ikCalculations++;
                if (ikCalculations == decreaseIKCalculations)
                    ikCalculations = 0;

                if (ikCalculations != 0)
                    return;
            }
            for (int i = 0; i < target.Length; i++)
            {
                IKUpdate(upperLeg[i], lowerLeg[i], foot[i], target[i], knee[i]);
            }
        }
    }

    public void UpdatePositions()
    {
        for (int i = 0; i < target.Length; i += 2)
        {
                StartCoroutine(SetPositions(i));
                StartCoroutine(SetPositions(i + 1));
        }
    }

    IEnumerator SetPositions(int index)
    {
        if (!decreaseLegAnimations)
        {
            target[index].position = Vector3.MoveTowards(target[index].position, goal[index].position, footReturnStep);

            if (target[index].position == goal[index].position)
            {
                StopCoroutine(SetPositions(index));
            }
            else
            {
                yield return new WaitForSeconds(0.01f);
                StartCoroutine(SetPositions(index));
            }
        }
        else
        {
            target[index].position = goal[index].position;
        }
    }

    public void IKUpdate(Transform _upperLeg, Transform _lowerLeg, Transform _foot, Transform _target, Transform _knee)
    {
        _upperLeg.LookAt(_target, _knee.position - _upperLeg.position);
        _upperLeg.Rotate(uppperArm_OffsetRotation);

        Vector3 cross = Vector3.Cross(_knee.position - _upperLeg.position, _lowerLeg.position - _upperLeg.position);



        _upperLeg_Length = Vector3.Distance(_upperLeg.position, _lowerLeg.position);
        _lowerLeg_Length = Vector3.Distance(_lowerLeg.position, _foot.position);
        arm_Length = _upperLeg_Length + _lowerLeg_Length;
        targetDistance = Vector3.Distance(_upperLeg.position, _target.position);
        targetDistance = Mathf.Min(targetDistance, arm_Length - arm_Length * 0.001f);

        adyacent = ((_upperLeg_Length * _upperLeg_Length) - (_lowerLeg_Length * _lowerLeg_Length) + (targetDistance * targetDistance)) / (2 * targetDistance);

        angle = Mathf.Acos(adyacent / _upperLeg_Length) * Mathf.Rad2Deg;

        if (!float.IsNaN(angle))
            _upperLeg.RotateAround(_upperLeg.position, cross, -angle);

        _lowerLeg.LookAt(_target, cross);
        _lowerLeg.Rotate(forearm_OffsetRotation);
    }

    public void SlopeTooHigh()
    {
        List<GameObject> childrenList = new List<GameObject>();
        Transform[] children = GetComponentsInChildren<Transform>(true);

        for (int i = 0; i < children.Length; i++)
        {
            Transform child = children[i];
            if (child != transform)
            {
                childrenList.Add(child.gameObject);
            }
        }

        for (int i = 0; i < childrenList.Count; i++)
        {
            Rigidbody rb = childrenList[i].AddComponent<Rigidbody>();
        }
    }
}
