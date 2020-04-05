using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamSpin : MonoBehaviour
{
    [SerializeField] float rotSpeed = 5.0f;

    void Update()
    {
        transform.Rotate(0, rotSpeed * Time.deltaTime, 0);
    }
}
