using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
   public  int i,j;

    GameObject wall;

    bool[] activeWalls={ true,true,true,true};

    public bool visited = false;
    public bool drawn = false;
    public int cols;

    //public GameObject sherlock;
    public GameObject sherlock;

    public Vector3 position;

    private void Awake()
    {
        position = new Vector3(0, 0, 0);
    }


    private void Start()
    {
        
        

    }

    private void Update()
    {

       

    }


    public  Cell(int i, int j,GameObject sherlock)
    {
        this.i = i;
        this.j = j;
        this.sherlock = sherlock;
    }

    public void Show(GameObject wal,GameObject sphere,GameObject sherlock,int cols)
    {
        this.cols = cols;
         //Show the wire of each cell

        wall = wal;

        position.y = 0.5f;
        position.z = 100 * cols;
        ShowSphereWire(sphere);

        if (activeWalls[0])
            ShowLeftSide();
        
        if (activeWalls[1])
            ShowRightSide();
        if (activeWalls[2])
            ShowUpSide();
        if (activeWalls[3])
                ShowDownSide();
        position.z += 0.5f;
  



    }
    void ShowLeftSide()
    {
        position.x = this.j;
        position.z = -(this.i + 0.5f);

        Instantiate(wall, position, Quaternion.identity);
    }

    void ShowRightSide()
    {
        position.x += 1;
        if(this.j==cols-1)
            Instantiate(wall, position, Quaternion.identity);
    }
    void ShowUpSide()
    {
        position.x -= 0.5f;
        position.z += 0.5f;
        if(this.i==0)
            Instantiate(wall, position, Quaternion.Euler(0, 90, 0));   
    }
    void ShowDownSide()
    {
        position.z -= 1;

        Instantiate(wall, position, Quaternion.Euler(0, 90, 0));
    }

    void ShowSphereWire(GameObject sphere)
    {
        //1. Create Sphere
        position.z -= (this.i+0.5f);
        position.x = this.j+ 0.5f;
        Instantiate(sphere, position, Quaternion.identity);
    }


   
}
