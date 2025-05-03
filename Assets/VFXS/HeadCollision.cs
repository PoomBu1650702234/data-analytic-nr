using UnityEngine;

public class HeadCollision : MonoBehaviour
{
    private Collider2D col2d;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col2d = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IItemDropAble itemDropObj;
        bool isFound = collision.gameObject.TryGetComponent<IItemDropAble>(out itemDropObj);
        if (isFound)
        {
            itemDropObj.Drop();
        }
    }

    

}
