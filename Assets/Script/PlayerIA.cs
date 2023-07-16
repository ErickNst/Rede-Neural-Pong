using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerIA : MonoBehaviour
{   
    private float[] InputLayer = new float[4];
    public bool firstPlayer;

    // private float[] weights11;
    // public float[] HiddenOutput11;
    // public float[] weights22;

    // public float[] HiddenOutput22;
    // public float[] weights00;

    // public float[] OutPutOutput00;
    
    public float SpeedPlayer;
    public GameObject Ball;
    public Transform reference;
    public float alpha;

    [SerializeField]
    public class neuron //: PlayerIA
    {   
        public float alpha = 0.001f;
        public float[] neurons_out;
        public float[] input_Layer;
        public List<float[]> Weights_List = new List<float[]>();

        //MUDAR O CONTRUTOR PARA RECEBER APENAS UM INT AO INVES DE UM ARRAY FLOAT
        public neuron(float[] input_layer, int size_Layer)
        {
            neurons_out = new float[size_Layer];
            input_Layer = input_layer;
            CreateWeigths();
        }

    void CreateWeigths(){
        float [] auxList;
        for (int i = 0; i < neurons_out.Length ; i++)
        {   
            auxList = new float[input_Layer.Length];
            auxList = fillRandom(auxList);
            Weights_List.Add(auxList);
        }
    }
    float[] fillRandom (float[] vectorRand ){
        for (int i = 0; i < vectorRand.Length; i++){
            vectorRand[i] = Random.Range(-1f, 1f);
        }
        return vectorRand;
    }
    public void BipolarStep(float[] x){
        for(int i=0; i< x.Length; i++)
        {      
        if (x[i] < 0)
            x[i]= -1f;
        else
            x[i]=  1f;
        }

    }
    public void TangHiper(float[] x){
        for(int i=0; i< x.Length; i++)
        {      
            x[i] = (float)System.Math.Tanh(x[i]);
        }
    }
    
    void UpdateInputs(float [] input)
    {
        input_Layer =  input;
    }
    public void Foward(float[] input, string stepFunction = ""){
        UpdateInputs(input);

        for( int i=0; i < neurons_out.Length; i++)
        {   for(int j = 0; j < input_Layer.Length ; j++ )
            {
                neurons_out[i] += Weights_List[i][j] * input_Layer[j];
            } 
        }
        if (stepFunction == "TangHiper" )  
        {
            TangHiper(neurons_out);
        }
        if (stepFunction == "BipolarStep" )  
        {
            BipolarStep(neurons_out);
        }
                
    }

    public void PrintMatrix(List<float[]> printer_List)
    {  
         Debug.Log("//=========================================================//");
        for (int i = 0; i < printer_List.Count; i++)
        {   
            string listaString = string.Join(" || ", printer_List[i]);
            Debug.Log("Weight: "+ i +" -----> "+ listaString);
        }
    }   
    public void PrintArray(float[] printer_List)
    {  
         Debug.Log("//=========================================================//");
        {   
            string listaString = string.Join(" || ", printer_List);
            Debug.Log("Array -----> "+ listaString);
        }
    }    
    public void BackPropagation(float erro){

        for(int i =0; i < neurons_out.Length;i++)
            for(int j =0; j < Weights_List[i].Length ;j++)
                Weights_List[i][j] += input_Layer[j] * alpha * erro;
    }


    }
    public neuron Hidden1 ;
    public neuron Hidden2 ;
    public neuron OutPut ;

    void Start() {
        UpdateInputs();
        Hidden1 = new neuron( InputLayer, 8);

        Hidden2 = new neuron(Hidden1.neurons_out,4);

        OutPut = new neuron(Hidden2.neurons_out,1);
    }
    void Update() {
        UpdateInputs();
        Hidden1.Foward(InputLayer,"TangHiper");
        Hidden2.Foward(Hidden1.neurons_out,"TangHiper");
        //OutPut.Foward(Hidden2.neurons_out,"BipolarStep");
        OutPut.Foward(Hidden2.neurons_out,"TangHiper");

        MoveIA(OutPut.neurons_out[0]);

        // if(Input.GetKeyDown(KeyCode.W))
        //     GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,SpeedPlayer);
        // if(Input.GetKeyDown(KeyCode.S))
        //     GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,-SpeedPlayer);


    
        // HiddenOutput11 = Hidden1.neurons_out;
        // weights11 = Hidden1.Weights_List[0];

        // HiddenOutput22 = Hidden2.neurons_out;
        // weights22 = Hidden2.Weights_List[0];

        // OutPutOutput00 = OutPut.neurons_out;
        // weights00 = OutPut.Weights_List[0];
    }
    void UpdateInputs(){
        InputLayer[0] = (Ball.transform.position.x)/reference.position.x;
        InputLayer[1] = (Ball.transform.position.y)/reference.position.y;
        InputLayer[2] = (transform.position.y)/reference.position.y;
        InputLayer[3] = (1);
    }
    void MoveIA(float output_IA)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, output_IA * SpeedPlayer);
    }
    public void BackPropagationGeral(string p)
    {
        if (p == "p1"){
            Hidden1.BackPropagation( Ball.GetComponent<Ball>().error_P1 );
            Hidden2.BackPropagation( Ball.GetComponent<Ball>().error_P1 );
            OutPut.BackPropagation( Ball.GetComponent<Ball>().error_P1  );

        }
         else{
            Hidden1.BackPropagation( Ball.GetComponent<Ball>().error_P2 );
            Hidden2.BackPropagation( Ball.GetComponent<Ball>().error_P2 );
            OutPut.BackPropagation( Ball.GetComponent<Ball>().error_P2  );
        }

    }
}
 
