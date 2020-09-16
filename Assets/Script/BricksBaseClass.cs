using UnityEngine;

public class RaycastBaseClass : MonoBehaviour
{
    // 射线函数封装
    public RaycastHit2D Raycast(Vector2 offset, Vector2 rayDiraction, float length, LayerMask layer)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y + 0.05f);
        RaycastHit2D hit = Physics2D.Raycast(pos + offset, rayDiraction, length, layer);
        Color color = hit ? Color.red : Color.green;
        Debug.DrawRay(pos + offset, rayDiraction * length, color);
        return hit;
    }
}
