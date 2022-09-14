using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Random = UnityEngine.Random;
public class GridGenerator : MonoBehaviour
{
    public GameObject pivote1;

    [Header("Inputs")]

    public Text colsInput;
    public Text rowsInput;
    public Text speedInput;


    [Header("Prefabs")]
    public GameObject endSlide;
    public GameObject input;
    public GameObject line;
    public GameObject sphere;
    public GameObject ground;
    public GameObject wall;
    public GameObject sherlockObj;
    public GameObject twinSherlock;
    public GameObject sherlockAnimation;

    [Space]

    [Header ("Grid Propierties")]
     public int cols;
     public int rows;

    // Cell manager variables
    Cell cell;
    List <Cell> grid =new List<Cell>();
    Vector3 groundPosition;
    Vector3 groundScale;
    Vector3 linePosition;

    Sherlock sherlock;

    Cell  current; //Celda actual.

    public float speed;
    public float distanciaMinima;
    int mayor;
    public Stack pila = new Stack();
    Cell startCell;

    List<Vector3> cameraPosition = new List<Vector3>(); //Para estar moviendo la camara
    List<Cell> sideCell = new List<Cell>();
    int  actualPosition; //actual posicion de la camara

    #region Camera Controller Variables
    public Transform  rotationCamera;
    public Camera camera;

    [Range(0,500)]

    public int speedRotation;


    float  mayorLado;
    float  menorLado;
    float maxSize;
    float minSize;
    float mayorActualLado;
    float  porcentajeSize;


    #endregion

    bool startMaze;

    int hold;

    private void Awake()
    {
        mayor = 0;
        
    }
    private void Start()
    {
        endSlide.SetActive(false);
        actualPosition = 0;
        hold = 0;
        startMaze = false;
        mayorLado = 30;
        menorLado = 2;
        maxSize = 13;
        minSize = 2;

        sherlock = Sherlock.Instance;



    }

    private void Update()
    {
        StartMaze();

        if (startMaze)
        {
            MoveCamera();
            if (!(current is null))
            {    // The Cell has been visited

                sherlockObj.transform.position = Vector3.MoveTowards(sherlockObj.transform.position, current.position, speed * Time.deltaTime);
                //Gemelo que se mueve igual a sherlock pero en el wire
                twinSherlock.transform.position = Vector3.MoveTowards(twinSherlock.transform.position, current.position + new Vector3(0, 0, 100 * cols), speed * Time.deltaTime);
                if (Vector3.Distance(current.position, sherlockObj.transform.position) < distanciaMinima)


                {

                    pila.Push(current);
                    if (pila.Count > mayor)
                    {
                        mayor = pila.Count;
                        startCell = (Cell)pila.Peek();
                    }

                    grid[current.j + current.i * cols].visited = true;


                    //   sherlockObj.transform.position = current.position;

                    current = CheckNeighbors(current);
                }



            }


            else if (pila.Count > 0)
            {
                pila.Pop();
                if (pila.Count > 0)
                    current = (Cell)pila.Pop();

            }
            else
            {

            }
        }
        else {
            SlidesControl();
        }
    }
    void StartMaze()
    {
     
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (startMaze)
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //Disable Input
            input.SetActive(false);
            //Read Values
            cols = int.Parse(colsInput.text.ToString());
            rows = int.Parse(rowsInput.text.ToString());
            speed = int.Parse(speedInput.text.ToString());
            //Create Maze

            GetBiggerSide();

            CreateCells();

            ShowGround();

            DisplayCells();
            DisplayLineWhire();
            SetCameraPositions();
            sherlockAnimation.SetActive(false);

            //Time.timeScale = 0;

            startMaze = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
    void GetBiggerSide()
        {
        if (cols > rows)
        {
            mayorActualLado = cols;

        }
        else
            mayorActualLado = rows;
    }
    void CreateCells()
    {
        for (int i = 0; i < rows; i++)
        { 
            for (int j = 0; j < cols; j++)
            {
                cell = new Cell(i, j,sherlockObj);
                grid.Add(cell);
            }
        }


    
        current = grid[0];
        current.visited = true;
        
        current.position.x = current.j + 0.5f;
        current.position.z =-(current.i+1f);
        current.position.y = 0.5f;


        #region Activar gameObjects
        sherlockObj.transform.position = current.position;
        sherlockObj.SetActive(true);
        //Ubicar en la wire el otro sherlock
        twinSherlock.transform.position = new Vector3(0.5f, 0.5f,100* cols);
        twinSherlock.SetActive(true);
        #endregion
        current.position = Vector3.zero;

        CameraController();

       

    }
    void SetCameraPositions()
    {


        cameraPosition.Add(rotationCamera.transform.position);
        cameraPosition.Add(rotationCamera.transform.position+Vector3.forward* 100 * cols);

    }

    void DisplayCells()
    {
        
        for (int i = 0; i < grid.Count; i++)
        {
            grid[i].Show(wall,sphere,sherlockObj,cols);
        }
        


    }
    void ShowGround()
    {
        FixValues();     
        Instantiate(ground, groundPosition,Quaternion.identity);
    }

    void FixValues()
    {
        groundScale = new Vector3(cols, 0.5f, rows);
        ground.transform.localScale = groundScale;
        groundPosition.x = cols / 2;
        groundPosition.z = -rows / 2;
    }
    
    Cell CheckNeighbors(Cell actual)
    {
        List<Cell> neighBors = new List<Cell>();
      


        Cell leftN = GetIndex(actual.i, actual.j - 1);
        Cell rightN = GetIndex(actual.i, actual.j + 1);
        Cell upN = GetIndex(actual.i-1, actual.j);
        Cell downN = GetIndex(actual.i+1, actual.j);


        if (!(leftN is null) && leftN.visited == false)
        {
            neighBors.Add(leftN);


        }

        if (!(upN is null) && upN.visited == false)
        {
            neighBors.Add(upN);

        }

       
         if (!(rightN is null) && rightN.visited == false)
        {
            neighBors.Add(rightN);
            
        }
         
        if (!(downN is null) && downN.visited == false)
        {
            neighBors.Add(downN);

        }

     
        int random = Random.Range(0, neighBors.Count);
        
        if (neighBors.Count>0)
            return neighBors[random];
        return null;

    }

    Cell GetIndex(int i , int j)
    {
        if(i>=0 && j >=0  && i<rows && j<cols)
            return grid[j + i * cols];
        return null;
    }

    void PrintNeighBors(List<Cell> neighBors)
    
    {
      
    }

    void CreateSideCells()

    {
        sideCell.Add(grid[0]);
        sideCell.Add(grid[cols - 1]);
        sideCell.Add(grid[(rows-1)*cols]);
        sideCell.Add(grid[grid.Count - 1]);
       
     }

    void CameraController()
    {
        pivote1.transform.position = Vector3.back * rows / 2f + Vector3.right * cols / 2f;

        SizeView();
    }

    void SizeView()
    {
        //Get sizePorcentaje;

        porcentajeSize = (mayorActualLado-menorLado)/ (mayorLado - menorLado) ;
       
         camera.orthographicSize= minSize+(porcentajeSize * (maxSize - minSize));

    }

    void MoveCamera()
    {





        if (Input.GetMouseButtonDown(1))
        {
            actualPosition = (actualPosition + 1) % cameraPosition.Count;
            rotationCamera.transform.position = cameraPosition[actualPosition];

        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (actualPosition - 1 < 0)
            {
                actualPosition = (actualPosition + 1) % cameraPosition.Count;
            }
            else
                actualPosition = (actualPosition - 1) % cameraPosition.Count;

            rotationCamera.transform.position = cameraPosition[actualPosition];



        }

      
        else if (Input.GetMouseButtonDown(2))
        {
            if (Time.timeScale == 1) // Pause Game
                Time.timeScale = 0;
            else //Continue game
                Time.timeScale = 1;
        }
        
            Vector3 temp = rotationCamera.transform.rotation.eulerAngles;
            temp.y += speedRotation * Time.deltaTime * Input.mouseScrollDelta.y;
        

            rotationCamera.rotation = Quaternion.Lerp(rotationCamera.rotation, Quaternion.Euler(temp), speedRotation*Time.deltaTime);

        


        //speed* Vector3.left* Input.GetAxis("Mouse Y") * Time.deltaTime

       
    }

    void DisplayLineWhire()
    {
        //Display Vertical lines
        for (int i = 0; i < cols; i++)
        {
            
            //Vertical lines
            line.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0.5f+i*1, 0.5f, 100 * cols-0.5f));
            line.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0.5f + i * 1, 0.5f, 100 * cols-rows+0.5f));

            Instantiate(line,new Vector3(0,0,0),Quaternion.identity);
          
        }
        for (int j = 0; j < rows; j++)
        {
            //Horizontal Lines
            line.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0.5f , 0.5f, 100 * cols - 0.5f-j));
            line.GetComponent<LineRenderer>().SetPosition(1, new Vector3(cols -0.5f  , 0.5f, 100 * cols - 0.5f-j));

            Instantiate(line, new Vector3(0, 0, 0), Quaternion.identity);
            
        }
    }

    void SlidesControl()
    {
       
        if (Input.GetMouseButton(0)) // Preview Slide
        {
            hold++; //Hold for more than 10 seconds
            if (hold>10)
                SceneManager.LoadScene("Presentation");
        }
        else if (Input.GetMouseButtonDown(1)) // Next Slide
        {
            //Active Slide Panel 
          //  endSlide.SetActive(true);
        }
        else
            hold = 0;
    }
}


