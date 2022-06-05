using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SunLight : MonoBehaviour
{
    public GameObject sun;
    public Color color {get; private set;}

    void Awake() {
        GetSunColors();
    }

    void Start()
    {
        UpdateColors();
    }

    public void UpdateColors() {
        UpdateSceneColor();
        transform.GetComponent<Light>().color = color;
    }

    void GetSunColors() {
        if (sun) {
            Material sunMat = sun.GetComponent<Renderer>().sharedMaterial;
            if (sunMat) {
                color = AverageColors(new Color[] {sunMat.GetColor("_ValleyColor"), sunMat.GetColor("_MidColor"), sunMat.GetColor("_PeakColor")});
            }
        }
    }

    void UpdateSceneColor()
    {
        // Find new scene color based on all sun lights
        SunLight[] lights = GameObject.FindObjectsOfType<SunLight>();
        Color[] colors = new Color[lights.Length];
        for (int i = 0; i < lights.Length; i++) colors[i] = lights[i].color;
        Color newColor = AverageColors(colors);
        // Use the intensity
        newColor = AdoptIntensity(newColor, RenderSettings.ambientLight);
        RenderSettings.ambientLight = newColor;
    }

    Color AverageColors(Color[] colors) {
        Vector4 color = new Vector4(0,0,0,0);
        if (colors.Length > 0) {
            foreach(Color c in colors) color += new Vector4(c.r,c.g,c.b,c.a);
            color /= colors.Length;
        }
        return color;
    }

    Color AdoptIntensity(Color a, Color b) {
        float intensityA = (a.r + a.g + a.b) / 3f;
        float intensityB = (b.r + b.g + b.b) / 3f;
        a.r = a.r * intensityB / intensityA;
        a.g = a.g * intensityB / intensityA;
        a.b = a.b * intensityB / intensityA;
        return a;
    }
}
