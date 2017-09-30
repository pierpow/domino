using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public GameObject ChoiceUI;
	public GameObject ConsequenceUI;

	public Text descriptionText;
	public Text networkText;
	public Text consequenceText;

	public Slider timerBar;

	private Story story;

	public StoryElement currentStoryElement;

	private int cumulatedInactions = 0;

	public enum GameState {
		Choosing,
		Reading
	};

	public GameState currentGameState;

	void Awake()
	{
		story = new Story();

        string filePath = Path.Combine(Application.streamingAssetsPath, "story.json");

        if (File.Exists(filePath)) {
            string dataAsJson = File.ReadAllText(filePath); 
            story = JsonUtility.FromJson<Story>(dataAsJson);
        } else {
            Debug.LogError("Cannot load game data!");
        }

		ChangeToReadingState();

		timerBar.maxValue = 10;
		timerBar.minValue = 0;
		timerBar.value = timerBar.maxValue;
	}

	void FixedUpdate()
	{
		if (currentGameState == GameState.Choosing) {
			timerBar.value -= Time.deltaTime;
			if (timerBar.value <= 0) {
				DoNothing();
			}
		}
	}

	void Update()
	{
		if (currentGameState == GameState.Reading) {
			if (Input.GetMouseButtonDown(0)) {
				SwitchToChoiceView();
			}
		}
	}

	public void DoSomething() {
		IncrementNetwork();
		DisplayConsequenceText();
	}

	public void DoNothing() {
		DisplayInactionText();		
	}

	void IncrementNetwork() {
		int currentNetwork = System.Convert.ToInt32(networkText.text);
		networkText.text = (currentNetwork + currentStoryElement.networkBonus).ToString();
	}

	void UpdateToNewStoryElement() {
		int numberOfStoryElements = story.storyElements.Length;
		int level = Random.Range(0, numberOfStoryElements);
		currentStoryElement = story.storyElements[level];
	}

	void DisplayConsequenceText() {
		consequenceText.text = string.Format(currentStoryElement.consequenceDescription, currentStoryElement.networkBonus);
		ChangeToReadingState();
	}
	void DisplayInactionText() {
		consequenceText.text = "Vous ne faites rien.";
		ChangeToReadingState();
	}

	void SwitchToChoiceView() {
        ChangeToChoiceState();

		UpdateToNewStoryElement();
		descriptionText.text = currentStoryElement.description;

		timerBar.value = 100;
	}

	void ChangeToReadingState() {
		currentGameState = GameState.Reading;
		ChoiceUI.SetActive(false);
		ConsequenceUI.SetActive(true);
	}

	void ChangeToChoiceState() {
		currentGameState = GameState.Choosing;
		ChoiceUI.SetActive(true);
		ConsequenceUI.SetActive(false);
	}
}
