using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool isGrounded;
    private Rigidbody rb;
    private float jumpForce = 2.5f;
    private float speed = 250;
    private GameManager gameManager;

    public GameObject poufParticle { private get; set; }
    public bool isPowerOn { get; private set; }
    public float powerLeft { get; set; }

    [SerializeField] private Material powerUpMaterial;

    
    // Start is called before the first frame update
    void Start()
    {
        isGrounded = true;
        isPowerOn = false;
        powerLeft = 0;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.isGameOn)
        {
            if (Input.GetKey(KeyCode.A))
            {
                rb.AddForce(Vector3.left * Time.deltaTime * speed, ForceMode.Force);
            }

            if (Input.GetKey(KeyCode.D))
            {
                rb.AddForce(Vector3.right * Time.deltaTime * speed, ForceMode.Force);
            }

            if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space)) && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }

            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(Vector3.down * Time.deltaTime * speed, ForceMode.Force);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (Input.GetKey(KeyCode.S))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Liane"))
        {
            rb.useGravity = false;
        }

        if (other.CompareTag("Explosion"))
        {
            IsHitted(other.gameObject);
        }

        if (other.CompareTag("PowerUp"))
        {
            Destroy(other.gameObject);
            gameManager.GetComponent<AudioSource>().volume = MainManager.Instance.volume;
            gameManager.GetComponent<AudioSource>().Play();
            StartCoroutine(PowerEffect());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Liane"))
        {
            rb.velocity *= 0.95f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Liane"))
        {
            rb.useGravity = true;
        }
    }

    // ABSTRACTION
    public void Explode()
    {
        rb.isKinematic = true;
        gameObject.SetActive(false);
        Instantiate(gameManager.explosionList[MainManager.Instance.selectedFood], transform.position, Quaternion.identity);
    }

    private IEnumerator PowerEffect()
    {
        Renderer renderer = GetComponent<Renderer>();
        Material originalMaterial = renderer.material;

        isPowerOn = true;
        renderer.material = powerUpMaterial;
        powerLeft += 10;

        while (powerLeft > 0)
        {
            powerLeft -= Time.deltaTime;
            yield return null;
        }

        isPowerOn = false;
        renderer.material = originalMaterial;
    }

    // ABSTRACTION
    public void IsHitted(GameObject other)
    {
        if (isPowerOn)
        {
            GameObject i = Instantiate(poufParticle, other.transform.position,Quaternion.identity);
            i.transform.localScale *= 5;
            poufParticle.GetComponent<ParticleSystem>().Play();
            Destroy(other);
            powerLeft = 0;
            StartCoroutine(DestroyParticle(i));
        }
        else
            gameManager.GameOver();
    }

    private IEnumerator DestroyParticle(GameObject i)
    {
        yield return new WaitForSeconds(3);
        Destroy(i);
    }
}
