using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NinjaCrab : Crabs
{
    private Rigidbody rb;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private ParticleSystem poufParticle;
    [SerializeField] private GameObject body;

    private float force = 15f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(RemoveCollisionsTemp());
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (transform.position.x > 16)
        {
            rb.AddForce(Vector3.left * 100, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            rb.AddForce(Vector3.up * force, ForceMode.Impulse);
            rb.AddForce(Vector3.right * Random.Range(-1f, 1f), ForceMode.Impulse);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            gameManager.GameOver();
        }
    }

    private IEnumerator RemoveCollisionsTemp()
    {
        boxCollider.isTrigger = true;
        yield return new WaitUntil(() => (transform.position.y < 9 && transform.position.x < 10));
        boxCollider.isTrigger = false;
        yield return new WaitForSeconds(10);
        StartCoroutine(SpawnDispawnParticle());
    }

    private IEnumerator SpawnDispawnParticle()
    {
        poufParticle.Play();
        rb.isKinematic = true;
        Destroy(body);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
