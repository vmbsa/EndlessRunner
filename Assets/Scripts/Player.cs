using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed;
    public float lanespeed;
    public float jumpLenght;
    public float jumpHeight;
    public float slideLenght;
    public int maxLife = 3;
    public float minSpeed = 10f;
    public float maxSpeed = 30f;
    public float invencibleTime;
    public GameObject model;
    private float meanS = 1f;
    private float varianceS = 0.5f;

    private BoxCollider box;
    private Animator anim;
    private Rigidbody rb;
    private UIManager uimanager;

    private Vector3 verticalTargetPosition;
    private Vector3 boxSize;

    private int currentLane = 1;
    private int currentLife = 1;
    static int blinkingValue;
    private int coins;

    private bool jumping = false;
    private bool sliding = false;
    private bool invencible = false;
    private bool powerUpActive = false;
    private bool powerUpActiveX2 = false;



    private float jumpStart;
    private float slideStart;
    private float score;
    private float timeRemaining = 5;
    private float steps = 0;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        box = GetComponent<BoxCollider>();
        boxSize = box.size;
        anim.Play("runStart");
        currentLife = maxLife;
        speed = minSpeed;
        blinkingValue = Shader.PropertyToID("_BlinkingValue");
        uimanager = FindObjectOfType<UIManager>();

    }

    // Update is called once per frame
    void Update()
    {
        steps++;
        if (!powerUpActiveX2)
        {
            score += GetGaussian(meanS,varianceS) * Time.deltaTime * speed ;
            print(GetGaussian(meanS, varianceS));

        }
        else
        {
            score += GetGaussian(meanS, varianceS) * Time.deltaTime * speed * 2; 
        }
        uimanager.UpdateScore((int)score);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ChangeLane(1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Slide();
        }

        if (jumping)
        {
            float ratio = (transform.position.z - jumpStart) / jumpLenght;
            if (ratio >= 1f)
            {
                jumping = false;
                anim.SetBool("Jumping", false);
            }
            else
            {
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;
            }

        }
        else
        {
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);
        }

        if (sliding)
        {

            float ratio = (transform.position.z - slideStart) / slideLenght;
            if (ratio >= 1)
            {
                sliding = false;
                anim.SetBool("Sliding", false);
                box.size = boxSize;
            }

        }

        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, lanespeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.forward * speed;
    }

    void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;
        if (targetLane < 0 || targetLane > 2)
            return;
        currentLane = targetLane;
        verticalTargetPosition = new Vector3((currentLane - 1), 0, 0);
    }

    void Jump()
    {
        if (!jumping)
        {
            jumpStart = transform.position.z;
            anim.SetFloat("JumpSpeed", speed / jumpLenght);
            anim.SetBool("Jumping", true);
            jumping = true;
        }
    }

    void Slide()
    {
        if (!jumping && !sliding)
        {
            slideStart = transform.position.z;
            anim.SetFloat("JumpSpeed", speed / slideLenght);
            anim.SetBool("Sliding", true);
            Vector3 newSize = box.size;
            newSize.y = newSize.y / 2;
            box.size = newSize;
            sliding = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Coin"))
        {
            coins++;
            uimanager.Updatecoins(coins);
            other.transform.parent.gameObject.SetActive(false); // desativar a moeda

        }

        if (other.CompareTag("Colectables"))
        {
            other.transform.parent.gameObject.SetActive(false);
            uimanager.MagnetPanel.SetActive(true);
            powerUpActive = true;

        }

        if (other.CompareTag("X2"))
        {
            other.transform.parent.gameObject.SetActive(false);
            powerUpActiveX2 = true;
            uimanager.X2Panel.SetActive(true);
        }

        if (powerUpActiveX2)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime * 60;
            }
            else
            {
                uimanager.X2Panel.SetActive(false);
                powerUpActiveX2 = false;

            }
        }




        if (powerUpActive)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime * 60;
            }
            else
            {
                powerUpActive = false;
                uimanager.MagnetPanel.SetActive(false);
                //coin.SetActive(false);

            }
        }

        if (invencible)
            return;

        if (other.CompareTag("Obstacle"))
        {
            currentLife--;
            uimanager.UpdateLifes(currentLife);
            anim.SetTrigger("Hit");
            speed = 0;
            if (currentLife <= 0)
            {
                speed = 0;
                anim.SetBool("Dead", true);
                uimanager.gameOverPanel.SetActive(true);
                GameManager.gm.Save();
                Invoke("CallMenu", 2f);
            }
            else
            {
                StartCoroutine(Blinking(invencibleTime));
            }

        }
    }

    IEnumerator Blinking(float time)
    {
        invencible = true;
        float timer = 0;
        float currentBlink = 1f;
        float lastBlink = 0;
        float blinkPeriod = 0.1f;
        bool enabled = false;
        yield return new WaitForSeconds(1f);
        speed = minSpeed;
        while (timer < time && invencible)
        {
            model.SetActive(enabled);

            yield return null;
            timer += Time.deltaTime;
            lastBlink += Time.deltaTime;
            if (blinkPeriod < lastBlink)
            {
                lastBlink = 0;
                currentBlink = 1f - currentBlink;
                enabled = !enabled;
            }
        }
        model.SetActive(true);

        invencible = false;
    }

    void CallMenu()
    {
        GameManager.gm.coins += coins;
        GameManager.gm.Save();
        GameManager.gm.EndRun();
    }

    public void IncreaseSpeed() // ate agora aumenta 15% a velocidade sempre que chega ao fim do mapa
    {
        speed *= 1.15f;
        if (speed >= maxSpeed)
            speed = maxSpeed;
    }

    /* public double parabolic ( double xMin, double xMAx)
     {
         if(xMin < xMAx)
         {
             double a = 0.5 * (xMin * xMAx);
             double yMAx = Mathf.
         }
         else
         {
             //erro
         }

     }*/

    

    public float ScoreMultiplier() {
        if (steps < 10000)
        {

            float random = Random.Range(0.0f, 1.0f);
            if(random <= 0.7f)
            {
                //vale 1
            }else if(0.7f < random && random <= 0.9f)
            {
                //vale 2
            }
            else
            {
                //vale 3
            }
            float mu = 0.5f;
            float sigma = 1f;

            float p = 100f, p1 = 100f, p2;
            while (p >= 1.0f)
            {
                p1 = Uniform(Random.Range(-1.0f,0.0f), Random.Range(0.0f, 1.0f));
                p2 = Uniform(Random.Range(-1.0f, 0.0f), Random.Range(0.0f, 1.0f));
                p = p1 * p1 + p2 * p2;
            }
            return mu + sigma * p1 * Mathf.Sqrt(-2.0f * Mathf.Log10(p) / p);
        }
        else
            return 1;
        }



    public float Uniform( float xMin, float xMax)
    {
        return 1 / (xMax - xMin);
    }

    public float GetGaussian(float mean, float variance)
    {
        float random1 = Random.Range(0f, 1f);
        float random2 = Random.Range(0f, 1f);
        Debug.Log("RD1" + random1 + "RD2" + random2);

        float pi = Mathf.PI;
        const float e = 2.71828182845905f;

        float lnU1 = Mathf.Log(e, random1);
        float cos = Mathf.Cos(2 * pi * random2);

        float random = Mathf.Sqrt(-2 * lnU1) * cos;

        float result = mean + random * variance;

        if (result < 0)
            return 0.05323f;
        else
            return result;
    }


}
