using UnityEngine;

public class TPlayer : MonoBehaviour
{
    public static TPlayer instance;
    private void Awake()
    {
        instance = this;
    }
}
