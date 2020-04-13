using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LT_SplashScreenPrompt : MonoBehaviour
{
    [SerializeField] Vector3 scaleToSize;
    [SerializeField] float timeToScale;
    private void OnEnable()
    {
        LeanTween.scale(this.gameObject, scaleToSize, timeToScale).setLoopPingPong();
    }
}
