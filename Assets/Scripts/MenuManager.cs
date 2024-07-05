using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TextMeshProUGUI highscoreText;
    [SerializeField] public GameObject[] foods;
    private GameObject instantiatedFood;
    private Vector3 posInstantiated = new Vector3(0, -3, 0);
    [SerializeField] private UnityEngine.UI.Slider slider;

    private void Start()
    {
        if (MainManager.Instance.topScore != 0)
        {
            highscoreText.text = "Highest Score: " + MainManager.Instance.topPlayerName + " - " + MainManager.Instance.topScore;
        }
        SetSelectedFoodVisual();
        slider.value = MainManager.Instance.volume;
    }

    public void SetName()
    {
        MainManager.Instance.playerName = nameField.text;
    }

    public void StartGame()
    {
        if (nameField.text == "")
        {
            MainManager.Instance.playerName = "Unknown";
        }
        SceneManager.LoadScene(1);
    }

    public void SetFood(int num)
    {
        Destroy(instantiatedFood);
        int value = (MainManager.Instance.selectedFood + num) % 10;
        if (value < 0)
            value += 10;
        MainManager.Instance.selectedFood = value;
        SetSelectedFoodVisual();
    }
    
    private void SetSelectedFoodVisual()
    {
        instantiatedFood = Instantiate(foods[MainManager.Instance.selectedFood], posInstantiated, Quaternion.identity);
        instantiatedFood.AddComponent<RotateOnItself>();
        instantiatedFood.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void VolumeSetter()
    {
        MainManager.Instance.GetComponent<AudioSource>().volume = slider.value;
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
