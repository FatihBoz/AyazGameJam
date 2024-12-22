using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndingManager : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;


    private readonly string allyTag = "AllyCastle";

    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    private void OnLordDestroyed(Lord lord)
    {
        if (lord.CompareTag(allyTag))
        {
            losePanel.SetActive(true);
        }
        else
        {
            winPanel.SetActive(true);
        }
    }

    private void OnEnable()
    {
        Lord.OnLordDestroyed += OnLordDestroyed;
    }

    private void OnDisable()
    {
        Lord.OnLordDestroyed -= OnLordDestroyed;
    }

}
