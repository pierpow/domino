using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Text descriptionText;

	private Story story;

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
		int numberOfStoryElements = story.storyElements.Length;
		int level = Random.Range(0, numberOfStoryElements);
		descriptionText.text = story.storyElements[level].description;
	}
}
