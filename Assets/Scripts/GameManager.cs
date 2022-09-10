using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Debug --------------------
    [SerializeField]
    private TextMeshProUGUI tiltInputText;
    // -------------------------

    void Start()
    {
        
    }

    void Update()
    {
        // Accelerometer
        // Input.acceleration.x

       // tiltInputText.text = Input.acceleration.x.ToString();
    }
}
