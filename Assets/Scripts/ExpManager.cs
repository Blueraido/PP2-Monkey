using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    // Reference the instance
    public static ExpManager instance;

    // Player's level and exp, public for now but should prob have getters and setters for security
    public int playerExp = 0;
    public int playerLevel = 0;
    [SerializeField] int expForNextLevel = 40; // Increases by itself / 2 every level up
    int baseExpForNextLevel;


    // Serialized inputs for starting exp, level, and level mult; purely for testing
    [SerializeField] int modExp = 0;
    [SerializeField] int modLevel = 0;

    // Objects to be replaced by random list of 
    [SerializeField] GameObject expOption1;
    [SerializeField] GameObject expOption2;
    [SerializeField] GameObject expOption3;
    
    [SerializeField] GameObject randLevelDamage;
    [SerializeField] GameObject randLevelHealth;
    [SerializeField] GameObject randLevelSpeed;
    [SerializeField] GameObject randLevelStamina;
    [SerializeField] GameObject randLevelJumpcount;
    List<GameObject> expList;

    // Start is called before the first frame update
    void Awake()
    {
        // Singleton moment
        instance = this;

        baseExpForNextLevel = expForNextLevel;

        // Purely for the modifiers: makes sure that expfornextlevel is properly scaled to overriden level
        for (int levelOverride = 0; levelOverride < modLevel; levelOverride++)
        {
            expForNextLevel += expForNextLevel / 2;
        }

        // Overrides for testing
        playerExp += modExp;
        playerLevel += modLevel;
    }

    // Update is called once per frame
    void Update()
    {
                                          
        if (Input.GetButtonDown("Fire1"))   // Debug code
        {
            updateExp(10);
        }
        
    }

    public void updateExp(int exp = 0) 
    {
        playerExp += exp;
        if (playerExp >= expForNextLevel)
        {
            updateLevel(1);
        } // There may be an error where, if exp is greater than the next expfornextlevel value,
    } // an update in exp will be needed to start the level up menu. Hopefully we can't level that quickly.

    public void updateLevel(int level = 0)
    {
        playerExp -= expForNextLevel; // Needs testing to verify this works properly
        expForNextLevel += baseExpForNextLevel/2; // Temp level increase
        playerLevel += level;

        // Generates random number based on list size, replaces spaces on level up to fix
        expList = new List<GameObject>();
        randomizeList();

        int rand = Random.Range(0, expList.Count);
        expList[rand].transform.position = expOption1.transform.position;
        expOption1 = expList[rand];
        expOption1.SetActive(true);
        expList.RemoveAt(rand);

        rand = Random.Range(0, expList.Count);
        expList[rand].transform.position = expOption2.transform.position;
        expOption2 = expList[rand];
        expOption2.SetActive(true);
        expList.RemoveAt(rand);

        rand = Random.Range(0, expList.Count);
        expList[rand].transform.position = expOption3.transform.position;
        expOption3 = expList[rand];
        expOption3.SetActive(true);
        expList.RemoveAt(rand);

        GameManager.instance.updateLevelUp(); // Displays level up menu
    }
    void randomizeList() // Anytime we add more menus, add them here also.
    {
        expList.Add(randLevelDamage);
        expList.Add(randLevelHealth);
        expList.Add(randLevelSpeed);
        expList.Add(randLevelStamina);
        expList.Add(randLevelJumpcount);
    }
}
