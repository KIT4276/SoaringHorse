using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingOrigin : MonoBehaviour
{
    [SerializeField] private Transform reference; 
    [SerializeField] private float threshold = 20000f;

    private void LateUpdate()
    {
        if (!reference) return;

        float x = reference.position.x;
        if (Mathf.Abs(x) < threshold) return;

        Vector3 offset = new Vector3(x, 0f, 0f);

        var roots = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0; i < roots.Length; i++)
        {
            roots[i].transform.position -= offset;
        }
    }
}
