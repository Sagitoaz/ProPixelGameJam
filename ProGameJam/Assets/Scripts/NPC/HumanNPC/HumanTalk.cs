using UnityEngine;

public class HumanTalk : NPCInterface
{
    protected override void Talk()
    {
        Debug.Log("Human is talking...");
        HumanTalkBox.Instance.ShowDialogue("Human is talking...");
    }
}