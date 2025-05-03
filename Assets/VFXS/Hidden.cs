using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Hidden : MonoBehaviour,IHiddenObject
{
    [SerializeField]private bool interactable = false;
    [SerializeField]private GameObject lightObj;
    public bool Interactabe { get { return interactable; } }
    private SpriteRenderer spRen;
    [SerializeField] private float lerpDuration = 0.7f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spRen = GetComponent<SpriteRenderer>();
        spRen.material.SetFloat("_DissloveScale", 1);
        interactable = false;
        if (lightObj != null) 
        {
            lightObj.SetActive(false);
        }
    }

    public void MakeItDisslove(bool isDisslove)
    {
        if (isDisslove == true) 
        {
            StartCoroutine(DissolveEffect(0,1,lerpDuration));
            interactable = false;
            if (lightObj != null)
            {
                lightObj.SetActive(false);
            }
        }
        if (isDisslove == false)
        {
            StartCoroutine(DissolveEffect(1, 0, lerpDuration));
            interactable = true;
            if (lightObj != null)
            {
                lightObj.SetActive(true);
            }
        }

    }

    private IEnumerator DissolveEffect(float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            spRen.material.SetFloat("_DissloveScale",Mathf.Lerp(start, end, t));
            yield return null;
        }

        spRen.material.SetFloat("_DissloveScale", end);
    }
}

