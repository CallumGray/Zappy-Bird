using System.Collections.Generic;
using UnityEngine;

public class Birb : MonoBehaviour
{
    public enum State{ready,playing,dead,respawn}

    private GameObject birbAnimator;
    private BirbAnimator birbAnimatorScript;
    public GameObject birb;

    private State state;
    public GameObject flash;
    public GameObject dedfeather;
    private List<DeadFeather> dedfeathers;

    private float width;
    private float height;
    private CircleCollider2D box;
    private Rigidbody2D rb;
    private Main main;

    private float flySpeed = 3.5f;
    private float flyBounceSpeed = 7f;
    private float flyDeceleration = 20f;

    private float gravity = -50f;
    private float jumpSpeed = 12.5f;
    private Vector2 velocity = Vector2.zero;
    
    private GameUI gameUI;
    public GameObject tap;
    private Animator tapAnimator;

    public AudioClip flap;
    private AudioSource flapSource;

    public AudioClip plusOne;
    private AudioSource plusOneSource;

    public AudioClip zap;
    private AudioSource zapSource;

    private CameraShake camShake;

    // Start is called before the first frame update
    void Start()
    {
        camShake = Camera.main.GetComponent<CameraShake>();
        main = Camera.main.GetComponent<Main>(); 
        box = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        gameUI = GetComponent<GameUI>();

        tapAnimator = tap.GetComponent<Animator>();

        flapSource = gameObject.AddComponent<AudioSource>();
        flapSource.clip = flap;
        flapSource.playOnAwake = false;
        flapSource.volume = 0.15f;

        zapSource = gameObject.AddComponent<AudioSource>();
        zapSource.clip = zap;
        zapSource.playOnAwake = false;
        zapSource.volume = 1f;

        birbAnimator = Instantiate(birb);
        birbAnimatorScript = birbAnimator.GetComponent<BirbAnimator>();

        dedfeathers = new List<DeadFeather>();

        for(int i=0;i<50;i++)
        {
            GameObject featherz = Instantiate(dedfeather,Vector3.left * 10f,Quaternion.identity);
            featherz.GetComponent<SpriteRenderer>().color = birbAnimatorScript.colours[i % birbAnimatorScript.colours.Length];
            dedfeathers.Add(featherz.GetComponent<DeadFeather>());
        }

        plusOneSource = gameObject.AddComponent<AudioSource>();
        plusOneSource.clip = plusOne;
        plusOneSource.playOnAwake = false;
        plusOneSource.volume = 0.75f;

        width = box.bounds.max.x - box.bounds.min.x;
        height = box.bounds.max.y - box.bounds.min.y;
        
        state = State.ready;
    }
    
    private void zapSound()
    {
        zapSource.Play();
    }

    private void plusOneSound()
    {
        plusOneSource.Play();
    }

    private void flapSound()
    {
        flapSource.Play();
    }

    public void startFlight()
    {
        jump();
        velocity.x = flySpeed;
        tap.transform.Translate(10f, 0f, 0f);
        state = State.playing;
        gameUI.besttext.transform.Translate(10f, 0f, 0f);
        gameUI.scoretext.text = gameUI.score.ToString();
    }

    private bool pointInRT(Vector2 point, RectTransform rt)
    {
        // Get the rectangular bounding box of your UI element
        Rect rect = rt.rect;

        // Get the left, right, top, and bottom boundaries of the rect
        float leftSide = rt.anchoredPosition.x - rect.width / 2;
        float rightSide = rt.anchoredPosition.x + rect.width / 2;
        float topSide = rt.anchoredPosition.y + rect.height / 2;
        float bottomSide = rt.anchoredPosition.y - rect.height / 2;

        // Check to see if the point is in the calculated bounds
        return point.x >= leftSide &&
            point.x <= rightSide &&
            point.y >= bottomSide &&
            point.y <= topSide;
    }

    // Update is called once per frame
    void Update()
    {
        print(state);

        if (state != State.dead)
        {
            float dt = Time.deltaTime;

            if (state != State.respawn)
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);

                    if (touch.phase == TouchPhase.Began)
                    {
                        if (state == State.ready)
                        {
                            startFlight();
                        }
                        else
                        {
                            jump();
                        }
                    }
                }
                else if (Input.GetMouseButtonDown(0))
                {
                    if (state == State.ready)
                    {
                        startFlight();
                    }
                    else
                    {
                        jump();
                    }
                }
            }

            if (state == State.ready)
            {
                if (transform.position.y < -1f)
                {
                    state = State.ready;
                    if (!tapAnimator.GetCurrentAnimatorStateInfo(0).IsName("tap")) tapAnimator.SetTrigger("tap");
                    transform.position = new Vector3(transform.position.x, -1f, transform.position.z);
                    jump();
                }
            }

            if (state == State.respawn)
            {
                if (transform.position.y < -1f)
                {
                    tap.transform.Translate(-10f, 0f, 0f);
                    state = State.ready;
                    if (!tapAnimator.GetCurrentAnimatorStateInfo(0).IsName("tap")) tapAnimator.SetTrigger("tap");
                    transform.position = new Vector3(transform.position.x, -1f, transform.position.z);
                    jump();
                }
            }

            if(state != State.dead)
            {
                if(state != State.respawn)
                { 
                    //hit right
                    if (transform.position.x > main.width * 0.5f - width * 0.5f)
                    {
                        transform.position = new Vector2(main.width * 0.5f - width * 0.5f - 0.01f, transform.position.y);
                        velocity.x = -flyBounceSpeed;
                        birbAnimatorScript.flip();
                        gameUI.incrementScore();
                        plusOneSound();
                    }

                    //hit left
                    if (transform.position.x < -main.width * 0.5f + width * 0.5f)
                    {
                        transform.position = new Vector2(-main.width * 0.5f + width * 0.5f + 0.01f, transform.position.y);
                        velocity.x = flyBounceSpeed;
                        birbAnimatorScript.flip();
                        gameUI.incrementScore();
                        plusOneSound();
                    }

                    //hit bottom
                    if (transform.position.y < -main.height * 0.5f + height * 0.5f)
                    {
                        transform.position = new Vector2(transform.position.x, -main.height * 0.5f + height * 0.5f);
                        jump();
                    }

                    //hit top
                    if (transform.position.y > main.height * 0.5f - height * 0.5f)
                    {
                        transform.position = new Vector2(transform.position.x, main.height * 0.5f - height * 0.5f);
                        velocity.y = 0f;
                    }
                }
                if (velocity.x > flySpeed) velocity.x -= flyDeceleration * dt;
                if (velocity.x < -flySpeed) velocity.x += flyDeceleration * dt;
                if (velocity.x > 0 && velocity.x < flySpeed) velocity.x = flySpeed;
                if (velocity.x < 0 && velocity.x > -flySpeed) velocity.x = -flySpeed;

                velocity.y = velocity.y + gravity * dt;

                rb.velocity = velocity;
                
            }
        }

        birbAnimator.transform.position = transform.position;
    }
    
    private void jump()
    {
        velocity.y = jumpSpeed;
        birbAnimatorScript.flap();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Pain")
        {
            ded();
        }
    }

    private void ded()
    {
        for(int i=0;i<dedfeathers.Count;i++)
        {
            dedfeathers[i].gameObject.transform.position = transform.position;
            dedfeathers[i].explode();
        }

        flash.transform.position = Vector2.zero;
        StartCoroutine(camShake.shake(0.2f,0.5f));
        zapSound();
        flash.GetComponent<Animator>().SetTrigger("flash");

        transform.position = Vector2.zero;
        
        if(gameUI.highscore < gameUI.score)gameUI.highscore = gameUI.score;

        state = State.dead;
        rb.velocity = Vector2.zero;
        velocity = Vector2.zero;
        
        transform.Translate(new Vector2(10f, 0f));

        gameUI.StartCoroutine(gameUI.ded());
    }

    public void respawn()
    {
        state = State.respawn;

        transform.position = new Vector2(0f, 6f);
        velocity = Vector2.zero;
        birbAnimatorScript.faceRight();
    }
}
