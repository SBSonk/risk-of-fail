using UnityEngine;

public class CameraFunctions : MonoBehaviour
{
    public static CameraFunctions main;

    [SerializeField] private Vector3 defaultOffset;
    [SerializeField] private float lerp = 0.5f;
    private Vector3 offset;

    private void Awake()
    {
        if (!main) main = this;
    }

    private void FixedUpdate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, offset + new Vector3(0f, 0f, -10f), lerp);
    }
}