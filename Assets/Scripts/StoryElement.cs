using System;

[Serializable]
public class StoryElement
{
    public int id;
    public string description;
    public int networkBonus;
    public int dangerAmount;
    public string consequenceDescription;
    public string picture;
    public StoryPrerequisite prerequisites;
    public int unlocksElementId;
}