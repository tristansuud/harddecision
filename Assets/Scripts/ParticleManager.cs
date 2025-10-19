using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    // Particle Prefabs

    // Events
    // Start is called before the first frame update

    private void OnEnable()
    {
        EventBus.Subscribe<ParticleEvents.ParticleOnFireEvent>(FireParticle);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe<ParticleEvents.ParticleOnFireEvent>(FireParticle);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FireParticle(ParticleEvents.ParticleOnFireEvent evt)
    {

    }
}
