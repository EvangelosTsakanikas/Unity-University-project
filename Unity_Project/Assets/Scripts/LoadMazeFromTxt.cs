using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;

[System.Serializable]
public class CubeTextures
{
    public GameObject T1;
    public GameObject T2;
    public GameObject T3;
    public GameObject W;
}

public class LoadMazeFromTxt : MonoBehaviour
{
    public GameObject secondaryCameraGameObject;
    public GameObject player;
    public GameObject plane;
    public Image endOfGameImage;
    public CubeTextures cubeTextures;   
    public static int K;

    private int L, N;
    private int startHammers;
    private bool isKey_R_pressed;
    private bool isCameraRotating = false;
    private bool gameEnded = false;
    private bool isTransparent = true;
    private string[] TxtFileLines;
    private float scrollingVariety = 1;
    private float scalingFactor = 4f;
    private float scaleMultiplier;    
    private Color endOfGameBackground = new Color(0f, 0f, 0f, 1f);
    private Vector3 startingCameraPosition;
    private List<Vector3> wormHolesList;
    private Camera secondaryCamera;
    private GameObject cylinder;
    private Shader shaderForTransparency;
    private Shader shaderWithoutTransparency;    

    void Start()
    {
        //shaderForTransparency = Shader.Find("Sprites/Diffuse");
        shaderForTransparency = Shader.Find("Transparent/Diffuse");
        //shaderForTransparency = Shader.Find("Standard");
        shaderWithoutTransparency = Shader.Find("Standard");

        secondaryCamera = secondaryCameraGameObject.GetComponent<Camera>();
        startingCameraPosition = secondaryCamera.transform.position;
        cylinder = GameObject.FindWithTag("Cylinder");

        ReadTxtFile();
        initializeGameParameters();
        CreateCubes();
        changeShader();
        searchForFirstLevelEmptySpace();
    }

    private void ReadTxtFile()
    {
        TxtFileLines = File.ReadAllLines(@"maze.txt");
    }

    private void initializeGameParameters()
    {        
        L = Int32.Parse (Regex.Match(TxtFileLines[0], @"\d+").Value);       
        N = Int32.Parse (Regex.Match(TxtFileLines[1], @"\d+").Value);
        K = Int32.Parse (Regex.Match(TxtFileLines[2], @"\d+").Value);

        startHammers = K;

        float posOfPlane = (N * scalingFactor - scalingFactor) / 2.0f;
        plane.transform.position = new Vector3(posOfPlane, 0.0f, posOfPlane);
        plane.transform.localScale = new Vector3(N * scalingFactor / 10.0f, 1.0f, N * scalingFactor / 10.0f);
    }

    private void CreateCubes()
    {      
        Vector3 cubeDimensions = new Vector3(scalingFactor, scalingFactor, scalingFactor);
        wormHolesList = new List<Vector3>();
        char[] charSeparators = new char[] { ' ' };
        int levelToDrawCube = 1;       
        int offset = 4;

        while (levelToDrawCube <= L)
        {
            
            for (int i = 0; i < N; i++)
            {
                string[] splittedLine = TxtFileLines[i + offset].Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < N; j++)
                {
                    Vector3 positionToDrawCube = new Vector3(scalingFactor * i,
                                                             scalingFactor * (levelToDrawCube - 0.5f),
                                                             scalingFactor * j);

                    if (splittedLine[j].Equals("R"))
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);                        
                        initializeCube(cube, "Red_Cube", cubeDimensions, positionToDrawCube);                       
                    }
                    else if (splittedLine[j].Equals("G"))
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        initializeCube(cube, "Green_Cube", cubeDimensions, positionToDrawCube);
                    }
                    else if (splittedLine[j].Equals("B"))
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        initializeCube(cube, "Blue_Cube", cubeDimensions, positionToDrawCube);                    
                    }
                    else if (splittedLine[j].Equals("W"))
                    {
                        cubeTextures.W.transform.localScale = cubeDimensions;
                        wormHolesList.Add(positionToDrawCube);
                        Instantiate(cubeTextures.W, positionToDrawCube, Quaternion.identity);
                    }
                    else if (splittedLine[j].Equals("T1"))
                    {
                        cubeTextures.T1.transform.localScale = cubeDimensions;
                        Instantiate(cubeTextures.T1, positionToDrawCube, Quaternion.identity);
                    }
                    else if (splittedLine[j].Equals("T2"))
                    {
                        cubeTextures.T2.transform.localScale = cubeDimensions;
                        Instantiate(cubeTextures.T2, positionToDrawCube, Quaternion.identity);
                    }
                    else if (splittedLine[j].Equals("T3"))
                    {
                        cubeTextures.T3.transform.localScale = cubeDimensions;
                        Instantiate(cubeTextures.T3, positionToDrawCube, Quaternion.identity);
                    }                
                }
            }
            
            levelToDrawCube++;
            offset += N + 1;
        }    
    }

    private void initializeCube(GameObject cube, string cubeName, Vector3 cubeDimensions, Vector3 positionToDrawCube)
    {       
        cube.transform.localScale = cubeDimensions;
        cube.transform.position = positionToDrawCube;
        cube.name = cubeName;
        cube.tag = "Cube";        
        cube.AddComponent<CubeHealth>();
        
        if (cubeName.Equals("Red_Cube"))
        {
            cube.GetComponent<Renderer>().material.color = Color.red;
        }
        else if(cubeName.Equals("Green_Cube"))
        {
            cube.GetComponent<Renderer>().material.color = Color.green;
        }
        else if(cubeName.Equals("Blue_Cube"))
        {
            cube.GetComponent<Renderer>().material.color = Color.blue;
        }
        
    }

    private void searchForFirstLevelEmptySpace()
    {
        List<int> listWithI = new List<int>();
        List<int> listWithJ = new List<int>();
        int offset = 4;

        char[] charSeparators = new char[] { ' ' };

        for (int i = 0; i < N; i++)
        {
            for (int j = 0; j < N; j++)
            {
                string[] splittedLine = TxtFileLines[i + offset].Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);

                if (splittedLine[j].Equals("E"))
                {
                    listWithI.Add(i);
                    listWithJ.Add(j);
                }
            }
        }
        int randomPosition = UnityEngine.Random.Range(0, listWithI.Count);
        player.transform.position = new Vector3(listWithI[randomPosition] * scalingFactor,
                                                0,
                                                listWithJ[randomPosition] * scalingFactor);        
    }

    void Update()
    {
        updateScore();

        if (Input.GetKeyDown(KeyCode.R))
        {
            isCameraRotating = !isCameraRotating;

            if (isCameraRotating)
            {
                setPositionOfCameraBeforeRotation();
            }
            else
            {
                setPositionOfCameraToDefault();
            }
        }

        if (isCameraRotating)
        {
            secondaryCamera.transform.RotateAround(new Vector3(N * scaleMultiplier,
                                                               0,
                                                               N * scaleMultiplier),
                                                   Vector3.up,
                                                   20 * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {    
            if (gameEnded == false)
            {
                secondaryCamera.enabled = !secondaryCamera.enabled;
                player.SetActive(!secondaryCamera.enabled);
                isTransparent = !isTransparent;

                changeShader();
            }                   
        }

        enableOrDisableCylinder();

        if (Input.GetKeyDown(KeyCode.X))
        {
            endTheGame();
        }     
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (player.transform.position.y > (L * scalingFactor))
            {
                endTheGame();
            }            
        }
        
        if (ScoreManager.score <= 0)
        {
            endTheGame();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        moveInGodMode();
    }

    private void updateScore()
    {
        int usedHammers = startHammers - K;
        float elapsedTime = Mathf.Round(Time.time);
        ScoreManager.score = N * N - (elapsedTime) - (usedHammers * 50);
    }

    private void setPositionOfCameraBeforeRotation()
    {
        scaleMultiplier = scalingFactor / 2.0f;

        secondaryCamera.transform.position = new Vector3(-(N * scaleMultiplier) - N,
                                                           scalingFactor * scalingFactor * 1.5f,
                                                          (N * scaleMultiplier));
        secondaryCamera.transform.rotation = new Quaternion();
        secondaryCamera.transform.Rotate(0, 90, 0);
    }

    private void setPositionOfCameraToDefault()
    {
        secondaryCamera.transform.position = startingCameraPosition;
        secondaryCamera.transform.rotation = new Quaternion();
        secondaryCamera.transform.Rotate(90, 0, 0);
    }

    private void changeShader()
    {        
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
        GameObject[] wormholes = GameObject.FindGameObjectsWithTag("WormHole");

        foreach (GameObject cube in cubes)
        {
            if (isTransparent == true)
            {
                cube.GetComponent<Renderer>().material.shader = shaderForTransparency;
                changeOpacity(cube, 0.35f);
            }
            else
            {
                cube.GetComponent<Renderer>().material.shader = shaderWithoutTransparency;
                changeOpacity(cube, 1.0f);
            }
        }
        foreach (GameObject wormhole in wormholes)
        {
            if (isTransparent == true)
            {
                wormhole.GetComponent<Renderer>().material.shader = shaderForTransparency;
                changeOpacity(wormhole, 0.5f);
            }
            else
            {
                wormhole.GetComponent<Renderer>().material.shader = shaderWithoutTransparency;
                changeOpacity(wormhole, 1.0f);
            }
        }
    }

    private void changeOpacity(GameObject cube, float alpha)
    {
        Color cubeColor = cube.GetComponent<Renderer>().material.color;

        if (cubeColor == Color.red)
        {
            cube.GetComponent<Renderer>().material.color = new Vector4(1, 0, 0, alpha);
        }
        else if (cubeColor == Color.green)
        {
            cube.GetComponent<Renderer>().material.color = new Vector4(0, 1, 0, alpha);
        }
        else if (cubeColor == Color.blue)
        {
            cube.GetComponent<Renderer>().material.color = new Vector4(0, 0, 1, alpha);
        }
        else if (cubeColor == Color.white)
        {
            cube.GetComponent<Renderer>().material.color = new Vector4(1, 1, 1, alpha);
        }
        else if (cubeColor == Color.black)
        {
            cube.GetComponent<Renderer>().material.color = new Vector4(0, 0, 0, alpha);
        }
    }

    private void endTheGame()
    {
        if (gameEnded == false)
        {
            endOfGameImage.color = endOfGameBackground;

            if (player.transform.position.y < (L * scalingFactor))
            {
                FinalScore.finalScore = 0;
            }
            else
            {
                FinalScore.finalScore = ScoreManager.score;
            }
            
            gameEnded = true;
            player.SetActive(false);
            secondaryCamera.enabled = true;
        }
    }

    private void enableOrDisableCylinder()
    {
        if (secondaryCamera.enabled == true)
        {
            cylinder.SetActive(true);
            cylinder.transform.position = player.transform.position;
        }
        else
        {
            cylinder.SetActive(false);
        }
    }

    private void moveInGodMode()
    {
        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0)
        {
            secondaryCamera.transform.position = new Vector3(secondaryCamera.transform.position.x,
                                                             secondaryCamera.transform.position.y - scrollingVariety,
                                                             secondaryCamera.transform.position.z);
        }
        else if (d < 0)
        {
            secondaryCamera.transform.position = new Vector3(secondaryCamera.transform.position.x,
                                                             secondaryCamera.transform.position.y + scrollingVariety,
                                                             secondaryCamera.transform.position.z);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            secondaryCamera.transform.position = new Vector3(secondaryCamera.transform.position.x + scrollingVariety,
                                                             secondaryCamera.transform.position.y,
                                                             secondaryCamera.transform.position.z);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            secondaryCamera.transform.position = new Vector3(secondaryCamera.transform.position.x - scrollingVariety,
                                                             secondaryCamera.transform.position.y,
                                                             secondaryCamera.transform.position.z);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            secondaryCamera.transform.position = new Vector3(secondaryCamera.transform.position.x,
                                                             secondaryCamera.transform.position.y,
                                                             secondaryCamera.transform.position.z + scrollingVariety);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            secondaryCamera.transform.position = new Vector3(secondaryCamera.transform.position.x,
                                                             secondaryCamera.transform.position.y,
                                                             secondaryCamera.transform.position.z - scrollingVariety);
        }

    }

    public List<Vector3> getWormHolesList()
    {
        return wormHolesList;
    }

    public float getScalingFactor()
    {
        return scalingFactor;
    }

    public int getN()
    {
        return N;
    }
}
