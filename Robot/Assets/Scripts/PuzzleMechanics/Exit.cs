using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Exit : MonoBehaviour
{

    void OnTriggerEnter(Collider theCollision)
    {
        if (theCollision.name.Contains("Door"))
        {
            StartCoroutine("Wait");
        }

    }

        IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(1);
    }
}
