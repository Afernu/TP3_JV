using UnityEngine;

public class PickupAnimation : MonoBehaviour
{
    public float rotationSpeedX = 50f;
    public float rotationSpeedY = 30f;
    public float rotationSpeedZ = 40f;
    public float pulseSpeed = 2f;
    public float pulseMagnitude = 0.1f;
    public float floatSpeed = 0.5f;
    public float floatMagnitude = 0.5f;

    private Vector3 baseScale;
    private Vector3 basePosition;

    void Start()
    {
        baseScale = transform.localScale;
        basePosition = transform.position;
    }

    void Update()
    {
        Spin();
        Pulse();
        Float();
    }

    void Spin()
    {
        transform.Rotate(Vector3.right, rotationSpeedX * Time.deltaTime);
        transform.Rotate(Vector3.up, rotationSpeedY * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotationSpeedZ * Time.deltaTime);
    }

    void Pulse()
    {
        float scale = Mathf.Sin(Time.time * pulseSpeed) * pulseMagnitude + 1;
        transform.localScale = baseScale * scale;
    }

    void Float()
    {
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatMagnitude;
        transform.position = basePosition + new Vector3(0, newY, 0);
    }
}
