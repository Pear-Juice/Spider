using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Spider : MonoBehaviour
{

    public float speed;
    public float strafeSpeed;
    public float turnSpeed;
    public bool isGrounded;
    public Rigidbody rb;
    public InverseKinematics ik;
    float time;
    bool isIdle = true;
    bool isWalking = false;
    Vector3 velocity;
    Vector3 lastPos;

    private void LateUpdate()
    {
        velocity = (transform.position - lastPos) / Time.deltaTime;
        lastPos = transform.position;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        if (ik.distanceFromGround < 4)
            isGrounded = true;
        else
            isGrounded = false;

        if (ik.distanceFromGround <= 1)
        {
            time = Time.time;
            rb.useGravity = false;
            rb.isKinematic = true;
            ik.lockMovement = true;
            ik.ikIsActive = true;

            transform.position = ik.lastRecordedPostion;
            transform.rotation = ik.lastRecordedRotation;
        }


        transform.Translate(Input.GetAxisRaw("Horizontal") * strafeSpeed * Time.deltaTime, 0, Input.GetAxisRaw("Vertical") * speed * Time.deltaTime);
        transform.Rotate(0, Input.GetAxisRaw("Mouse X") * turnSpeed, 0);

        #region if is on ground

        if (velocity.magnitude > 4)
        {
            if (isIdle)
            {
                isIdle = false;
            }

            isWalking = true;
            transform.hasChanged = false;
        }

        if (velocity.magnitude < 4)
        {
            if (isWalking)
            {
                isWalking = false;
            }

            isIdle = true;
        }
        #endregion

        if (isGrounded && Time.fixedTime - .2 > time)
        {
            time = Time.time;
            rb.useGravity = false;
            rb.isKinematic = true;
            ik.lockMovement = true;
        }

    }

    public void DetachFromGround()
    {
        time = Time.time;
        rb.useGravity = true;
        rb.isKinematic = false;
        ik.lockMovement = false;
    }

    public void IdleStance()
    {
        List<GameObject> goals = GameObject.FindGameObjectsWithTag("Goal").ToList();

        goals[0].transform.localPosition = new Vector3(-3.3f, 0, 2.4f);
        goals[1].transform.localPosition = new Vector3(3.3f, 0, 2.4f);

        goals[2].transform.localPosition = new Vector3(-3.3f, 0, -1.9f);
        goals[3].transform.localPosition = new Vector3(3.3f, 0, -1.9f);

        goals[4].transform.localPosition = new Vector3(-2, 0, -4.56f);
        goals[5].transform.localPosition = new Vector3(2, 0, -4.56f);

        goals[6].transform.localPosition = new Vector3(-1.36f, 0, 5.58f);
        goals[7].transform.localPosition = new Vector3(1.36f, 0, 5.58f);

        ik.UpdatePositions();
    }

    public void WalkingStance(Vector3 offset, bool update = false)
    {
        List<GameObject> goals = GameObject.FindGameObjectsWithTag("Goal").ToList();
        goals[0].transform.localPosition = new Vector3(-2.3f, 0, 2.4f) + offset;
        goals[1].transform.localPosition = new Vector3(2.3f, 0, 2.4f) + offset;

        goals[2].transform.localPosition = new Vector3(-2.3f, 0, 0) + offset;
        goals[3].transform.localPosition = new Vector3(2.3f, 0, 0) + offset;

        goals[4].transform.localPosition = new Vector3(-2.3f, 0, -2.5f) + offset;
        goals[5].transform.localPosition = new Vector3(2.3f, 0, -2.5f) + offset;

        goals[6].transform.localPosition = new Vector3(-2.3f, 0, 4.5f) + offset;
        goals[7].transform.localPosition = new Vector3(2.3f, 0, 4.5f) + offset;

        if (update)
            transform.GetComponent<InverseKinematics>().UpdatePositions();
    }
}