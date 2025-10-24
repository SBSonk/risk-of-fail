using UnityEngine;

public class FakeWall : MonoBehaviour
{
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    public void OnEnable()
    {
        //AstarPath.active.UpdateGraphs(col.bounds, 0.5f);
    }

    public void OnDisable()
    {
        AstarPath.active.UpdateGraphs(col.bounds, 0.5f);
    }
}