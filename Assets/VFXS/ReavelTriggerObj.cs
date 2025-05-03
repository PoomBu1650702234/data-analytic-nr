using UnityEngine;
using System.Collections;
public class ReavelTriggerObj : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IHiddenObject hiddenObj;
        bool isFound = collision.gameObject.TryGetComponent<IHiddenObject>(out hiddenObj);
        if (isFound)
        {
            hiddenObj.MakeItDisslove(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IHiddenObject hiddenObj;
        bool isFound = collision.gameObject.TryGetComponent<IHiddenObject>(out hiddenObj);
        if (isFound)
        {
            hiddenObj.MakeItDisslove(true);
        }
    }
}
    
