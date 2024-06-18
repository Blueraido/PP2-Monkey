using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    // Reference the instance
    public static ExpManager instance;

    // Player's level and exp, public for now but should prob have getters and setters for security
    public int playerExp = 0;
    public int playerLevel = 0;
    public int expForNextLevel; // Increases by itself / 2 every level up
    public int baseExpForNextLevel;

    // Serialized inputs for starting exp, level, and level mult; purely for testing
    [SerializeField] int modExp = 0;
    [SerializeField] int modLevel = 0;

    // Start is called before the first frame update
    void Awake()
    {
        // Singleton moment
        instance = this;

        // Base experience needed to go from level 0 to 1
        // Could be refactord to go from 1 to 2
        expForNextLevel = 40;
        baseExpForNextLevel = expForNextLevel;

        // Purely for the modifiers: makes sure that expfornextlevel is properly scaled to overriden level
        for (int levelOverride = 0; levelOverride < modLevel; levelOverride++)
        {
            expForNextLevel += baseExpForNextLevel / 2;
        }

        // Overrides for testing
        playerExp += modExp;
        playerLevel += modLevel;
    }

    // Update is called once per frame
    void Update()
    {
        /*                                   
        if (Input.GetButtonDown("Fire1"))   // Debug code
        {
            updateExp(10);
        }
        */
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
        GameManager.instance.updateLevelUp(); // Manages menu process
    }
}
