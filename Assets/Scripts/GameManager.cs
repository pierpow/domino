using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Text descriptionText;

	void Awake()
	{
		Story story = new Story();
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, "story.json");
		Debug.Log(filePath);

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath); 
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            story = JsonUtility.FromJson<Story>(dataAsJson);
			// id = storyData.id;
			// description = storyData.description;
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
		// print(story.id);
		descriptionText.text = story.description;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
