using UnityEngine;

public class MushroomTalk : NPCInterface
{
    protected override void Talk()
    {
        Debug.Log("Mushroom is talking...");
        MushroomTalkBox.Instance.ShowDialogue("Mushroom is talking...");
    }
}