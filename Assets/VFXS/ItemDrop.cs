using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDrop : MonoBehaviour,IItemDropAble
{
    private bool isAlreadyDrop = false;
    [SerializeField] bool useWithHideenObject;
    [SerializeField] private List<GameObject> dropPool;
    [SerializeField] Transform dropSpawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Drop()
    {
        if (useWithHideenObject == true) 
        {
            if (isAlreadyDrop == false)
            {
                if (gameObject.TryGetComponent<IHiddenObject>(out IHiddenObject hiddenObj))
                {
                    Hidden hidden = hiddenObj as Hidden;
                    if (hidden != null && hidden.Interactabe == true)
                    {
                        //isAlreadyDrop = true;
                        for (int i = 0; i < dropPool.Count; i++)
                        {
                            GameObject dropObj = Instantiate(dropPool[i]);
                            dropObj.transform.parent = dropSpawnPoint;
                            dropObj.transform.localPosition = Vector3.zero;
                        }
                    }
                }

            }
        }
        if (useWithHideenObject == false) 
        {
            if (isAlreadyDrop == false)
            {
                //isAlreadyDrop = true;
                for (int i = 0; i < dropPool.Count; i++)
                {
                    GameObject dropObj = Instantiate(dropPool[i]);
                    dropObj.transform.parent = dropSpawnPoint;
                    dropObj.transform.localPosition = Vector3.zero;
                }
            }
        }
        
    }
}
