using UnityEngine;

public class ScaleChanger : MonoBehaviour
{
    private void Start()
    {
        ChangeScale(new Vector3(1f, 1f, 1f));
    }
    
    private void ChangeScale(Vector3 newScale)
    {
        transform.localScale = newScale;
    }
}
