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
	public Text daysText;
	public GameObject actionImage;
	private Image actionImageComponent;

	public Slider timerBar;

	private Story story;

	public StoryElement currentStoryElement;

	private int cumulatedInactions = 0;
	private int dayNumber = 0;
	private int networkAmount = 0;
	private int riskAmount = 0;

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

		actionImageComponent = actionImage.GetComponent<Image>();
		actionImage.SetActive(false);

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
		cumulatedInactions = 0;
		IncrementNetwork();
		IncrementRisk();
		DisplayConsequenceText();
	}

	public void DoNothing() {
		DisplayInactionText();		
		cumulatedInactions += 1;
		DecrementRisk();
		if (cumulatedInactions >= 2) {
			DecrementNetwork();
		}
	}

	void IncrementNetwork() {
		networkAmount += currentStoryElement.networkBonus;
		networkText.text = networkAmount.ToString();
	}

	void IncrementRisk() {
		riskAmount += currentStoryElement.dangerAmount;
	}

	void DecrementRisk() {
		riskAmount -= 10;
		if (riskAmount < 0) {
			riskAmount = 0;
		}
	}

	void DecrementNetwork() {
		networkAmount -= 10;
		if (networkAmount < 0) {
			networkAmount = 0;
		}
		networkText.text = networkAmount.ToString();
	}

	void UpdateToNewStoryElement() {
		dayNumber += 1;
		daysText.text = dayNumber.ToString();

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

		actionImage.SetActive(true);
		Sprite newSprite = Resources.Load(currentStoryElement.picture, typeof(Sprite)) as Sprite;
		actionImageComponent.sprite = newSprite;

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
