using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField]
    private Image loadingFill;
    private float lastValue;
    // Start is called before the first frame update

    public IEnumerator StartAsyncLoading(int index)
    {
        yield return new WaitForSeconds(1.2f);
        AsyncOperation loadingScene = SceneManager.LoadSceneAsync(index);
        //AsyncOperation unloading = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        lastValue = 0f;
        while (!loadingScene.isDone)
        {
            loadingFill.fillAmount = loadingScene.progress;
            if(loadingFill.fillAmount <= lastValue)
            {
                lastValue += 0.01f;
                loadingFill.fillAmount = lastValue;
            }
            //Debug.Log(loadingFill.fillAmount);
            yield return new WaitForSeconds(0.001f);
        }

    }
}
