using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCrab : MonoBehaviour
{
    public bool isActive;
    private GameManager gameManager;

    [SerializeField] private GameObject missile;
    [SerializeField] private GameObject explosion;
    private Vector3 initialMissiliPos = new Vector3(-11.51f, 3.29f, 13f);
    private Quaternion initialMissiliRot = Quaternion.Euler(-49.89f, 115.86f, -127.73f);

    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.FindWithTag("Player");

        StartCoroutine(Activate());
    }

    private IEnumerator Activate()
    {
        yield return new WaitUntil(() => isActive);
        StartCoroutine(MoveUp());
        WaitForSeconds w = new WaitForSeconds(6);
        yield return w;

        while (gameManager.isGameOn)
        {
            StartCoroutine(ShootMissile());
            yield return w;
        }

    }

    private IEnumerator MoveUp()
    {
        Vector3 initialPos = transform.position;
        Vector3 finalPos = new Vector3(-13.7f, -9.9f, 26.2f);
        
        float totalTime = 5f;
        float timeLeft = totalTime;

        while (timeLeft > 0)
        {            
            float factor = (totalTime - timeLeft) / totalTime;

            factor = Mathf.SmoothStep(0, 1, factor);

            transform.position = Vector3.Lerp(initialPos, finalPos, factor);

            yield return null;
            timeLeft -= Time.deltaTime;
        }
    }

    private IEnumerator ShootMissile()
    {
        GameObject insMissile = Instantiate(missile, initialMissiliPos, initialMissiliRot);
        Vector3 finalPos = player.transform.position;

        insMissile.transform.LookAt(finalPos);
        insMissile.transform.Rotate(90, 0, 0);

        float duration = 2f;
        while (duration > 0)
        {
            insMissile.transform.position = Vector3.Lerp(finalPos, initialMissiliPos, duration);

            yield return null;
            duration -= Time.deltaTime;
        }

        GameObject insExplosion = Instantiate(explosion, insMissile.transform.position, Quaternion.identity);

        ParticleSystem particles = insExplosion.GetComponentInChildren<ParticleSystem>();
        BoxCollider boxCollider = insExplosion.GetComponent<BoxCollider>();
        insExplosion.GetComponent<AudioSource>().volume = MainManager.Instance.volume;

        Destroy(insMissile);
        yield return new WaitForSeconds(2);
        boxCollider.enabled = false;
        yield return new WaitForSeconds(2);
        Destroy(insExplosion);
    }
}
