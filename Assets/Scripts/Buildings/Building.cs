using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector2Int _size = Vector2Int.one;
    public Renderer _mainRender;

    public void SetTransparent(bool available)
    {
        if (available)
            SetColor(Color.green);
        else
            SetColor(Color.red);
    }

    public void SetColor(Color color)
    {
        _mainRender.material.color = color;
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                if ((x+y)%2  == 0) Gizmos.color = new Color(0.88f, 0f, 1f, 0.3f);
                else Gizmos.color = new Color(1f, 0.69f, 1f, 0.3f);

                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, 0.01f, 1));
            }
        }
    }
}
