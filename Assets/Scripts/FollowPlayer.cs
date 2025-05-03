using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform targetObj;
    [SerializeField] private Vector2 offset;
    private void Update()
    {
        transform.position = new Vector3(targetObj.position.x + offset.x,0 + offset.y,0);
    }
}
