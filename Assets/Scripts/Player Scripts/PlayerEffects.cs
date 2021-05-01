using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float aberrationTime = 1.0f;
    private PostProcessVolume volume;
    private ChromaticAberration aberration;

    private float intensity = 1.0f;
    private bool chromaticEffect = false;
    // Start is called before the first frame update
    void Start()
    {
        volume = mainCamera.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings<ChromaticAberration>(out aberration);
    }

    // Update is called once per frame
    void Update()
    {
        if (chromaticEffect && aberration != null)
        {
            intensity -= Time.deltaTime / aberrationTime;
            aberration.intensity.value = intensity;
            if(intensity <= 0)
            {
                chromaticEffect = false;
            }
        }
        else if (!chromaticEffect)
        {
            intensity = 1.0f;
        }
    }

    public void boostEffect()
    {
        chromaticEffect = true;
        intensity = 1.0f;
    }
}
