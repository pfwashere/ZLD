using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public Animator transitionAnim;
    public string sceneName;

    public void StartTransition(string sceneLoad)
    {
        StartCoroutine(LoadScene(sceneLoad));
    }

    IEnumerator LoadScene(string sceneName)
    {
        transitionAnim.SetTrigger("EndCircle");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneName);
    }
}
