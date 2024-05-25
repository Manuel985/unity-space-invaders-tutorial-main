using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;

    [SerializeField]
    private float speed;

    private void Update()
    {
        transform.position += speed * Time.deltaTime * direction;
    }
}
