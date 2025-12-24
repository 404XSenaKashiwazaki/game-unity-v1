using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textTimer;            // teks timer utama
    [SerializeField] TextMeshProUGUI textTimerCountDown;   // teks countdown
    [SerializeField] TextMeshProUGUI textGo;               // teks "GO!"
    [SerializeField] float time;
    [SerializeField] float remainTime = 3f;                // durasi countdown

    private bool countdownFinished = false;
    private bool isFading = false;
    private bool showGo = false;
    private float fadeDuration = 1f;                       // durasi fade out countdown
    private float fadeTimer = 0f;
    private float goDuration = 1.5f;                       // durasi "GO!" tampil
    private float goTimer = 0f;

    // Parameter animasi "GO!"
    private float goScaleMin = 1f;
    private float goScaleMax = 1.5f;
    private float goPulseSpeed = 4f; // semakin besar = makin cepat

    void Start()
    {
        // Validasi awal
        if (textTimer == null) Debug.LogError("[Timer] textTimer belum di-assign di Inspector!");
        if (textTimerCountDown == null) Debug.LogError("[Timer] textTimerCountDown belum di-assign di Inspector!");
        if (textGo == null) Debug.LogError("[Timer] textGo belum di-assign di Inspector!");

        // Pastikan textGo tersembunyi di awal
        if (textGo != null)
        {
            textGo.gameObject.SetActive(false);
            textGo.rectTransform.localScale = Vector3.one * goScaleMin;
        }
    }

    void Update()
    {
        if (!countdownFinished)
        {
            Remain();
        }
        else if (showGo)
        {
            ShowGoText();
        }
        else
        {
            FnTimer();
        }
    }

    void FnTimer()
    {
        time += Time.deltaTime;
        if (textTimer != null)
        {
            int min = Mathf.FloorToInt(time / 60);
            int sec = Mathf.FloorToInt(time % 60);
            textTimer.text = string.Format("{0:00}:{1:00}", min, sec);
        }
    }

    void Remain()
    {
        if (textTimerCountDown == null)
        {
            countdownFinished = true;
            showGo = true;
            return;
        }

        if (remainTime > 0)
        {
            remainTime -= Time.deltaTime;
            if (remainTime < 0) remainTime = 0;
            int min = Mathf.FloorToInt(remainTime / 60);
            int sec = Mathf.FloorToInt(remainTime % 60);
            textTimerCountDown.text = string.Format("{0:00}:{1:00}", min, sec);
        }
        else if (!isFading)
        {
            StartFadeOut();
        }
        else
        {
            FadeOutCountdown();
        }
    }

    void StartFadeOut()
    {
        isFading = true;
        fadeTimer = 0f;
    }

    void FadeOutCountdown()
    {
        if (textTimerCountDown == null)
        {
            isFading = false;
            countdownFinished = true;
            showGo = true;
            return;
        }

        fadeTimer += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
        Color color = textTimerCountDown.color;
        color.a = alpha;
        textTimerCountDown.color = color;

        if (fadeTimer >= fadeDuration)
        {
            textTimerCountDown.gameObject.SetActive(false);
            isFading = false;
            countdownFinished = true;
            showGo = true;

            // tampilkan teks GO!
            if (textGo != null)
            {
                textGo.gameObject.SetActive(true);
                Color goColor = textGo.color;
                goColor.a = 1f;
                textGo.color = goColor;
                textGo.rectTransform.localScale = Vector3.one * goScaleMin;
                goTimer = 0f;
            }
        }
    }

    void ShowGoText()
    {
        if (textGo == null)
        {
            goTimer += Time.deltaTime;
            if (goTimer >= goDuration)
                showGo = false;
            return;
        }

        goTimer += Time.deltaTime;

        // 🔥 Efek animasi "GO!" membesar dan mengecil
        float pulse = Mathf.PingPong(Time.time * goPulseSpeed, 1f); // antara 0-1
        float scale = Mathf.Lerp(goScaleMin, goScaleMax, pulse);
        textGo.rectTransform.localScale = Vector3.one * scale;

        // Fade-out perlahan di akhir durasi
        if (goTimer > goDuration - 0.5f)
        {
            float fadeOut = Mathf.InverseLerp(goDuration, goDuration - 0.5f, goTimer);
            Color c = textGo.color;
            c.a = Mathf.Lerp(1f, 0f, fadeOut);
            textGo.color = c;
        }

        if (goTimer >= goDuration)
        {
            textGo.gameObject.SetActive(false);
            showGo = false;
        }
    }

    // Optional: reset ulang timer + countdown
    public void ResetAndStart(float newRemain = -1f)
    {
        if (newRemain > 0f) remainTime = newRemain;
        time = 0f;
        countdownFinished = false;
        isFading = false;
        showGo = false;
        fadeTimer = 0f;
        goTimer = 0f;

        if (textTimerCountDown != null)
        {
            textTimerCountDown.gameObject.SetActive(true);
            Color c = textTimerCountDown.color;
            c.a = 1f;
            textTimerCountDown.color = c;
        }

        if (textGo != null)
        {
            textGo.gameObject.SetActive(false);
            textGo.rectTransform.localScale = Vector3.one * goScaleMin;
        }
    }
}
