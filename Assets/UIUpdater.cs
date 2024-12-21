using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIUpdater : MonoBehaviour
{
    public static UIUpdater instance;

    public GameObject playerCastle;
    public TextMeshProUGUI SourceText;

    private void Awake()
    {
        instance = this;
    }

    public void UpdateSource()
    {
        SourceText.text = playerCastle.GetComponent<Lord>().gold.ToString();
    }

    private void Start()
    {
        UpdateSource();
    }
}
