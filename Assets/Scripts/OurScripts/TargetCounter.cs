using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetCounter : MonoBehaviour
{
    public static TargetCounter instance;
    public TMP_Text counterText;
    public int currentTargets = 0;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        counterText.text = "\n" + currentTargets.ToString();
    }
    
    public void AddTargetHit()
    {
        currentTargets++;
        counterText.text = "\n" + currentTargets.ToString();
    }
}
