using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	const int MAX_TIMER_VALUE = 5;

	public GameObject ChoiceUI;
	public GameObject ConsequenceUI;

	public Text descriptionText;
	public Text networkText;
	public Text riskText;
	public Text consequenceText;
	public Text daysText;
	public Slider timerBar;
	public GameObject actionImage;
	private Image actionImageComponent;

	private Story story;
	public StoryElement currentStoryElement;
	public Pitfall currentPitfall;

	public GameObject musicManager;
	private MusicScript musicScript;

	private AudioSource audioSourceComponent;

	private int cumulatedInactions = 0;
	private int dayNumber = 0;
	private int networkAmount = 0;
	private int riskAmount = 0;
	private int currentLevel = 0;
	private List<int> unlockedStories = new List<int>();
	private List<int> alreadyDoneStories = new List<int>();

	public enum GameState {
		Choosing,
		Arrested,
		Reading
	};

	public GameState currentGameState;

	void Awake()
	{
		musicScript = musicManager.GetComponent<MusicScript>();
		musicScript.PlayMusicForLevel(riskAmount);

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

		audioSourceComponent = gameObject.GetComponent<AudioSource>();

		timerBar.maxValue = MAX_TIMER_VALUE;
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

		unlockedStories.Add(currentStoryElement.unlocksElementId);
		alreadyDoneStories.Add(currentStoryElement.id);

		musicScript.PlayMusicForLevel(riskAmount);
	}

	public void DoNothing() {
		DisplayInactionText();		
		cumulatedInactions += 1;
		DecrementRisk();
		if (cumulatedInactions >= 2) {
			DecrementNetwork();
		}

		musicScript.PlayMusicForLevel(riskAmount);
	}

	void IncrementNetwork() {
		networkAmount += currentStoryElement.networkBonus;
		networkText.text = networkAmount.ToString();
	}

	void IncrementRisk() {
		riskAmount += currentStoryElement.dangerAmount;
		riskText.text = riskAmount.ToString();
	}

	void DecrementRisk() {
		// TODO
		riskAmount -= 10;
		if (riskAmount < 0) {
			riskAmount = 0;
		}
		riskText.text = riskAmount.ToString();
	}

	void DecrementNetwork() {
		// TODO
		networkAmount -= 10;
		if (networkAmount < 0) {
			networkAmount = 0;
		}
		networkText.text = networkAmount.ToString();
	}

	void UpdateToNewStoryElement() {
		dayNumber += 1;
		daysText.text = dayNumber.ToString();

        // TODO
        int caughtScore = Random.Range(40, 100);
		if (riskAmount > caughtScore) {
			currentStoryElement = null;
			int numberOfPitfalls = story.pitfalls.Length;
			int pitfallIndex = Random.Range(0, numberOfPitfalls);
			currentPitfall = story.pitfalls[pitfallIndex];
		} else {
			currentPitfall = null;
			int numberOfStoryElements = story.storyElements.Length;

			List<StoryElement> accessibleStoryElements = new List<StoryElement>();

			// This is HORRIBLE >:(
			for (int i = 0; i < numberOfStoryElements; i++) {
				StoryElement current = story.storyElements[i];
				bool isLevelAchievedHighEnough = current.prerequisites.level <= currentLevel;
				bool isStoryAlreadyDone = alreadyDoneStories.Contains(current.id) && current.prerequisites.doableOnce;
				if (isLevelAchievedHighEnough && !isStoryAlreadyDone) {
					if (!current.prerequisites.needsUnlock) {
						// If it does not need any unlock, it's okay!
						accessibleStoryElements.Add(current);
					} else {
						// Otherwise, element has to be present in list
						if (unlockedStories.Contains(current.id)) {
							accessibleStoryElements.Add(current);
						}
					}
				}
			}

			int numberOfAccessibleStoryElements = accessibleStoryElements.Count;
			int level = Random.Range(0, numberOfAccessibleStoryElements);
			currentStoryElement = accessibleStoryElements[level];

			currentLevel++;
		}
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

		UpdateToNewStoryElement();
		
		if (currentPitfall != null) {
			ChangeToArrestedState();
			consequenceText.text = currentPitfall.description;
			audioSourceComponent.PlayOneShot ((AudioClip)Resources.Load (currentPitfall.sound));
		} else {
            ChangeToChoiceState();
			actionImage.SetActive(true);
			Sprite newSprite = Resources.Load(currentStoryElement.picture, typeof(Sprite)) as Sprite;
			actionImageComponent.sprite = newSprite;

			descriptionText.text = currentStoryElement.description;
		}

		timerBar.value = MAX_TIMER_VALUE;
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

	void ChangeToArrestedState() {
		currentGameState = GameState.Arrested;
		ChoiceUI.SetActive(false);
		ConsequenceUI.SetActive(true);
	}
}
