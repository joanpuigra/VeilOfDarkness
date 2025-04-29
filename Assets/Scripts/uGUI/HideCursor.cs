using UnityEngine;

public class HideCursor : MonoBehaviour
{
    private void Start() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnApplicationFocus(bool focusStatus)
    {
        if (focusStatus) return;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
