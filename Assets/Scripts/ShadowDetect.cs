using UnityEngine;

public class ShadowDetect : MonoBehaviour
{
    private void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (!Physics.Raycast(ray, out var hit)) return;
        Vector2 uv = hit.textureCoord;
        Texture2D shadowTexture = hit.collider.GetComponent<Renderer>().material.mainTexture as Texture2D;

        Color pixelColor = shadowTexture!.GetPixelBilinear(uv.x, uv.y);

        Debug.Log(pixelColor.grayscale > 0.5f ? "Shadow detected!" : "No shadow detected.");
    }
}
