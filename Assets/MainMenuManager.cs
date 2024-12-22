using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private int sceneIndex;

    [Header("FADE")]
    [SerializeField]
    private Image blackFade;
    [SerializeField]
    private float fadeDuration = .5f;
    [SerializeField]
    private bool fadeEnable = true;

    private bool fadeStart = false;


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
        blackFade.gameObject.SetActive(true);
        fadeStart = true;
    }

    void OnExitButtonClicked()
    {
        Application.Quit();
    }


    private void Update()
    {
        if (fadeStart)
        {
            if (blackFade.color.a >= 1f)
            {
                LoadScene(sceneIndex);
            }
            else
            {
                Color fadeColor = blackFade.color;
                fadeColor.a += (Time.deltaTime / fadeDuration);
                blackFade.color = fadeColor;
            }
        }
    }

}
