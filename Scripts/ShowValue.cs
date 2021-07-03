using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowValue : MonoBehaviour
{
    Text percentText;

    void Start()
    {
        percentText = GetComponent<Text>();
    }

    
    public void UpdateText (float value)
    {
        percentText.text = Mathf.RoundToInt(value * 100) + "%";
    }
}
