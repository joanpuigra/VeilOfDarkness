using UnityEngine;

public class ShadowDetection : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject map;
    
    private static readonly int ShadowValue = Shader.PropertyToID("ShadowValue");
    private static readonly int BaseColor = Shader.PropertyToID("BaseColor");
    private static readonly int Transparency = Shader.PropertyToID("Transparency");

    private Material _shadowMaterial;
    private Material _playerMaterial;
    
    private float _shadowValue;
    
    private void Start()
    {
        _shadowMaterial = map.GetComponentInChildren<Renderer>().material;
        _playerMaterial = player.GetComponent<Renderer>().material;
    }

    private void Update()
    {
        _shadowValue = _shadowMaterial.GetFloat(ShadowValue);
        (_shadowValue < 0.5f ? (System.Action)OnEnterShadow : OnExitShadow)();
    }

    private void OnEnterShadow()
    {
        // _playerMaterial.SetColor(BaseColor, Color.deepSkyBlue);
        _playerMaterial.SetFloat(Transparency, 0.5f);
    }
    
    private void OnExitShadow()
    {
        // _playerMaterial.SetColor(BaseColor, Color.gold);
        _playerMaterial.SetFloat(Transparency, 1.0f);
        Debug.Log("Player on light");
    }
}
