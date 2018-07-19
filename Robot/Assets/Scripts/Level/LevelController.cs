using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    private List<LevelControlBaseClass> levelScripts = new List<LevelControlBaseClass>();
	public int currentLevel = 0;

	void Start ()
    {
		//Debug.Log (currentLevel);
        //loads first level automatically
		GameObject puzzle = GameObject.Find("PuzzleZero");
		if (puzzle != null)
		{
			currentLevel = 0;
		} 
		else
		{
			currentLevel = PlayerPrefs.GetInt("level");
		}

        Level(currentLevel);
    }

    public void NextLevel()
    {
		Debug.Log ("now");
        //add scene change code here, rewrite level stuff accordingly (currentlevel -> into next scene, so that start of this func loads correct puzzle script)
        currentLevel++;
        //Level(currentLevel);
		PlayerPrefs.SetInt("level", currentLevel);
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
    }

    //based on the level specified, the old level will be deleted and removed and the new level components will be loaded
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
