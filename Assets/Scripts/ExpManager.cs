using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExpManager : MonoBehaviour
{
    // Reference the instance
    public static ExpManager instance;

    // Player's level and exp, public for now but should prob have getters and setters for security
    public int playerExp;
    public int playerLevel;
    public int playerLevelScale;
    int expForNextLevel; // Will get multiplied by playerLevelScale per level gained

    // Serialized inputs for starting exp, level, and level mult; purely for testing
    [SerializeField] int modExp = 0;
    [SerializeField] int modLevel = 0;
    [SerializeField] int modLevelScale = 1;

    // Start is called before the first frame update
    void Awake()
    {
        // Singleton moment
        instance = this;

        // Base experience needed to go from level 0 to 1
        // Could be refactord to go from 1 to 2
        expForNextLevel = 40;

        // Overrides for testing
        playerExp += modExp;
        playerLevel += modLevel;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateExp(int exp = 0) 
    {
        playerExp += exp;
        if (playerExp >= expForNextLevel)
        {
            playerExp -= expForNextLevel; // Needs testing to verify this works properly
            updateLevel(1);
        } // There may be an error where, if exp is greater than the next expfornextlevel value,
    } // an update in exp will be needed to start the level up menu. Hopefully we can't level that quickly.

    public void updateLevel(int level)
    {
        playerLevel += level;
        expForNextLevel = expForNextLevel * playerLevelScale * modLevelScale;
        GameManager.instance.updateLevelUp();
    }
}
