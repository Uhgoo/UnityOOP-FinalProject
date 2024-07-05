using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TraversableGround : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private BoxCollider boxCollider;
    private MeshCollider playerCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerCollider = player.gameObject.GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y > gameObject.transform.position.y + GetPlayerSizeYForPlat())
        {
            boxCollider.enabled = true;
        }
        else
        {
            boxCollider.enabled = false;
        }
    }

    private float GetPlayerSizeYForPlat()
    {
        return playerCollider.bounds.size.y / 4;
    }
}
