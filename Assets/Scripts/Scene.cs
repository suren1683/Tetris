using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public void _Start() {
        SceneManager.LoadScene("SampleScene");
    }
}
