using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public float jumpHeight;
    public float leapHeight;
    public float leapLength;
    bool isJumpAttack;
    public Rigidbody rb;
    public InverseKinematics ik;
    public Spider spider;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w") && Input.GetKeyDown(KeyCode.Space) && spider.isGrounded)
            LeapAttack(0, leapHeight, leapLength);

        else if (Input.GetKey("s") && Input.GetKeyDown(KeyCode.Space) && spider.isGrounded)
            Leap(0, leapHeight, -leapLength);

        else if (Input.GetKey("a") && Input.GetKeyDown(KeyCode.Space) && spider.isGrounded)
            Leap(-leapLength, leapHeight, 0);

        else if (Input.GetKey("d") && Input.GetKeyDown(KeyCode.Space) && spider.isGrounded)
            Leap(leapLength, leapHeight, 0);

        else if (Input.GetKeyDown(KeyCode.Space) && spider.isGrounded)
            Leap(0, jumpHeight, 0);
    }

    public void LeapAttack(float x, float y, float z)
    {
        isJumpAttack = true;
        spider.DetachFromGround();

        rb.AddRelativeForce(x, y, z);
    }

    public void Leap(float x, float y, float z)
    {
        spider.DetachFromGround();

        rb.AddRelativeForce(x, y, z);
    }


    public void OnCollisionEnemy(GameObject enemy, float damage)
    {
        if (isJumpAttack)
            enemy.GetComponent<Enemy>().health -= damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
            OnCollisionEnemy(collision.gameObject, Player.damage);
    }
}
