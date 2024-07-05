using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crabs : MonoBehaviour
{
    public GameManager gameManager;
    private PlayerMovement playerMovement;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GameObject x = GameObject.FindWithTag("Player");
        if (x != null)
        {
            playerMovement = x.GetComponent<PlayerMovement>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerMovement.IsHitted(gameObject);
        }
    }
}
