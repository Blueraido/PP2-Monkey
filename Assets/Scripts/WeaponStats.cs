using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenuAttribute]
public class WeaponStats : MonoBehaviour
{

    public GameObject model;

    [Range(1, 50)] public int damage;
    [Range(1, 15)] public int speed;
    [Range(1, 10)] public int destroyTime;
    public GameObject HitEffect;
    public AudioClip Sound;

    [Range(1, 10)] public float arc;

}
