using UnityEngine;
using UnityEngine.UI; // Arayüz (Image) için gerekli

public class GameManager : MonoBehaviour
{
    [Header("Ayarlar")]
    public float dayDuration = 15f;   // Gündüz süresi
    public float nightDuration = 10f; // Gece süresi (Avlanma vakti)

    [Header("Referanslar")]
    public VampireController vampire; // Vampirimize ulaþmak için
    public Image darknessOverlay;     // Ekraný karartacak görsel

    // Durum takibi
    public bool isDayTime = true;
    private float timer;

    void Start()
    {
        timer = dayDuration;
        UpdateVampireState();

        // Baþlangýçta karanlýk efektini sýfýrla
        Color c = darknessOverlay.color;
        c.a = 0f;
        darknessOverlay.color = c;
    }

    void Update()
    {
        // Sayacý geriye doðru saydýr
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            SwitchTime(); // Süre bitince gece/gündüz deðiþtir
        }

        // --- GÖRSEL GEÇÝÞ (Yumuþak Kararma/Aydýnlanma) ---
        // Gündüzse alfa (saydamlýk) 0 olacak, Geceyse 0.6 (Yarý karanlýk) olacak.
        float targetAlpha = isDayTime ? 0f : 0.6f;

        Color currentColor = darknessOverlay.color;
        // Mathf.Lerp ile rengin yavaþça deðiþmesini saðlýyoruz (Fade efekti)
        currentColor.a = Mathf.Lerp(currentColor.a, targetAlpha, Time.deltaTime * 2f);
        darknessOverlay.color = currentColor;
    }

    void SwitchTime()
    {
        isDayTime = !isDayTime; // Durumu tersine çevir

        // Yeni duruma göre sayacý yeniden kur
        timer = isDayTime ? dayDuration : nightDuration;

        UpdateVampireState(); // Vampir'e haber ver

        if (isDayTime)
            Debug.Log("GÜNDÜZ OLDU! Gölgelere saklan!");
        else
            Debug.Log("GECE OLDU! Karanlýk seni koruyor, av zamaný!");
    }

    void UpdateVampireState()
    {
        // Vampir scriptindeki deðiþkeni güncelliyoruz
        if (vampire != null)
        {
            vampire.isDayTime = this.isDayTime;
        }
    }
}