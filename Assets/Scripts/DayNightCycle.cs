using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    [SerializeField] private float dayDurationInSeconds = 60f;

    private float rotationSpeed;

    private void Start()
    {
        if (!directionalLight)
        {
            enabled = false;
            return;
        }
 
        rotationSpeed = 360f / dayDurationInSeconds;
    }

    private void Update()
    {
        directionalLight.transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }
}