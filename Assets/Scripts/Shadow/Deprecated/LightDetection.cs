using UnityEngine;

public class LightDetection : MonoBehaviour
{
   private static readonly int LightIntensity = Shader.PropertyToID("LightIntensity");
   private Material _shadowMaterial;
   private float _lightIntensity;

   private void Start()
   {
      _shadowMaterial = GetComponent<Renderer>().material;
   }

   private void Update()
   {
      _lightIntensity = CalculateLightIntensity();
      _shadowMaterial.SetFloat(LightIntensity, _lightIntensity);
   }

   private float CalculateLightIntensity()
   {
      Vector3 lightDirection = (Vector3.zero - transform.position).normalized;
      float intensity = Vector3.Dot(lightDirection, transform.up);
      intensity = Mathf.Clamp(intensity, 0f, 1f);
      return intensity;
   }
}