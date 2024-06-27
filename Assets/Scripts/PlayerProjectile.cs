using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] Rigidbody body;

    public GameObject Weapmodel;


    public static Projectile instance;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.playerScript.arc == 0)
        {
            body.velocity = transform.forward  * GameManager.instance.playerScript.GetSelectedWeapon().ProjectileSpeed;
        }
        else
        {
            body.velocity = (transform.forward + new Vector3(0, GameManager.instance.playerScript.arc, 0)) * GameManager.instance.playerScript.GetSelectedWeapon().ProjectileSpeed;
        }
        Destroy(gameObject, GameManager.instance.playerScript.destroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.takeDamage(GameManager.instance.playerScript.shootDamage * GameManager.instance.playerScript.damageMult);
        }
        hitFanfare();
        Destroy(gameObject);

    }

    void hitFanfare()
    {
        if(GameManager.instance.playerScript.getFanfare()!=null)
            Instantiate(GameManager.instance.playerScript.getFanfare(), transform.position, transform.rotation);

    }
}


