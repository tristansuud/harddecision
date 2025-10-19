using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParticleEvents
{
    public class ParticleOnFireEvent : EventData
    {
        Transform target;
        GameObject parent;
        public ParticleOnFireEvent(Transform t, GameObject parent)
        {
            this.target = t;
            this.parent = parent;
        }
    }
    
}
