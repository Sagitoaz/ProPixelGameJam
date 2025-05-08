using UnityEngine;

public class KnightTalk : NPCInterface
{
    protected override void Talk()
    {
        Debug.Log("Knight is talking...");
        KnightTalkBox.Instance.ShowDialogue("Knight is talking...");
    }
}