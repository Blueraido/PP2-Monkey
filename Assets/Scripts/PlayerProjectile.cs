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

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        IDamage dmg = collision.gameObject.GetComponent<IDamage>();
        if (dmg != null)
        {
            dmg.takeDamage(GameManager.instance.playerScript.GetSelectedWeapon().damage);
        }
        hitFanfare();
        Destroy(gameObject);

    }

    void hitFanfare()
    {
        Instantiate(GameManager.instance.playerScript.getFanfare(), transform.position, transform.rotation);

    }
}


