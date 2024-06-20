using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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

    [SerializeField] GameObject expStart1;
    [SerializeField] GameObject expStart2;
    [SerializeField] GameObject expStart3;

    [SerializeField] List<GameObject> expList;
    [SerializeField] bool debug;

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
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))   // Debug code
        {
            if (debug)
            {
                updateExp(10);
            }
        }

    }

    public void updateExp(int exp = 0) 
    {
        playerExp += exp;
        UpdateUI();
        if (playerExp >= expForNextLevel)
        {
            updateLevel(1);
        } // There may be an error where, if exp is greater than the next expfornextlevel value,
    } // an update in exp will be needed to start the level up menu. Hopefully we can't level that quickly.

    public void updateLevel(int level = 0)
    {
        expOption1.SetActive(false);
        expOption2.SetActive(false);
        expOption3.SetActive(false);

        playerExp -= expForNextLevel; // Needs testing to verify this works properly
        expForNextLevel += baseExpForNextLevel/2; // Temp level increase
        playerLevel += level;

        UpdateUI();

        // Generates random number based on list size, replaces spaces on level up to fix
        List<GameObject> temp = new List<GameObject> ();
        
        for (int randMenu = 0; randMenu < expList.Count(); randMenu++)
        {
            temp.Add(expList[randMenu]);
        }

        int rand = Random.Range(0, temp.Count);
        expOption1 = temp[rand];
        expOption1.SetActive(true);
        expOption1.transform.position = expStart1.transform.position;
        temp.RemoveAt(rand);

        rand = Random.Range(0, temp.Count);
        expOption2 = temp[rand];
        expOption2.SetActive(true);
        expOption2.transform.position = expStart2.transform.position;
        temp.RemoveAt(rand);

        rand = Random.Range(0, temp.Count);
        expOption3 = temp[rand];
        expOption3.SetActive(true);
        expOption3.transform.position = expStart3.transform.position;
        temp.RemoveAt(rand);

        GameManager.instance.updateLevelUp(); // Displays level up menu
    }

    void UpdateUI()
    {
        GameManager.instance.playerExpBar.fillAmount = (float)playerExp / expForNextLevel;
        GameManager.instance.playerExpValueText.text = playerExp.ToString() + "  /  " + expForNextLevel.ToString() + " exp";
        GameManager.instance.playerLevelValueText.text = "Level " + playerLevel.ToString();
        // Debug.Log("UI UPDATED!!!!");
    }
}
