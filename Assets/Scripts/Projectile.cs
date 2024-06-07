using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody body;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;


    // Start is called before the first frame update
    void Start()
    {
        body.velocity = transform.forward * speed;
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.takeDamage(damage);
        }

        Destroy(gameObject);
    }
}
