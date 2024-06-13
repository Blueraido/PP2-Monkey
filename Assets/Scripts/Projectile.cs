using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Rigidbody body;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] GameObject hitEffect;
    [SerializeField] float arc;

    // Start is called before the first frame update
    void Start()
    {
        if(arc == 0)
        {
        body.velocity = transform.forward * speed;
        }
        else
        {
            body.velocity = (transform.forward + new Vector3(0,arc,0)) * speed;
        }
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.takeDamage(damage);
        }
        hitFanfare();
        Destroy(gameObject);

    }

    void hitFanfare()
    {
        Instantiate(hitEffect, transform.position, transform.rotation);

    }

}
