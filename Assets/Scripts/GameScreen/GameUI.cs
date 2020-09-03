using System.Collections;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public Birb birb;
    public int score = 0;
    public int highscore = 0;
    public GameObject scoreCanvas;
    private CanvasGroup scoreGroup;
    public TextMeshProUGUI scoretext;
    public TextMeshProUGUI besttext;

    // Start is called before the first frame update
    void Start()
    {
        scoreGroup = scoreCanvas.GetComponent<CanvasGroup>();
    }

    public void incrementScore()
    {
        score++;
        scoretext.text = score.ToString();
    }

    public IEnumerator ded()
    { 
        if (highscore < score) highscore = score;
        score = 0;
        scoretext.text = highscore.ToString();
        besttext.transform.Translate(-10f, 0f, 0f);

        yield return new WaitForSeconds(0.75f);

        birb.respawn();
    }
}
