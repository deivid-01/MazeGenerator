using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject sphereObj;
    public MeshRenderer sphereRenderer;

    public Animator animator;
    void Start()
    {
        sphereObj.GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //ChangeColor
       
        sphereRenderer.material.color = Color.black;
        sphereObj.GetComponent<Animator>().enabled = true;
    }
}
