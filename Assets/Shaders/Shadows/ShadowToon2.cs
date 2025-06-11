using System;
using UnityEngine;

public class ShadowToon : MonoBehaviour
{
    [Header("ToonShader Settings")]
    [SerializeField] private Material toonMaterial;
    [SerializeField] private Texture rampDay;
    [SerializeField] private Texture rampNight;

    private void Start()
    {
        SetNightMode();
    }

    public void SetDayMode()
    {
        toonMaterial.SetFloat("_ShadowCutoff", 0.3f);
        toonMaterial.SetTexture("_RampTex", rampDay);
        toonMaterial.SetColor("_ShadowColor", Color.gray);
    }

    public void SetNightMode()
    {
        toonMaterial.SetFloat("_ShadowCutoff", 0.85f);
        toonMaterial.SetTexture("_RampTex", rampNight);
        toonMaterial.SetColor("_ShadowColor", Color.blue);
    }

    public void ToggleRim(bool enabled)
    {
        toonMaterial.SetColor("_RimColor", enabled ? Color.white : Color.black);
    }
}