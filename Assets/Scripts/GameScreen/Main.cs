using UnityEngine;

public class Main : MonoBehaviour
{
    public float width;
    public float height;

    private float maskHeight;
    private float generatorHeight;
    
    public GameObject leftmask;
    public GameObject rightmask;
    public GameObject leftTopGenerator;
    public GameObject leftBotGenerator;
    public GameObject rightTopGenerator;
    public GameObject rightBotGenerator;

    public GameObject[] bolts;

    private float leftSpeed = 2f;
    private float rightSpeed = 2f;
    private float baseSpeed = 1.5f;
    private float speedVariance = 1f;
    private Vector2 leftDirection = Vector2.up;
    private Vector2 rightDirection = Vector2.up;

    // Start is called before the first frame update
    void Start()
    {
        //Get screen dimensions
        Camera cam = Camera.main;
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;

        //HEIGHTS LOL
        Bounds maskBounds = leftmask.GetComponent<MeshRenderer>().bounds;
        maskHeight = maskBounds.max.y - maskBounds.min.y;

        Bounds generatorBounds = leftBotGenerator.GetComponent<SpriteRenderer>().bounds;
        generatorHeight = generatorBounds.max.y - generatorBounds.min.y;

        //start bolts on random frame
        for (int i=0;i<bolts.Length;i++)
        {
            //have 20 separate animation and randomly jump between them!

            //flip half of them
            SpriteRenderer sr = bolts[i].GetComponent<SpriteRenderer>();
            sr.flipX = Random.value > 0.5f;
            sr.flipY = Random.value > 0.5f;
        }

        startingGenerators();
    }

    private void startingGenerators()
    {
        //random position / equal gaps / slightly random speed
        float yLeft = 0.8f * (Random.value * (height - maskHeight) - 0.5f * (height - maskHeight));
        float yRight = 0.8f * (Random.value * (height - maskHeight) - 0.5f * (height - maskHeight));

        leftDirection = Random.value > 0.5f ? Vector2.up : Vector2.down;
        rightDirection = Random.value > 0.5f ? Vector2.up : Vector2.down;

        leftSpeed = baseSpeed + Random.value * speedVariance;
        rightSpeed = baseSpeed + Random.value * speedVariance;

        leftmask.transform.position = new Vector3(leftmask.transform.position.x, yLeft, leftmask.transform.position.z);
        leftTopGenerator.transform.position = new Vector3(leftTopGenerator.transform.position.x, yLeft + maskHeight * 0.5f + generatorHeight * 0.5f, leftTopGenerator.transform.position.z);
        leftBotGenerator.transform.position = new Vector3(leftBotGenerator.transform.position.x, yLeft - maskHeight * 0.5f - generatorHeight * 0.5f, leftBotGenerator.transform.position.z);

        rightmask.transform.position = new Vector3(rightmask.transform.position.x, yRight, rightmask.transform.position.z);
        rightTopGenerator.transform.position = new Vector3(rightTopGenerator.transform.position.x, yRight + maskHeight * 0.5f + generatorHeight * 0.5f, rightTopGenerator.transform.position.z);
        rightBotGenerator.transform.position = new Vector3(rightBotGenerator.transform.position.x, yRight - maskHeight * 0.5f - generatorHeight * 0.5f, rightBotGenerator.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        //generators hitting boundaries
        if (leftTopGenerator.transform.position.y - generatorHeight * 0.5f > height * 0.5f)
        {
            leftDirection = Vector2.down;
            leftSpeed = baseSpeed + Random.value * speedVariance;
        }

        if (leftBotGenerator.transform.position.y + generatorHeight * 0.5f < -height * 0.5f) 
        {
            leftDirection = Vector2.up;
            leftSpeed = baseSpeed + Random.value * speedVariance;
        }

        if (rightTopGenerator.transform.position.y - generatorHeight * 0.5f > height * 0.5f) 
        {
            rightDirection = Vector2.down;
            rightSpeed = baseSpeed + Random.value * speedVariance;
        }

        if (rightBotGenerator.transform.position.y + generatorHeight * 0.5f < -height * 0.5f) 
        {
            rightDirection = Vector2.up;
            rightSpeed = baseSpeed + Random.value * speedVariance;
        }

        leftmask.transform.Translate(leftDirection * leftSpeed * dt);
        leftBotGenerator.transform.Translate(leftDirection * leftSpeed * dt);
        leftTopGenerator.transform.Translate(leftDirection * leftSpeed * dt);

        rightmask.transform.Translate(rightDirection * rightSpeed * dt);
        rightBotGenerator.transform.Translate(rightDirection * rightSpeed * dt);
        rightTopGenerator.transform.Translate(rightDirection * rightSpeed * dt);
    }
}
