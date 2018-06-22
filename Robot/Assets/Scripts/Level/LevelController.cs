using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private List<LevelControlBaseClass> levelScripts = new List<LevelControlBaseClass>();
    private int currentLevel = -1;

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
            case 0:
            LevelZero();
            break;
            case 1:
            LevelOne();
            break;
            case 2:
            LevelTwo();
            break;
            case 3:
            LevelThree();
            break;
            case 4:
            LevelFour();
            break;
            case 5:
            LevelFive();
            break;
        }
    }

    private void LevelZero()
    {
        levelScripts.Add(this.gameObject.AddComponent<PuzzleOnBoardingProcess>());
    }

    private void LevelOne()
    {
        levelScripts.Add(this.gameObject.AddComponent<PuzzleOneReflectors>());
    }

    private void LevelTwo()
    {
        levelScripts.Add(this.gameObject.AddComponent<PuzzleTwoColourChanging>());
    }

    private void LevelThree()
    {
        levelScripts.Add(this.gameObject.AddComponent<PuzzleThreeCombinersandRefractors>());
    }

    private void LevelFour()
    {
        levelScripts.Add(this.gameObject.AddComponent<PuzzleFourCombination>());
    }

    private void LevelFive()
    {
        levelScripts.Add(this.gameObject.AddComponent<PuzzleFiveFinal>());
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
