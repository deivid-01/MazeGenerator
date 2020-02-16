using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlidesController : MonoBehaviour
{
   public  Transform camera;


    private void Start()
    {
        float previewPosition = PlayerPrefs.GetFloat("PositionX", 0); // Load the Camera position when Scene has changed
        Debug.Log(previewPosition);
        if (previewPosition != 0)
        {
            Vector3 aux = camera.position;
            aux.x = 0;
            camera.position = aux;

            camera.position += Vector3.right * previewPosition;
        };
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // next Slide
        {
            
            if ((camera.position + Vector3.right * 201).x>-144)
            {
                PlayerPrefs.SetFloat("PositionX", camera.position.x);
                SceneManager.LoadScene("Maze");
            }
            else
                camera.position += Vector3.right * 201; 

        }
        else if (Input.GetMouseButtonDown(0)) // preview Slide
        {
            if(camera.position.x>-1348)
                camera.position -= Vector3.right * 201;
        }


      //  PlayerPrefs.DeleteAll();

    }
}
