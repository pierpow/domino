using System;

[Serializable]
public class StoryElement
{
    public int id;
    public string description;
    public int networkBonus;
    public int dangerAmount;
    public string consequenceDescription;
    public string inactionConsequenceDescription;
    public string picture;
    public string gameOverMessage;
    public StoryPrerequisite prerequisites;
    public int unlocksElementId;
}