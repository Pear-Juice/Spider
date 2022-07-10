using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static float health;
    public static float damage;
    public static float stamina;

    public float _health; 
    public float _damage;
    public float _stamina;

    // Start is called before the first frame update
    void Awake()
    {
        health = _health;
        damage = _damage;
        stamina = _stamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Death();
    }

    public void Death()
    {
        if (transform.GetComponent<Spider>() != null)
            Destroy(transform.GetComponent<Spider>());

        if (transform.GetComponent<Rigidbody>() == null)
            gameObject.AddComponent<Rigidbody>();
    }
}
