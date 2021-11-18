using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    public Vector3 newDirection;

    private Vector3 initialPosition;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > startTime + 20f)
        {
            Alive();
            startTime = Time.time;
            transform.position = initialPosition;
        }
        transform.position += newDirection * Time.deltaTime;
    }

    private void Alive()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }

    public void Die()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Hippo"))
        {
            collision.collider.GetComponent<Hippo_AI>().Die();
        }
	}
}
