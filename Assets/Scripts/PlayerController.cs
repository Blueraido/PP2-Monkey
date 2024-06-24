using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] Camera cam;

    [SerializeField] int HP;
    [SerializeField] int Stamina;
    [SerializeField] float shootSpeed;
    [SerializeField] PlayerProjectile projectile;
    [SerializeField] WeaponStats defaultWeap;
    [SerializeField] GameObject Weapmodel;
    [SerializeField] GameObject hitFanfare;
    public List<WeaponStats> WeapList = new List<WeaponStats>();

    public int destroyTime;
    public float arc;
    int selectedWeapon;
    bool isShooting;
    int HpOriginal;
    int StaminaOriginal;
    int selectedWeap;
    float projectileSpeed;
    int shootDamage;

    public static PlayerController instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        GetWeaponStats(defaultWeap);
        HpOriginal = HP;
        StaminaOriginal = Stamina;
        UpdateUI();

    }

    // Update is called once per frame
    void Update()
    {
 
        if (Input.GetButton("Fire1") && !isShooting)
            StartCoroutine(shoot());

         SelectWeap();
    }



    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(projectile, cam.transform.position + new Vector3(0, 1, 1), cam.transform.rotation);
        yield return new WaitForSeconds(shootSpeed);
        isShooting = false;
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        UpdateUI();
        if (HP <= 0)
        {
            GameManager.instance.updateLose();
        }
    }
    void UpdateUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / HpOriginal;
        GameManager.instance.playerHealthValueText.text = HP.ToString() + " / " + HpOriginal.ToString();
        GameManager.instance.playerStaminaBar.fillAmount = (float)Stamina / StaminaOriginal;
        GameManager.instance.playerStaminaValueText.text = Stamina.ToString() + " / " + StaminaOriginal.ToString();

    }

    void SelectWeap()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeap < WeapList.Count - 1)
        {
            selectedWeap++;
            changeWeap();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedWeap > 0)
        {
            selectedWeap--;
            changeWeap();   
        }
    }

    void changeWeap()
    {
        UpdateUI();
        shootDamage = WeapList[selectedWeap].damage;
        shootSpeed = WeapList[selectedWeap].speed;
        projectileSpeed = WeapList[selectedWeap].ProjectileSpeed;
        projectile = WeapList[selectedWeap].projectile;
        hitFanfare = WeapList[selectedWeap].HitEffect;
        destroyTime = WeapList[selectedWeap].destroyTime;
        arc = WeapList[selectedWeap].arc;
        Weapmodel.GetComponent<MeshFilter>().sharedMesh = WeapList[selectedWeap].Weapmodel.GetComponent<MeshFilter>().sharedMesh;
        Weapmodel.GetComponent<MeshRenderer>().sharedMaterial = WeapList[selectedWeap].Weapmodel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void GetWeaponStats(WeaponStats weap)
    {

        WeapList.Add(weap);
        selectedWeap = WeapList.Count - 1;
        changeWeap();

    }
    public WeaponStats GetSelectedWeapon()
    {
        return WeapList[selectedWeapon];
    }
    public GameObject getFanfare()
    {
        return hitFanfare;
    }
}

