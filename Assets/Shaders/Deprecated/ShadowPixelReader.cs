using UnityEngine;

public class ShadowPixelReader : MonoBehaviour
{
    [SerializeField] private Camera shadowCamera;
    [SerializeField] private RenderTexture shadowTexture;
    [SerializeField] private GameObject player;
    
    // private Texture2D _readerTexture;
    // private Rect _readRect;
    private Texture2D _pixelReader;

    private void Start()
    {
        _pixelReader = new Texture2D(1, 1, TextureFormat.RGB24, false);
        // _readerTexture = new Texture2D(shadowTexture.width, shadowTexture.height, TextureFormat.RGBA32, false);
        // _readRect = new Rect(0, 0, shadowTexture.width, shadowTexture.height);
    }

    private void Update()
    {
        // Check player position to pixel in shadow texture
        Vector3 worldPos = transform.position;
        Vector3 viewportPos = shadowCamera.WorldToViewportPoint(worldPos);
        
        int px = Mathf.Clamp((int)(viewportPos.x * shadowTexture.width), 0, shadowTexture.width - 1);
        int py = Mathf.Clamp((int)(viewportPos.y * shadowTexture.height), 0, shadowTexture.height - 1);
        
        // Read pixels from the shadow texture
        // int px = (int)(viewportPos.x * shadowTexture.width);
        // int py = (int)(viewportPos.y * shadowTexture.height);
        
        // Read the pixel color
        RenderTexture.active = shadowTexture;
        _pixelReader.ReadPixels(new Rect(px, py, 1, 1), 0, 0);
        _pixelReader.Apply();
        // _readerTexture.ReadPixels(_readRect, 0, 0);
        // _readerTexture.Apply();
        RenderTexture.active = null;
        
        // Get the pixel color
        Color pixelColor = _pixelReader.GetPixel(0, 0);
        // Color pixelColor = _readerTexture.GetPixel(px, py);
        float shadowAmount = pixelColor.g;
        
        Debug.Log($"Shadow: {shadowAmount}");
        if (shadowAmount < 0.5f) OnShadowEnter(); else OnShadowExit();
    }

    private void OnShadowEnter()
    {
        Material playerMaterial = player.GetComponentInChildren<Renderer>().material;
        playerMaterial.color = Color.red;
        Debug.Log("Player on shadow");
        // _playerMaterial.SetColor(BaseColor, Color.deepSkyBlue);
        // _playerMaterial.SetFloat(Transparency, 0.5f);
    }

    private void OnShadowExit()
    {
        Material playerMaterial = player.GetComponentInChildren<Renderer>().material;
        playerMaterial.color = Color.green;
        Debug.Log("Player on light");
    }
}
