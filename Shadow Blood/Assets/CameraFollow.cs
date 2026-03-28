using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Takip Ayarlarý")]
    public Transform target; // Takip edilecek obje (Vampir)
    public float smoothSpeed = 5f; // Kameranýn yumuþaklýk/gecikme hýzý
    public Vector3 offset = new Vector3(0f, 0f, -10f); // 2D oyunlarda Z ekseni kamerada -10 olmalýdýr

    void LateUpdate()
    {
        // Eðer takip edilecek bir hedef varsa
        if (target != null)
        {
            // Kameranýn gitmek istediði yeni pozisyon
            Vector3 desiredPosition = target.position + offset;

            // Vector3.Lerp ile mevcut pozisyondan hedef pozisyona yumuþak bir geçiþ yapýyoruz
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Kameranýn pozisyonunu güncelliyoruz
            transform.position = smoothedPosition;
        }
    }
}