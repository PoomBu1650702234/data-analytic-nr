using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class WorldToColorEffect : MonoBehaviour
{
    [SerializeField] private Transform spriteMaskScale;
    [SerializeField] private GameObject distortionScale;
    [SerializeField] private bool activate;
    [SerializeField] private Vector3 startSize;
    [SerializeField] private Vector3 endSize;
    [SerializeField] private float effectTime;
    [SerializeField] private Light2D globalLight;
    [SerializeField] private bool reset = false;

    private AudioSource sound;
    private void Start()
    {
        sound = GetComponent<AudioSource>();
    }
    private void Update()
    {
        
        if (activate) 
        {
            activate = false;
            print("Activate");
            StartCoroutine(SizeEffect(startSize,endSize, effectTime));
            if (sound != null) 
            {
                sound.Play();
            }
        }

        if (reset) 
        {
            print("Reset");
            StartCoroutine(SizeEffect(endSize, startSize, effectTime));
            reset = false;
        }
    }

    private IEnumerator SizeEffect(Vector3 start, Vector3 end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float t2 = Mathf.Clamp01(elapsed / (duration/2));

            spriteMaskScale.localScale = Vector3.Lerp(start, end + new Vector3(0.3f, 0.3f, 0), t);
            distortionScale.transform.localScale = Vector3.Lerp(start, end, t);
            distortionScale.GetComponent<SpriteRenderer>().material.SetFloat("_Refraction", Mathf.Lerp(0.07f, 0, t2));
            yield return null;
        }

    }

    public void SpawnWithSetSizeAndTime(Vector3 _endSize,float _time) 
    {
        endSize = _endSize;
        effectTime = _time;
        activate = true;
    }
    //[ContextMenu("WorldEffect Activate")]
    public void WorldEffectActivate()
    {
        activate = true;
        if (globalLight != null) 
        {
            StartCoroutine(LightIntensityEffect(effectTime));
        }
    }
    
    private IEnumerator LightIntensityEffect(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            globalLight.intensity = math.lerp(0f,1f, t);
            yield return null;
        }

    }
}
