using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject _layer_1;
    [SerializeField] private float _speed_layer_1 = 0.1f;
    [Space]
    [SerializeField] private GameObject _layer_2;
    [SerializeField] private float _speed_layer_2 = 0.2f;
    [Space]
    [SerializeField] private GameObject _layer_3;
    [SerializeField] private float _speed_layer_3 = 0.3f;

    private void Update()
    {
        OnMoveBackground(_layer_1, _speed_layer_1);
        OnMoveBackground(_layer_2, _speed_layer_2);
        OnMoveBackground(_layer_3, _speed_layer_3);
    }

    private void OnMoveBackground(GameObject gameObject, float _speed)
    {
        //if (gameObject.transform.position.x >= -19f)
        //{
            gameObject.transform.position -= (new Vector3(_speed * Time.deltaTime, 0, 0));
       // }
        /*else
        {
            gameObject.transform.position += Vector3.right * 38;
        }*/
    }
}
