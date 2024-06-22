using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamage : MonoBehaviour
{
    [SerializeField] int AOEDamage;


    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage damage = other.gameObject.GetComponent<IDamage>();
        if (damage != null)
        {
            damage.takeDamage(AOEDamage);
            Destroy(this);
        }
    }
}
