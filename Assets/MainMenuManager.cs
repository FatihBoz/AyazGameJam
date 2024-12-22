using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private int sceneIndex;



    private void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    void OnStartButtonClicked()
    {
        LoadScene(sceneIndex);
    }

    void OnExitButtonClicked()
    {
        Application.Quit();
    }

}
