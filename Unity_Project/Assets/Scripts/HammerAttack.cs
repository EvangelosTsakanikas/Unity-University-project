using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HammerAttack : MonoBehaviour
{
    public GameObject firstPersonController;
    public GameObject gameManager;
    public GameObject pickUp;

    private Transform childGameObject;
    private FirstPersonController fpsController;
    private int hammerHealth = 100;
    private Color hammerColor = new Color(1.0f, 0.0f, 0.0f);
    private AudioSource[] audioSourcesForHammer;
    private AudioSource cubeSmashedAudioSource;
    private AudioSource hammerHitAudioSource;
    private AudioSource lightSaberOnAudioSource;

    void Start ()
    {
        childGameObject = firstPersonController.transform.FindChild("Hammer");

        fpsController = firstPersonController.GetComponent<FirstPersonController>();
        Physics.IgnoreCollision(GetComponent<Collider>(), fpsController.GetComponent<Collider>());

        gameObject.GetComponent<Renderer>().material.color = hammerColor;
        initializeAudioSources();
    }

    private void initializeAudioSources()
    {
        audioSourcesForHammer = GetComponents<AudioSource>();

        cubeSmashedAudioSource = audioSourcesForHammer[0];
        hammerHitAudioSource = audioSourcesForHammer[1];
        lightSaberOnAudioSource = audioSourcesForHammer[2];
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cube")
        {
            childGameObject.gameObject.GetComponent<SwingTheHammer>().getAnimator().Play("HammerSwing", -1, 0.85f);                     

            hammerHealth -= 10;
            other.gameObject.GetComponent<CubeHealth>().health--;
            hammerColor.r -= 0.13f;
            gameObject.GetComponent<Renderer>().material.color = hammerColor;
            hammerHitAudioSource.Play();
            checkHammerCondition();

            if (other.gameObject.GetComponent<CubeHealth>().health == 0)
            {
                cubeSmashedAudioSource.Play();

                Destroy(other.gameObject, 0.2f);               
                spawnSmallerCubes(other);

                float spawnProbability = Random.value;
                if (spawnProbability <= 0.2f)
                {
                    Vector3 positionToSpawnPickUp = other.gameObject.transform.position;
                    positionToSpawnPickUp.y -= (gameManager.GetComponent<LoadMazeFromTxt>().getScalingFactor() / 2.0f);
                    positionToSpawnPickUp.y += 0.1f;

                    Instantiate(pickUp, positionToSpawnPickUp, Quaternion.Euler(90, 0, 0));
                }
            }                            
        }       
    }

    private void checkHammerCondition()
    {
        if (hammerHealth == 0)
        {
            LoadMazeFromTxt.K--;
            if (LoadMazeFromTxt.K != 0)
            {                
                lightSaberOnAudioSource.Play();
                hammerColor = Color.red;
                gameObject.GetComponent<Renderer>().material.color = hammerColor;
                hammerHealth = 100;
            }
            else
            {
                this.transform.parent.gameObject.SetActive(false);                
            }           
        }
    }

    private void spawnSmallerCubes(Collider destroyedCube)
    {
        float numberOfSmallerCubes = Random.Range(4, 8);

        Material materialOfSmallerCubes = destroyedCube.gameObject.GetComponent<Renderer>().sharedMaterial;

        for (int i = 0; i < numberOfSmallerCubes; i++)
        {
            GameObject smallCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            float s = gameManager.GetComponent<LoadMazeFromTxt>().getScalingFactor() / 2.0f;
            float randomX = Random.Range(destroyedCube.transform.position.x - s, destroyedCube.transform.position.x + s);
            float randomY = Random.Range(destroyedCube.transform.position.y - s, destroyedCube.transform.position.y + s);
            float randomZ = Random.Range(destroyedCube.transform.position.z - s, destroyedCube.transform.position.z + s);

            smallCube.transform.position = new Vector3(randomX, randomY, randomZ);
            smallCube.transform.localScale = (destroyedCube.transform.localScale / numberOfSmallerCubes);
            smallCube.gameObject.GetComponent<Renderer>().material = materialOfSmallerCubes;
            smallCube.AddComponent<Rigidbody>();

            Destroy(smallCube, 3f);
        }               
    }

    public void setHealth()
    {
        hammerHealth = 100;
    }

    public void setColor()
    {
        hammerColor = Color.red;
        gameObject.GetComponent<Renderer>().material.color = hammerColor;
    }

    public void playAudioSource()
    {
        lightSaberOnAudioSource.Play();
    }   
}
