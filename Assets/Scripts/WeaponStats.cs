using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenuAttribute]
public class WeaponStats : ScriptableObject
{

    public GameObject Weapmodel;

    [Range(1, 50)] public float damage;
    [Range(0.001f, 15)] public float speed;
    [Range(1, 20)] public float ProjectileSpeed;
    [Range(1, 10)] public int destroyTime;
    public PlayerProjectile projectile;
    public GameObject HitEffect;
    public int ammo;
    public int ammoMax;
    public bool isAmmoInfinite;
    [Range(0, 10)] public float arc;

}
