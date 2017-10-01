using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	const int MAX_TIMER_VALUE = 5;

	public GameObject ChoiceUI;
	public GameObject ConsequenceUI;

	public GameObject player;

	public Text descriptionText;
	public Text networkText;
	public Text riskText;
	public Text consequenceText;
	public Text daysText;
	public Text daysTextInOverlay;
	public Slider timerBar;
	public GameObject actionImage;
	private Image actionImageComponent;

	public GameObject overlay;
	public GameObject actionInterface;
	public GameObject characterIntroductionInterface;
	public GameObject introductionInterface;

	private Story story;
	public StoryElement currentStoryElement;
	public Pitfall currentPitfall;

	public GameObject musicManager;
	private MusicScript musicScript;

	private AudioSource audioSourceComponent;

	private int cumulatedInactions = 0;
	private int dayNumber = 1;
	private int networkAmount = 0;
	private int riskAmount = 0;
	private int currentLevel = 0;
	private List<int> unlockedStories = new List<int>();
	private List<int> alreadyDoneStories = new List<int>();

	public enum GameState {
		ReadingIntroduction,
		ReadingCharacterIntroduction,
		ReadingDays,
		Choosing,
		Arrested,
		Reading
	};

	public GameState currentGameState;

	void Awake()
	{
		overlay.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
		characterIntroductionInterface.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
		actionInterface.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;

		musicScript = musicManager.GetComponent<MusicScript>();
		musicScript.Init();
		musicScript.PlayMusicForLevel(riskAmount);

		story = new Story();

		string filePath = Path.Combine(Application.streamingAssetsPath, "story.json");
		string dataAsJson = File.ReadAllText(filePath); 
		story = JsonUtility.FromJson<Story>(dataAsJson);

		ChangeToIntroductionState();

		actionImageComponent = actionImage.GetComponent<Image>();
		actionImage.SetActive(false);

		audioSourceComponent = gameObject.GetComponent<AudioSource>();

		timerBar.maxValue = MAX_TIMER_VALUE;
		timerBar.value = timerBar.maxValue;

		PlayerScript playerScript = player.GetComponent<PlayerScript>();
		playerScript.ChangeColor();
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
		bool isMouseClicked = Input.GetMouseButtonDown(0);

		switch (currentGameState) {
			case GameState.ReadingIntroduction:
				if (isMouseClicked) {
					ChangeToReadingCharacterIntroduction();
				}
				break;
			case GameState.ReadingCharacterIntroduction:
				if (isMouseClicked) {
					ChangeToReadingDaysState();
				}
				break;
			case GameState.ReadingDays:
				if (isMouseClicked) {
					SwitchToChoiceView();
				}
				break;
			case GameState.Reading:
				if (isMouseClicked) {
					ChangeToReadingDaysState();
				}
				break;
			default:
				break;
		}
	}

	public void DoSomething() {
		cumulatedInactions = 0;
		IncrementNetwork();
		IncrementRisk();
		DisplayConsequenceText();

        currentLevel++;

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
		// TODO Day view is shifted by 1
		// This is stupid but no time to fix it
		daysText.text = dayNumber.ToString();
		dayNumber += 1;
		daysTextInOverlay.text = "Jour " + dayNumber.ToString();

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

		}
	}

	void DisplayConsequenceText() {
		consequenceText.text = string.Format(currentStoryElement.consequenceDescription, currentStoryElement.networkBonus);
		ChangeToReadingState();
	}
	void DisplayInactionText() {
		// Default value
		string inactionText = "Vous ne faites rien.";

		if (currentStoryElement.inactionConsequenceDescription != null) {
			inactionText = currentStoryElement.inactionConsequenceDescription;
		}
		consequenceText.text = inactionText;
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
		overlay.SetActive(false);
		characterIntroductionInterface.SetActive(false);
	}

	void ChangeToChoiceState() {
		currentGameState = GameState.Choosing;
		ChoiceUI.SetActive(true);
		ConsequenceUI.SetActive(false);
		overlay.SetActive(false);
		characterIntroductionInterface.SetActive(false);
	}

	void ChangeToArrestedState() {
		currentGameState = GameState.Arrested;
		ChoiceUI.SetActive(false);
		ConsequenceUI.SetActive(true);
		overlay.SetActive(false);
		characterIntroductionInterface.SetActive(false);
	}

	void ChangeToReadingDaysState() {
		currentGameState = GameState.ReadingDays;
		overlay.SetActive(true);
		characterIntroductionInterface.SetActive(false);
	}

	void ChangeToIntroductionState() {
		currentGameState = GameState.ReadingIntroduction;
		introductionInterface.SetActive(true);
		characterIntroductionInterface.SetActive(false);
		overlay.SetActive(false);
	}

	void ChangeToReadingCharacterIntroduction() {
		currentGameState = GameState.ReadingCharacterIntroduction;
		introductionInterface.SetActive(false);
		characterIntroductionInterface.SetActive(true);
		overlay.SetActive(false);
	}
}
