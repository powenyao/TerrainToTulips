using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;

public class EffectManager : MonoBehaviour
{
    public Planet Earth;
    public ShapeSettings Shape_Earth;
    public ColorSettings Color_Earth;
    public ShapeSettings Shape_Default;
    public ColorSettings Color_Default;
    public ShapeSettings Shape_EarthCurrent;
    public ColorSettings Color_EarthCurrent;

    public CinemachineVirtualCamera vc;

    public bool transitionToNextScene = true;
    private float timeElapsed = 0;
    public float lerpDuration = 1;
    public float waitAfterLerp = 2;

    public float initialDelayBeforeTransforming = 1f;
    // Start is called before the first frame update
    void Start()
    {
        Shape_EarthCurrent.CopyShapeSettings(Shape_Default);
    }

    // Update is called once per frame
    void Update()
    {
        if (initialDelayBeforeTransforming > 0)
        {
            initialDelayBeforeTransforming -= Time.deltaTime;
            return;
        }
        
        if (timeElapsed < lerpDuration)
        {
            var t = timeElapsed / lerpDuration;
            Shape_EarthCurrent.planetRadius = Mathf.Lerp(Shape_Default.planetRadius, Shape_Earth.planetRadius, t);
            Earth.resolution = (int)Mathf.Lerp(2, 100, t);

            for (int i = 0; i < Shape_EarthCurrent.noiseLayers.Length; i++)
            {
                if (i == 0)
                {
                    Shape_EarthCurrent.noiseLayers[i].noiseSettings.simpleNoiseSettings.strength = Mathf.Lerp(
                        Shape_Default.noiseLayers[i].noiseSettings.simpleNoiseSettings.strength,
                        Shape_Earth.noiseLayers[i].noiseSettings.simpleNoiseSettings.strength, t);
                    Shape_EarthCurrent.noiseLayers[i].noiseSettings.simpleNoiseSettings.baseRoughness = Mathf.Lerp(
                        Shape_Default.noiseLayers[i].noiseSettings.simpleNoiseSettings.baseRoughness,
                        Shape_Earth.noiseLayers[i].noiseSettings.simpleNoiseSettings.baseRoughness, t);
                    Shape_EarthCurrent.noiseLayers[i].noiseSettings.simpleNoiseSettings.roughness = Mathf.Lerp(
                        Shape_Default.noiseLayers[i].noiseSettings.simpleNoiseSettings.roughness,
                        Shape_Earth.noiseLayers[i].noiseSettings.simpleNoiseSettings.roughness, t);
                    Shape_EarthCurrent.noiseLayers[i].noiseSettings.simpleNoiseSettings.persistence = Mathf.Lerp(
                        Shape_Default.noiseLayers[i].noiseSettings.simpleNoiseSettings.persistence,
                        Shape_Earth.noiseLayers[i].noiseSettings.simpleNoiseSettings.persistence, t);
                    Shape_EarthCurrent.noiseLayers[i].noiseSettings.simpleNoiseSettings.minValue = Mathf.Lerp(
                        Shape_Default.noiseLayers[i].noiseSettings.simpleNoiseSettings.minValue,
                        Shape_Earth.noiseLayers[i].noiseSettings.simpleNoiseSettings.minValue, t);
                }
                else
                {
                    Shape_EarthCurrent.noiseLayers[i].noiseSettings.rigidNoiseSettings.strength = Mathf.Lerp(
                        Shape_Default.noiseLayers[i].noiseSettings.rigidNoiseSettings.strength,
                        Shape_Earth.noiseLayers[i].noiseSettings.rigidNoiseSettings.strength, t);
                    Shape_EarthCurrent.noiseLayers[i].noiseSettings.rigidNoiseSettings.baseRoughness = Mathf.Lerp(
                        Shape_Default.noiseLayers[i].noiseSettings.rigidNoiseSettings.baseRoughness,
                        Shape_Earth.noiseLayers[i].noiseSettings.rigidNoiseSettings.baseRoughness, t);
                    Shape_EarthCurrent.noiseLayers[i].noiseSettings.rigidNoiseSettings.roughness = Mathf.Lerp(
                        Shape_Default.noiseLayers[i].noiseSettings.rigidNoiseSettings.roughness,
                        Shape_Earth.noiseLayers[i].noiseSettings.rigidNoiseSettings.roughness, t);
                    Shape_EarthCurrent.noiseLayers[i].noiseSettings.rigidNoiseSettings.persistence = Mathf.Lerp(
                        Shape_Default.noiseLayers[i].noiseSettings.rigidNoiseSettings.persistence,
                        Shape_Earth.noiseLayers[i].noiseSettings.rigidNoiseSettings.persistence, t);
                    Shape_EarthCurrent.noiseLayers[i].noiseSettings.rigidNoiseSettings.minValue = Mathf.Lerp(
                        Shape_Default.noiseLayers[i].noiseSettings.rigidNoiseSettings.minValue,
                        Shape_Earth.noiseLayers[i].noiseSettings.rigidNoiseSettings.minValue, t);
                    Shape_EarthCurrent.noiseLayers[i].noiseSettings.rigidNoiseSettings.weightMultiplier = Mathf.Lerp(
                        Shape_Default.noiseLayers[i].noiseSettings.rigidNoiseSettings.weightMultiplier,
                        Shape_Earth.noiseLayers[i].noiseSettings.rigidNoiseSettings.weightMultiplier, t);
                }
            }

            Earth.OnShapeSettingsUpdated();
            timeElapsed += Time.deltaTime;
        }
        else
        {
            //Trigger Particle EFfects & Fade Out
            
            timeElapsed += Time.deltaTime;
            if (transitionToNextScene && timeElapsed > lerpDuration+waitAfterLerp)
            {
                SceneManager.LoadScene("TestScene_City");
            }
        }
    }
}