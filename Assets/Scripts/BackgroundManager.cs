using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Color backgroundColor;
    [SerializeField] Color particleColor;
    [SerializeField] ParticleSystem backgroundParticles;

    void Start()
    {
        Camera.main.backgroundColor = backgroundColor;
        var newShape = backgroundParticles.shape;
        //newShape.scale = new Vector3(Screen.width, Screen.width, 1f);
        var newMain = backgroundParticles.main;
        newMain.startColor = particleColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
