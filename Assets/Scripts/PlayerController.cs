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


    [SerializeField] Projectile projectile;
    public List<Projectile> WeapList = new List<Projectile>();

    int selectedWeapon;
    bool isShooting;
    int HpOriginal;
    int StaminaOriginal;
    int jumpCount;

    // Start is called before the first frame update
    void Start()
    {
        AddWeapon(projectile);
        HpOriginal = HP;
        StaminaOriginal = Stamina;
        UpdateUI();

    }

    // Update is called once per frame
    void Update()
    {
 
        if (Input.GetButton("Fire1") && !isShooting)
            StartCoroutine(shoot());

        
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
    public void AddWeapon(Projectile weap)
    {
        WeapList.Add(weap);
    }
}

