using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private List<LevelControlBaseClass> levelScripts = new List<LevelControlBaseClass>();
    private int currentLevel = 4;

	void Start ()
    {
        //loads first level automatically 
        Level(currentLevel);
    }
    //based on the level specified, the old level will be deleted and removed and the new level components will be loaded
    public void Level(int levelNumber)
    {
        if (levelScripts.Count > 0) CleanUp();
        switch (levelNumber)
        {
            case 1:
            LevelOne();
            break;
            case 2:
            LevelTwo();
            break;
            case 3:
            LevelThree();
            break;
        }
    }

    private void LevelOne()
    {
        levelScripts.Add(this.gameObject.AddComponent<SCR_TestLevelControl>());
    }

    private void LevelTwo()
    {
        levelScripts.Add(this.gameObject.AddComponent<LightLevelOneControl>());
    }

    private void LevelThree()
    {
        levelScripts.Add(this.gameObject.AddComponent<LightLevelTwoControl>());
    }

    private void CleanUp()
    {
        foreach (LevelControlBaseClass levelscript in levelScripts)
        {
            Destroy(levelscript);
        }
        levelScripts.Clear();
    }
}
