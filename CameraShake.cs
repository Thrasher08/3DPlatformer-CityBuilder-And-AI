using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{

    public CinemachineFreeLook freeLookCam;
    CinemachineImpulseSource camShake;

    float baseAmplitude;
    float totalTime;

    // Start is called before the first frame update
    void Start()
    {
        camShake = freeLookCam.GetComponent<CinemachineImpulseSource>();
        baseAmplitude = camShake.m_ImpulseDefinition.m_AmplitudeGain;
        totalTime = camShake.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime + camShake.m_ImpulseDefinition.m_TimeEnvelope.m_DecayTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Shake(float amplitude)
    {
        camShake.m_ImpulseDefinition.m_AmplitudeGain = amplitude;
        camShake.GenerateImpulse();
        yield return new WaitForSeconds(totalTime);
        camShake.m_ImpulseDefinition.m_AmplitudeGain = baseAmplitude;
        yield return new WaitForSeconds(1);
    }
}
