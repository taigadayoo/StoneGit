using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogStartTime : MonoBehaviour
{
    public ParticleSystem myParticleSystem;
    public float startTime = 2.0f;

    void Start()
    {
        if (myParticleSystem != null)
        {
            myParticleSystem.Simulate(startTime, true, true);
            myParticleSystem.Play();
        }
    }
}