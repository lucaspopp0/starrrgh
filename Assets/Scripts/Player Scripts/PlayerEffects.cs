using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    private PostProcessVolume volume;
    // Start is called before the first frame update
    void Start()
    {
        volume = mainCamera.GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void boostEffect()
    {
        StartCoroutine(chromaticAberration());
    }

    IEnumerator chromaticAberration()
    {
        ChromaticAberration aberration;
        volume.profile.TryGetSettings<ChromaticAberration>(out aberration);
        float intensity = 1;
        aberration.intensity.Override(intensity);

        yield return new WaitForSeconds(1);

        aberration.intensity.Override(0);
    }
}
