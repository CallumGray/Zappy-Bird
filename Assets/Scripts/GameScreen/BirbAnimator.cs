using System.Collections;
using UnityEngine;

public class BirbAnimator : MonoBehaviour
{
    public Sprite[] sprites;
    public AudioClip flapSound;
    private AudioSource flapSource;
    public string birbName;
    public string birbFlavour;
    private SpriteRenderer sr;
    private bool initiallyFlipped;
    public Color[] colours;

    public void faceRight()
    {
        sr.flipX = initiallyFlipped;
    }

    public void flip()
    {
        sr.flipX = !sr.flipX;
    }

    public void flap()
    {
        StopAllCoroutines();
        StartCoroutine(animateFlap());
        flapSource.Play();
    }

    private IEnumerator animateFlap()
    {
        for(int i=1;i<sprites.Length;i++)
        {
            sr.sprite = sprites[i];
            yield return new WaitForSeconds(0.05f);
        }
        sr.sprite = sprites[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        flapSource = gameObject.AddComponent<AudioSource>();
        flapSource.clip = flapSound;
        flapSource.playOnAwake = false;
        flapSource.volume = 0.15f;

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprites[0];
        initiallyFlipped = sr.flipX;
    }
}
