using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Redirect(int choice){
        switch(choice){
            case 0:
                // Classic run
                SceneManager.LoadScene("TeamPrep");
                break;

            case 1:
                // Themed run
                break;

            case 2:
                // Quit
                break;
        }
    }
}
