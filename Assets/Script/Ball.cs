using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{   
    public Text Text_P1;
    private int placarP1 = 0;
    public Text Text_P2;
    private int placarP2 = 0;

    public float velocity;
    public GameObject wall_L;
    public GameObject wall_R;
    float x ;
    float y ;
    public float  error_P1=0;
    public float  error_P2 = 0;
    public GameObject player1;
    public GameObject player2;

    public float force_ball = 5f;

    void Start()
    {   
        x = BipolarStep( Random.Range(-2,2));
        GetComponent<Rigidbody2D>().velocity = new Vector2 (x * velocity,  velocity);

        InvokeRepeating("InpuseBall", 0f, force_ball);
    }

    private void OnCollisionEnter2D(Collision2D ball) {
        if (ball.gameObject.tag == wall_L.tag)
        {   
            error_P1 = player1.GetComponent<Transform>().position.y - transform.position.y;
            RandomMoveBall();
            placarP1++;
            Text_P1.text = "" + placarP1;

            player1.GetComponent<PlayerIA>().BackPropagationGeral("p2");
                                                                
        }
        if (ball.gameObject.tag == wall_R.tag)
        {
            error_P2 = player2.GetComponent<Transform>().position.y - transform.position.y;
            RandomMoveBall();
            placarP2++;
            Text_P2.text = "" + placarP2;  

            player2.GetComponent<PlayerIA>().BackPropagationGeral("p2");                                                           
        }
        if (ball.gameObject.tag == "Player")
        {
            RandomMoveBall();  
        }
    }


    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Space))
            InpuseBall();
       
   }
    public float BipolarStep(float x){
        if (x < 0)
            return -1f;
        else
            return  1f;
    
    }
    void InpuseBall()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(velocity, GetComponent<Rigidbody2D>().velocity.y);
    }

    void RandomMoveBall()
    {
        y = Random.Range(-10f,10f);
        GetComponent<Rigidbody2D>().velocity = new Vector2 (GetComponent<Rigidbody2D>().velocity.x,y);
    }

}