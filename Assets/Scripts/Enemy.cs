using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public float health;
    public float damage;

    public bool contactDamage;
    public float contactDamageDelay;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
            Death();
    }

    void Death()
    {
        if (transform.GetComponent<WolfAI>() != null)
            Destroy(transform.GetComponent<WolfAI>());

        if (transform.GetComponent<Enemy>() != null)
            Destroy(transform.GetComponent<Enemy>());

        if (transform.GetComponent<NavMeshAgent>() != null)
            Destroy(transform.GetComponent<NavMeshAgent>());

        if (transform.GetComponent<Rigidbody>() != null)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.GetComponent<Rigidbody>().useGravity = true;
        }

        transform.GetComponent<MeshRenderer>().material.color = Color.red;
        transform.tag = "Untagged";
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player" && contactDamage)
        {
            Debug.Log("Damage");
            StopCoroutine(DealDamage(other.gameObject));
            StartCoroutine(DealDamage(other.gameObject));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (contactDamage)
            StopAllCoroutines();
    }

    IEnumerator DealDamage(GameObject other)
    {
        Player.health -= damage;
        yield return new WaitForSeconds(contactDamageDelay);
        StartCoroutine(DealDamage(other));
    }
}