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
        HpOriginal = HP;
        StaminaOriginal = Stamina;
        UpdateUI();
        GetWeaponStats(defaultWeap);
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
            HP = 0;
            UpdateUI();
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
        shootDamage = WeapList[selectedWeap].damage;
        shootSpeed = WeapList[selectedWeap].speed;
        projectileSpeed = WeapList[selectedWeap].ProjectileSpeed;
        projectile = WeapList[selectedWeap].projectile;
        hitFanfare = WeapList[selectedWeap].HitEffect;
        destroyTime = WeapList[selectedWeap].destroyTime;
        arc = WeapList[selectedWeap].arc;

        //Weapmodel.GetComponent<MeshFilter>().sharedMesh = WeapList[selectedWeap].Weapmodel.GetComponent<MeshFilter>().sharedMesh;
        //Weapmodel.GetComponent<MeshRenderer>().sharedMaterial = WeapList[selectedWeap].Weapmodel.GetComponent<MeshRenderer>().sharedMaterial;
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

    // More getters as far as the eye can see
    public int GetHealthMax()
    {
        return HpOriginal;
    }
    public int GetHealth()
    {
        return HP;
    }
    public int GetStaminaMax()
    {
        return StaminaOriginal;
    }
    public int GetStamina()
    {
        return Stamina;
    }

    // Functions add to current values
    public void AddHealthMax(int toAdd)
    {
        HpOriginal += toAdd;
        UpdateUI();
    }

    public void AddHealth(int toAdd)
    {

        if (toAdd + HP <= HpOriginal)
        {
            HP += toAdd; // If the sum of toAdd and HP goes below max, add like normal.
        }
        else if (toAdd + HP > HpOriginal)
        {
            HP = HpOriginal; // If over max, set HP to max in order to prevent exceeding it.
        }

        UpdateUI();
    }

    public void AddStaminaMax(int toAdd)
    {
        StaminaOriginal += toAdd;
        UpdateUI();
    }

    public void AddStamina(int toAdd) // May or may not get used
    {
        if (toAdd + Stamina <= StaminaOriginal)
        {
            Stamina += StaminaOriginal;
        }
        else if (toAdd + Stamina > StaminaOriginal)
        {
            Stamina = StaminaOriginal;
        }

        UpdateUI();
    }
}

