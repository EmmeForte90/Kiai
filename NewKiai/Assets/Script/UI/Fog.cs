using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Fog : MonoBehaviour
{
    public Color fogColor;
    public float fogDensity;
    public Image fogImage;
    public bool fogEnabled;

    // Image distortion effect
    public bool useDistortion;
    public float distortionAmount;
    public Material distortionMaterial;

    // Noise effect
    public bool useNoise;
    public float noiseAmount;
    public Material noiseMaterial;

    private void Update()
    {
        if (fogEnabled)
        {
            fogImage.color = fogColor;
            fogImage.fillAmount = fogDensity;
            fogImage.enabled = true;

            // Apply distortion effect if enabled
            if (useDistortion)
            {
                fogImage.material = distortionMaterial;
                distortionMaterial.SetFloat("_Amount", distortionAmount);
            }

            // Apply noise effect if enabled
            if (useNoise)
            {
                fogImage.material = noiseMaterial;
                noiseMaterial.SetFloat("_Amount", noiseAmount);
            }
        }
        else
        {
            fogImage.enabled = false;
        }
    }
}