using UnityEngine;

public class DeadFeather : MonoBehaviour
{
    private Vector2 velocity = Vector2.zero;
    private float min = 10f;
    private float max = 20f;

    public void explode()
    {
        velocity = new Vector2(Random.value * (max - min) + min,Random.value * (max - min) + min);
        if (Random.value > 0.5f) velocity.x *= -1f;
        if (Random.value > 0.5f) velocity.y *= -1f;
        transform.Rotate(Vector3.back, Random.value * 360f);
        transform.localScale = Vector3.one;
        transform.localScale *= (0.5f + Random.value * 0.5f);
    }
    
    void Update()
    {
        transform.Translate(velocity * Time.deltaTime);
    }
}
