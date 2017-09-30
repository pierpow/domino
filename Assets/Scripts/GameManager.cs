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

		descriptionText.text = story.storyElements[1].description;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
