using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Teleportation : MonoBehaviour
{
    public GameObject gameManager;
    public Image teleportationImage;
    public float flashSpeed;
   
    private float x, y, z;
    private bool teleportationEnabled = false;
    private Color flashColour = new Color(1f, 1f, 1f, 1f);
    private List<Vector3> wormholesList = new List<Vector3>();
    private AudioSource[] audioSourcesForPlayer;
    private AudioSource teleportationAudioSource;

    void Start()
    {
        audioSourcesForPlayer = GetComponents<AudioSource>();
        teleportationAudioSource = audioSourcesForPlayer[1];       
    }

    void OnEnable()
    {
        StartCoroutine(getTheList());
    }
        
    void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "WormHole")
        {
            x = other.transform.position.x;
            y = other.transform.position.y;
            z = other.transform.position.z;

            if (wormholesList != null)
            {
                teleportationEnabled = true;
                teleportationAudioSource.Play();
                goToOtherWormhole(x, y, z);
            }            
        }
    }        

    private void goToOtherWormhole(float x, float y, float z)
    {
        for (int i = 0; i < wormholesList.Count; i++)
        {
            if ((wormholesList[i].x != x && wormholesList[i].y == y) ||
                (wormholesList[i].z != z && wormholesList[i].y == y))
            {
                transform.position = wormholesList[i];                
            }
        }
        wormholesList = null;
    }

    void Update()
    {
        if (teleportationEnabled)
        {
            teleportationImage.color = flashColour;
        }
        else
        {
            teleportationImage.color = Color.Lerp(teleportationImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        teleportationEnabled = false;
    }

    IEnumerator getTheList()
    {
        yield return new WaitForSeconds(2);
        while (true)
        {
            wormholesList = gameManager.GetComponent<LoadMazeFromTxt>().getWormHolesList();
            yield return new WaitForSeconds(2);
        }
    }
   
}
