using UnityEngine;

public class URPShadowDetector : MonoBehaviour
{
    [SerializeField] private Transform directionalLight;
    [SerializeField] private Renderer characterRenderer;
    
    private MaterialPropertyBlock propertyBlock;
    private static readonly int ShadowStrengthId = Shader.PropertyToID("_MainLightShadow");
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private void Awake()
    {
        propertyBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        if (!characterRenderer) return;
        
        float shadowStrength = CalculateShadowStrength();
        
        bool isInShadow = shadowStrength > 0.9f;
        
        characterRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(BaseColor, isInShadow ? Color.red : Color.green);
        characterRenderer.SetPropertyBlock(propertyBlock);
    }

    private static float CalculateShadowStrength()
    {
        return 1.0f;
    }
}
