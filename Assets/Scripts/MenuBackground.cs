using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private MenuManager menuManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnBananas());
        menuManager = GameObject.Find("Canvas").GetComponent<MenuManager>();
    }


    IEnumerator SpawnBananas()
    {
        WaitForSeconds wait = new WaitForSeconds(2);
        yield return null;
        while (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("menu"))
        {
            GameObject flyingFood = Instantiate(menuManager.foods[MainManager.Instance.selectedFood], GenerateRandomPos(), Quaternion.identity);
            flyingFood.transform.localScale = new Vector3(200, 200, 200);
            flyingFood.GetComponent<Rigidbody>().mass = 1f;
            flyingFood.AddComponent<FlyingBanana>();
            yield return wait;
        }
    }

    private Vector3 GenerateRandomPos()
    {
        Vector3 pos = Vector3.zero;
        switch (Random.Range(0, 3))
        {
            case 0:
                pos = new Vector3(-12, Random.Range(-5f, 5f), 0);
               break;
            case 1:
                pos = new Vector3(12, Random.Range(-5f, 5f), 0);
                break;
            case 2:
                pos = new Vector3(Random.Range(-10f, 10f), -6, 0);
                break;
        }
        return pos;
    }
}
