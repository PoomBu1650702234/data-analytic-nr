using NUnit;
using Unity.VisualScripting;
using UnityEngine;

public class PrototypeRobotAI : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] float followRadius;
    [SerializeField] float moveSpeed;

    private float dist;
    private void Update()
    {
        if (Target == null) 
        {
            return;
        }
        dist = Vector3.Distance(transform.position, Target.position);
        
        if (dist >= followRadius)
        {
            // Smoothly interpolate towards the target
            transform.position = Vector3.Lerp(transform.position, Target.position, moveSpeed * Time.deltaTime);
            // Flip the sprite based on movement direction
            if (transform.position.x < Target.position.x)
                transform.localScale = new Vector3(1, 1, 1); // Facing right
            else
                transform.localScale = new Vector3(-1, 1, 1); // Facing left
        }
    }
}
