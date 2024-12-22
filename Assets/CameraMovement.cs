using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 initialCameraPos;
    [SerializeField] private float moveDuration = 2f;
    [SerializeField] private float targetSize = 16.79f;
    //private Vector3 a = new Vector3(-19, 9.5f, 1.3f);

    private Vector3 targetPos = new Vector3(0f, 9.5f, -10f);
    private Camera cam;
    //ortogrophic size

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        transform.DOMove(targetPos,moveDuration);
        cam.DOOrthoSize(targetSize, moveDuration);
    }
}
