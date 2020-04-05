using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Display_GUIMessage : MonoBehaviour
{
    public float hideTime = 3.5f;
    [SerializeField] TextMeshProUGUI messageDisplay;
    public void DisplayWarning(string text)
    {
        messageDisplay.enabled = true;
        messageDisplay.text = text;
        Invoke("HideWarning", hideTime);
    }

    public void HideWarning()
    {
        messageDisplay.enabled = false;
    }
}
