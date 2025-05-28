using UnityEngine;

public class ShadowDetection : MonoBehaviour
{
    private static readonly int ShadowValue = Shader.PropertyToID("ShadowValue");
    private static readonly int BaseColor = Shader.PropertyToID("BaseColor");
    private Material _shadowMaterial;
    private float _shadowValue;

    [SerializeField] private GameObject player;
    private Material _playerMaterial;
    
    private void Start()
    {
        _shadowMaterial = GetComponent<Renderer>().material;
        _playerMaterial = player.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        _shadowValue = _shadowMaterial.GetFloat(ShadowValue);

        if (_shadowValue < 0.5f) { OnEnterShadow(); }
        else { OnExitShadow(); }
    }

    private void OnEnterShadow()
    {
        _playerMaterial.SetColor(BaseColor, Color.deepSkyBlue);
        _playerMaterial.SetFloat("Transparency", 0.5f);
        Debug.Log("Player on Shadow");
    }
    
    private void OnExitShadow()
    {
        _playerMaterial.SetColor(BaseColor, Color.gold);
        _playerMaterial.SetFloat("Transparency", 1.0f);
        Debug.Log("Player on light");
    }
}
