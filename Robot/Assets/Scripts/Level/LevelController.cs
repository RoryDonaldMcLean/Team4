using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private List<LevelControlBaseClass> levelScripts = new List<LevelControlBaseClass>();
	public int currentLevel = 0;
	public bool levelSelect = false;

	void Start ()
    {
		//LevelSelect control was added to allow game designers / debugging to occur
		//in individual scenes, if this functionality is disabled, use the int value that
		//controls which level is to be loaded in the newly loaded scene.
		//A safety measure was added to ensure that in level 1 the first level setup is secured.		
		if (!levelSelect)
		{
			GameObject puzzle = GameObject.Find ("PuzzleZero");
			if (puzzle != null)
			{
				currentLevel = 0;
			} 
			else
			{
				currentLevel = PlayerPrefs.GetInt ("level");
			}
		} 

        Level(currentLevel);
    }

    //When called, the next level is loaded, incrementing the level parameter
    //which is used to load the correct level scene and level script.     
    public void NextLevel()
    {
        currentLevel++;
		PlayerPrefs.SetInt("level", currentLevel);
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Based on the level specified, the old level will be deleted and removed and the new level components will be loaded
    private void Level(int levelNumber)
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
        }
    }

    private void LevelZero()
    {
        levelScripts.Add(this.gameObject.AddComponent<PuzzleOnBoardingProcess>());
    }

    private void LevelOne()
    {
        levelScripts.Add(this.gameObject.AddComponent<PuzzleOneReflectors>());
        this.gameObject.AddComponent<TutorialOne>();
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
        levelScripts.Add(this.gameObject.AddComponent<PuzzleFourFinal>());
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
