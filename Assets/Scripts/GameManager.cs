using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Text descriptionText;
	public Text networkText;

	private Story story;

	public StoryElement currentStoryElement;

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
	}

	public void changeLevel() {
		int currentNetwork = System.Convert.ToInt32(networkText.text);
		networkText.text = (currentNetwork + currentStoryElement.networkBonus).ToString();

		int numberOfStoryElements = story.storyElements.Length;
		int level = Random.Range(0, numberOfStoryElements);
		currentStoryElement = story.storyElements[level];
		descriptionText.text = currentStoryElement.description;
	}
}
