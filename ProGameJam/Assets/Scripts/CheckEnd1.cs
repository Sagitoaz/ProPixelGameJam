using UnityEngine;
using UnityEngine.Video;

public class CheckEnd1 : MonoBehaviour
{
    [SerializeField] VideoManager videoPlayer;
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int TotalAns = NPC.totalNoCount + NPC.totalYesCount;
            if (TotalAns != 5)
            {
                videoPlayer.PlayVideoED(videoPlayer.Ending1);
            }
        }
    }
}
