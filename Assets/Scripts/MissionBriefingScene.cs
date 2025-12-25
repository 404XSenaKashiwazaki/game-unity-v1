using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class MissionBriefingSceneController : MonoBehaviour
{
    public TextMeshProUGUI briefingText, objectText;
    public string gameplaySceneName = "GameScene";

    [Header("Typewriter")]
    public float typingSpeed = 0.035f;
    public float paragraphHoldTime = 1.2f;
    public int maxVisibleParagraph = 2;

    private bool isTyping = false;
    private bool canContinue = false;

    void Start()
    {
        StartCoroutine(PlayBriefing());
        MissionManager.ResetMissionState();
    }

   IEnumerator PlayBriefing()
{
    isTyping = true;
    canContinue = false;
    objectText.text = "";

    briefingText.text =
        $"MISSION: {MissionManager.currentMission}\n\n\n" +
        $"fpv used: {MissionManager.totalUsedDrones}/{MissionManager.totalDrones}\n\n" +
        $"neutralized: {MissionManager.totalDestroyedTargets}/{MissionManager.totalTargets}\n\n\n";

    // 🔹 PARAGRAF (KATA ASLI, TIDAK DIUBAH)
    List<string> paragraphs = new List<string>();

    switch (MissionManager.currentMission)
    {
        case 1:
           paragraphs.Add("KOMANDAN:");
    paragraphs.Add("ORCHESTRA BEKERJA, BEKERJA, BEKERJA,");
    paragraphs.Add("BURUNG-BURUNG DARI MARINIR KE-155 MELIHAT KOLOM MUSUH");
    paragraphs.Add("MUSUH MAJU, 2 KENDARAAN, MAJU DARI PISHCHANOYE.");

    paragraphs.Add("MEREKA BERGERAK, 2 KENDARAAN, 2 KENDARAAN DARI PISHCHANOYE.");
    paragraphs.Add("ORCHESTRA BEKERJA, BEKERJA,");
    paragraphs.Add("KIRIMKAN BEBERAPA BURUNG, HENTIKAN SERANGAN BALIK MUSUH, GILING MEREKA SEBELUM MEMASUKI WILAYAH KITA.");

            break;
         case 2:
           paragraphs.Add("KOMANDAN:");
paragraphs.Add("ORCHESTRA, ORCHESTRA, BEKERJA,");
paragraphs.Add("MUSUH MENGRIMKAN GELOMBANG KEDUA,");
paragraphs.Add("KOLOM MASIH TERDIRI DARI 2 KENDARAAN,");
paragraphs.Add("****");
paragraphs.Add("MUSUH MASIH BERUPAYA MAJU, DARI PISHCHANOYE,");
paragraphs.Add("BERKERJA, BERKERJA, BERKERJA,");
paragraphs.Add("ORCHESTRA,");
paragraphs.Add("GILING KELOMPOK KEDUA MUSUH");
            break;
             case 3:
           paragraphs.Add("KOMANDAN:");
paragraphs.Add("ORCHESTRA,");
paragraphs.Add("MUSUH MENGRIMKAN GELOMBANG TERAKHIR,");
paragraphs.Add("KOLOM MASIH SAMA, TERDIRI DARI 2 KENDARAAN,");
paragraphs.Add("KERUGIAN DI DUA KOLOM PERTAMA,");
paragraphs.Add("MENGHABISKAN CADANGAN MUSUH,");
paragraphs.Add("INI PERKERJAAN TERAHIR,");
paragraphs.Add("GILING MUSUH YANG MENCOBA MAJU SEPERTI BIASA");
            break;
    }

    // 🔁 1 paragraf → hapus → lanjut
    foreach (string paragraph in paragraphs)
    {
        objectText.text = "";

        // TYPEWRITER PER PARAGRAF
        foreach (char c in paragraph)
        {
            objectText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        // TAHAN SEBENTAR BIAR TERBACA
        yield return new WaitForSeconds(1.1f);

        // HAPUS LANGSUNG
        objectText.text = "";
        yield return new WaitForSeconds(0.15f);
    }

    isTyping = false;
    canContinue = true;
}

    void Update()
    {
        if (canContinue && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(gameplaySceneName);
        }
    }

    public void GamePlay()
    {
        if (!isTyping)
            SceneManager.LoadScene(gameplaySceneName);
    }
}
