using UnityEngine;

public class Building : MonoBehaviour
{
    public Vector2Int Size = Vector2Int.one;
    public Renderer MainRender;

    public void SetTransparent(bool available)
    {
        if(available)
            MainRender.material.color = Color.green;
        else
            MainRender.material.color = Color.red;
    }

    public void SetNormal()
    {
        MainRender.material.color = Color.white;
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                if ((x+y)%2  == 0) Gizmos.color = new Color(0.88f, 0f, 1f, 0.3f);
                else Gizmos.color = new Color(1f, 0.69f, 1f, 0.3f);

                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, 0.01f, 1));
            }
        }
    }
}
