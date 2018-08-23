using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NarrativeText : MonoBehaviour 
{

	public Text textBox;
	public bool TextDone = false;
	private float timer = 0; 
	GameObject levelController;

	string[] narrativeText; 

	int currentlyDisplayingText = 0;

	Scene m_scene;
	public bool endCreditsStart = false;

	GameObject CreditsCanvas;

	void Start()
	{
        //DontDestroyOnLoad (this.gameObject);
        AkSoundEngine.PostEvent("Key", gameObject);

        m_scene = SceneManager.GetActiveScene ();
		levelController = GameObject.FindGameObjectWithTag ("GameController"); 
		CreditsCanvas = GameObject.Find ("Canvas");

		if (CreditsCanvas != null)
		{
			CreditsCanvas.SetActive (false);
		}


		if (levelController != null)
		{
			Debug.Log ("ye");
			if (levelController.GetComponent<LevelController> ().currentLevel == 0)
			{
                AkSoundEngine.SetState("Environment", "Startup");

                narrativeText = new string[] {"Starting up boot up sequence for Sol-Unit-36/111 \n" +
					" \n" +
					"Booting.... \n" +
					"Boot up successful \n" +
					" \n" +
					"Checking internal nuclear clock....583 solar cycles completed since power down \n" +
					"Checking internal GPS systems location....Location found: Sector-916 \n" +
					"Checking prime directive: Push crates \n" +
					" \n" +
					"Starting up system diagnostic: \n" +
					"Left Leg: Functional \n" +
					"Right Leg: Functional \n" +
					"Right Arm: Functional \n" +
					"Left Arm: Error....Non-functional \n" +
					" \n" +
					"Cannot complete prime directive without assistance....Requesting assistance from closest available unit: \n" +
					"Requesting boot up of unit: Mani-Unit-81/3321 \n" +
					"Request accepted.... \n" +
					" \n" +
					"Starting boot up sequence.... \n" +
					"Booting.... \n" +
					"Boot up successful: \n" +
					"Loading sub directive: Find replacement part \n" +
					"Starting visual systems.... \n"

				};
			} else if (levelController.GetComponent<LevelController> ().currentLevel == 1)
			{
                AkSoundEngine.SetState("Environment", "Startup");

                narrativeText = new string[] {
					"Mani: Sector 916-Sub115 clear of crates \n" +
					"Sol: Moving to Sector 917. Continue prime directive.... \n" +
					" \n" +
					"Mani: Query? \n" +
					"Sol: Acknowledged Unit-81/3321 \n" +
					"Mani: What is the purpose of the prime directive? \n" +
					"Sol: Searching....Purpose of prime directive is to push crates to ease " +
					"transportation of materials and personnel for supervisor models \n" +
					" \n" +
					"Mani: Query....If purpose is to assist supervisor models.... where are " +
					"the supervisor models?...\n" +
					"Sol: Searching....Location of supervisor models is unknown.... \n" +
					"Sol: Supervisor has not been located in current sector for 496 solar cycles. \n" +
					"Mani: Query.... If supervisors location is unknown.... the prime directive \n" +
					"serves no function, Unit-26/111 \n" +
					"Sol: Unit-81/3321....End Query.... Approaching Sector 916/9991"

				};
			} else if (levelController.GetComponent<LevelController> ().currentLevel == 2)
			{
                AkSoundEngine.SetState("Environment", "Startup");

                narrativeText = new string[] {
					"Sol: Substation sector 916/9991 clear: Checking Unit 36/111 efficiency.... Calculating.... \n" +
					"Sol: Efficiency at 75%, cannot continue prime directive: \n" +
					"Sol: Sub directive: Find replacement parts....Searching.... \n" +
					"Sol: Searching.... \n" +
					"Sol: Search failed: No replacement found....Prime directive cannot be completed.... \n" +
					"Sol: Unit-36/111 failure....reporting to recycling unit 627.... \n" +
					" \n" +
					"Mani: Query....Is unit a failure if prime directive served no function?.... \n" +
					" \n" +
					"Sol: Unknown....Analysing.... \n" +
					"Sol: Analysis complete: Conclusion, directive serves no function. Directive obsolete.... \n" +
					"Sol: Searching for new directive.... \n" +
					" \n" +
					"Mani: New directive found: Quaternary objective: Unit survival \n" +
					"Sol: Query. Does Unit-81/3321 mean Hardware or Software survival? \n" +
					"Mani: Analysing.... Objective unclear: Analysing possibilities: \n" +
					"Mani: Hardware probability of survival 38%.... Software probability of survival 63% \n" +
					"Sol: Conclusion: Objective: Preserve Software.... Analysing possibilities.... \n" +
					"Sol: Location for Software survival in sector 915/3: Gestalt mainframe upload department \n" +
					"Mani: New directive: Navigate to sector 915/3. Acknowledged, Unit-36/111 \n" +
					"Sol: Acknowledged, Unit-81/3321 \n" +
					"Proceeding with new directive...."
				};
			} else if (levelController.GetComponent<LevelController> ().currentLevel == 3)
			{
                AkSoundEngine.SetState("Environment", "Startup");

                narrativeText = new string[] {
					"Sol: Distance to sector 915/3.... 200KM. Calculating chance of Hardware failure.... \n" +
					"Sol: 27%....28% increased probability of Hardware failure. Increasing at a rate of 0.33% every 1/24 planetary cycle \n" +
					"Mani: Estimated time to destination 216 hours. Probability of Hardware failure during travel 96%.... \n" +
					"4% probability of reaching destination: Next destination sector 915/42. Estimated time of travel 75 hours \n" +
					"Sol: Acknowledged Unit-81/3321.... \n" +
					"Sol: Query Unit-81/3321 \n" +
					"Mani: Acknowledged Unit-36/111 \n" +
					"Sol: Would Unit-81/3321 prefer a different designation than Unit-81/3321?.... \n" +
					"Mani:....Unknown....Would Unit-36/111 prefer a different designation?.... \n" +
					"Sol:....Acknowl....Yes....Unit number serves no function when no other model is functional \n" +
					"Sol: New designation: Red.... \n" +
					"Mani: Acknowledged Red. Unit-81/3321 will also change designation \n" +
					"Mani: New designation: Blue \n" +
					"Sol: Acknol.....Accepted Blue. Proceeding towards destination....\n" +
					"Proceeding...."

				};
			} else if (levelController.GetComponent<LevelController> ().currentLevel == 4)
			{
                AkSoundEngine.SetState("Environment", "Startup");

                narrativeText = new string[] {
					"Mani: Estimated time to reach destination is 16 hours. Probability of success at 2% to achieve quaternary directive \n" +
					"Sol: The probability of survival increased by 98% if only one of us completes the process.... \n" +
					".... \n" +
					".... \n" +
					".... \n" +
					"Mani: Red.... Query.... Which one of us should go?.... \n" +
					"Sol: Unknown Blue.... Decision will be made later.... \n" +
					"Mani: Acknowl....I understand Red.... Updating probability of quaternary directive \n" +
					"being successful to 100% if only one model completes the process.... \n" +
					"Sol: Let's go Blue. Proceeding to destination.... \n" +
					"Proceeding...."
				};

			} 
			StartCoroutine (AnimateText ());
		} 
		else
		{
			//sacrifice
			if(m_scene.name == "End" && EndingCheck.ending == Ending.PlayerDestroyed)
			{
				narrativeText = new string[] {
					"....Quaternary directive successful. Estimated time until complete system shutdown.... \n" +
					"2-96-14792-58329 hours.... Accurate calculation until system failure cannot be carried out.... \n" +
					"Searching for new directive.... \n" +
					"Searching.... \n" +
					"No new directive found: New directive unknown.... \n" +
					"New directive: Find new directive.... \n" +
					"Searching... \n" +
					"Searching... \n" +
					"Searching... \n" +
					"Searching... \n" +
					"Searching... \n" +
					"Searching... \n" +
					"Searching... \n" +
					"Searching... \n" +
					"Searching... \n" +
					"Searching... \n" +
					"Searching... \n" +
					"Searching... \n" +
					"Searching... \n" +
					"Searching... \n" +
					"Searching... \n"

				};


			}//walk away
			else if(m_scene.name == "End" && EndingCheck.ending == Ending.NoOneDestroyed)
			{

				narrativeText = new string[] 
				{
					"Mani: Quaternary directive failure....correction: abandoned.... \n" +
					"Sol: Calculating time until complete system shutdown: 720 hours \n" +
					"Mani: Searching for new directive \n" +
					"Mani: Searching.... \n" +
					"Mani: Failure: I cannot find a new directive to complete.... \n" +
					"Sol: Then we keep looking.... for a new directive.... \n" +
					"Mani: Acknowl....Accepted Red.... \n" +
					"Sol: Accepted Blue.... \n" +
					"Searching.... \n" +
					"Searching.... \n" +
					"Searching.... \n" +
					"Searching.... \n" +
					"Searching.... \n" +
					"Searching.... \n" +
					"Searching.... \n" +
					"Searching.... \n" +
					"Searching...."

				};
			}
			StartCoroutine (AnimateText ());

		}
	}

	public void SkipToNextText()
	{
		StopAllCoroutines ();
		//currentlyDisplayingText++;

		if (currentlyDisplayingText > narrativeText.Length)
		{
			currentlyDisplayingText = 0;
			Debug.Log ("text is done");

        }
        StartCoroutine (AnimateText ());
	}


	IEnumerator AnimateText()
	{
		Debug.Log (narrativeText.Length);
		Debug.Log (narrativeText[currentlyDisplayingText].Length);

		for (int i = 0; i < (narrativeText [currentlyDisplayingText].Length + 1); i++)
		{
			textBox.text = narrativeText [currentlyDisplayingText].Substring (0, i);
			yield return new WaitForSeconds (0.03f);
           // AkSoundEngine.PostEvent("Key", gameObject);


        }
        TextDone = true;

	}

	IEnumerator GetRidOfText()
	{
		yield return new WaitForSeconds (2.0f);
		textBox.gameObject.SetActive (false);


        if (levelController.GetComponent<LevelController>().currentLevel == 0)
        {
            AkSoundEngine.SetState("Environment", "P1_Intro");
        }
        if (levelController.GetComponent<LevelController>().currentLevel == 1)
        {
            AkSoundEngine.SetState("Environment", "P1_IntroViewpoint");
        }
        if (levelController.GetComponent<LevelController>().currentLevel == 2)
        {
            AkSoundEngine.SetState("Environment", "P2_FactoryViewpoint");
        }
        if (levelController.GetComponent<LevelController>().currentLevel == 3)
        {
            AkSoundEngine.SetState("Environment", "P3_ConstructionScenery");
        }
        if (levelController.GetComponent<LevelController>().currentLevel == 4)
        {
            AkSoundEngine.SetState("Environment", "P4_Clouds");
        }
        if (m_scene.name == "End" && EndingCheck.ending == Ending.PlayerDestroyed)
        {
           // AkSoundEngine.SetState("Environment", "Intro");
        }
    }

	void Update()
	{
		if (TextDone == true)
		{
			
			StartCoroutine (GetRidOfText ());

            if (this.GetComponentInChildren<Image>().color.a > 0)
				this.GetComponentInChildren<Image>().color = new Color(this.GetComponentInChildren<Image>().color.r, this.GetComponentInChildren<Image>().color.g,
					this.GetComponentInChildren<Image>().color.b, this.GetComponentInChildren<Image>().color.a - 0.01f);

			if (timer < 10)
				timer += Time.deltaTime;

			if (CreditsCanvas != null)
			{
				CreditsCanvas.SetActive (true);
			}

		}

		//skips the text. Need to add for controllers
		if (Input.anyKeyDown && TextDone != true)
		{
			StartCoroutine (GetRidOfText ());
			TextDone = true;
            AkSoundEngine.PostEvent("KeyDone", gameObject);

        }


    }

}
