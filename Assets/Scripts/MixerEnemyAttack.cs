using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerEnemyAttack : MonoBehaviour
{
    [SerializeField] MixerEnemy mixerEnemy;

    [SerializeField] Collider fistLeft;
    [SerializeField] Collider fistRight;
    [SerializeField] Collider leg;
    // Start is called before the first frame update
    void Start()
    {
        if (mixerEnemy == null) 
            mixerEnemy = GetComponent<MixerEnemy>(); 
    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider other)
    {
        float damage = mixerEnemy.meleeDamage;

        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.gameObject.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(damage);
            }
        }
    }
}
