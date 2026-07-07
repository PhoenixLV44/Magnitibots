using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            float sceneIndex = SceneManager.GetActiveScene().buildIndex;
            Globals.Managers.Settings.TransitionScene();
        }
    }
    
}
