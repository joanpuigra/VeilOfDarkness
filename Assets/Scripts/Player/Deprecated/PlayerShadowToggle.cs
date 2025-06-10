using UnityEngine;

public class PlayerShadowToggle : MonoBehaviour
{
    [SerializeField] private Camera shadowCamera;
    private bool isActive;

    public void Toggle(float value)
    {
        bool newState = value > 0.5f;
        if (newState == isActive) return;
        
        isActive = newState;
        shadowCamera.gameObject.SetActive(isActive);
        Debug.Log(isActive ? "Shadow on!" : "Shadow off!");
    }
}
