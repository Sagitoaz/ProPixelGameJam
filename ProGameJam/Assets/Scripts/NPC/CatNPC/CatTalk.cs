using UnityEngine;

public class CatTalk : NPCInterface
{
    protected override void Talk()
    {
        Debug.Log("Cat is talking...");
        CatTalkBox.Instance.ShowDialogue("Cat is talking...");
    }
}