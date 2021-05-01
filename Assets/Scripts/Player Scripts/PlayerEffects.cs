using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerEffects : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float aberrationReleaseTime = 1.0f;
    [SerializeField] private float aberrationChargeSpeed = 20.0f;
    private PostProcessVolume volume;
    private ChromaticAberration aberration;

    private float intensity = 0.0f;
    private bool releaseChromatic = false;
    private bool chargeChromatic = false;
    // Start is called before the first frame update
    void Start()
    {
        volume = mainCamera.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings<ChromaticAberration>(out aberration);
    }

    // Update is called once per frame
    void Update()
    {
        intensity = Mathf.Clamp(intensity, 0, 1);
        if (Input.GetKey(KeyCode.Space) && intensity != 1)
        {
            intensity += Time.deltaTime * aberrationChargeSpeed;
            aberration.intensity.value = intensity;
        }
        else if(intensity != 0)
        {
            intensity -= Time.deltaTime / aberrationReleaseTime;
            aberration.intensity.value = intensity;
        }
    }
}
