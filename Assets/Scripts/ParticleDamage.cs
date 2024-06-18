using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    [SerializeField] int particleDamage;
    [SerializeField] ParticleSystem particles;



    private void OnParticleCollision(GameObject other)
    {
        IDamage damage = other.gameObject.GetComponent<IDamage>();
        if (damage != null)
        {
            damage.takeDamage(particleDamage);
            Destroy(this);
        }
    }
}
