using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject colorFilterEffect;
    public void TakeDamage()
    {
        GameObject instantObj = Instantiate(colorFilterEffect);
        
        instantObj.transform.position = transform.position;
        instantObj.GetComponent<WorldToColorEffect>().SpawnWithSetSizeAndTime(new Vector3(1, 1, 0), 2f);
        Destroy(gameObject); // ทำลายศัตรูเมื่อถูกโจมตี
    }
}
