using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkerLight : MonoBehaviour
{
    public Light mainlight;
    public float lightintensityInForest = 0.75f;
    private float oldintensity;

    private void Start() {
        oldintensity = mainlight.intensity;
    }
    private void OnTriggerEnter(Collider other) {
        mainlight.intensity = lightintensityInForest;
    }
    private void OnTriggerExit(Collider other) {
        mainlight.intensity = oldintensity;
    }
}
