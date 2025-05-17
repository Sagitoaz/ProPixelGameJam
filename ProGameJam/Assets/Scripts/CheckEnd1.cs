using UnityEngine;

public class CheckEnd1 : MonoBehaviour
{
    void OggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int TotalAns = NPC.totalNoCount + NPC.totalYesCount;
            if (TotalAns == 5)
            {
                //ED1
            }
        }
    }
}
