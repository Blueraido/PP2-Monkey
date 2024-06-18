using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    [SerializeField] int AOEDamage;
    [SerializeField] SphereCollider AOECollider;

    private void OnCollisionEnter(Collision collision)
    {
        IDamage damage = collision.gameObject.GetComponent<IDamage>();
        if (damage != null)
        {
            damage.takeDamage(AOEDamage);
            Destroy(this);
        }
    }

}
