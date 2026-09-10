using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public TMP_Text loadingText;
    public TMP_Text tipsText;

    public string[] tips =
    {
        "Naikkan throttle secara perlahan agar drone stabil.",
        "Gunakan yaw untuk berbelok lebih halus.",
        "Lewati checkpoint sesuai urutan.",
        "Tekan ESC untuk Pause.",
        "Latihan membuat kontrol drone menjadi lebih mudah."
    };

    public static string SceneToLoad;

    IEnumerator Start()
    {
        // Pilih tips secara acak
        // tipsText.text = tips[Random.Range(0, tips.Length)];

        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneToLoad);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingText.text = $"Loading... {(progress * 100f):0}%";

            if (operation.progress >= 0.9f)
            {
                loadingText.text = "Loading... 100%";

                yield return new WaitForSeconds(1f);

                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}