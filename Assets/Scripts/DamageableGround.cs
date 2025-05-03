using UnityEngine;

public class DamageableGround : MonoBehaviour
{
    private Collider2D col2d;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col2d = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>()) 
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage();
        }
    }
}
