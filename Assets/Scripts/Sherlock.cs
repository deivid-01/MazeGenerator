using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sherlock : MonoBehaviour
{
    // Start is called before the first frame update
    
    public Transform transform;


    public static Sherlock Instance;
    private void Awake()
    {
        
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Asignar destino
    }

    public  void Move(Vector3 destination)
    {

        transform.Translate(destination);
    }

    


}
