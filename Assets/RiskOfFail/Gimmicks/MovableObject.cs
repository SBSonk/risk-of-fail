using System.Collections;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    [SerializeField] private bool canBePushed;

    private RigidbodyConstraints2D defaultConstraints;
    private Rigidbody2D rb;
    private Vector2 startPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        defaultConstraints = rb.constraints;
        startPos = transform.position;
    }

    private void Update()
    {
        if (!canBePushed)
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        else
            rb.constraints = defaultConstraints;
    }

    public void SetPush(bool val)
    {
        canBePushed = val;
    }

    public void ReturnToStart(float t)
    {
        StartCoroutine(LerpToPosition(startPos, t));
    }

    private IEnumerator LerpToPosition(Vector2 pos, float t)
    {
        var startPos = transform.position;
        float time = 0;
        while (time < t)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime;

            transform.position = Vector2.Lerp(startPos, pos, time / t);
        }

        transform.position = pos;
    }
}