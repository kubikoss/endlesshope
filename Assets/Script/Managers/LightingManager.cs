using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    public static LightingManager Instance { get; private set; }

    [SerializeField]
    private Light DirectionalLight;
    [SerializeField]
    private LightingPreset Preset;
    [SerializeField, Range(0, 900)]
    private float TimeOfDay;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        TimeOfDay = 550f;
    }

    private void Update()
    {
        if (Preset == null)
        {
            return;
        }

        if(Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 900;
            UpdateLighting(TimeOfDay / 900f);
        }
        else
        {
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 900;
            UpdateLighting(TimeOfDay / 900f);
        }
    }

    public void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalLight.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, -170f, 0));
        }
    }

    public void ChangeLight()
    {
        UpdateLighting(TimeOfDay += 280f);
    }

    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;

        if (RenderSettings.sun != null)
            DirectionalLight = RenderSettings.sun;
        else
        {
            Light[] lights = GameObject.FindObjectsByType<Light>(FindObjectsSortMode.None);
            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}