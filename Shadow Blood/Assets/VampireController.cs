using UnityEngine;

public class VampireController : MonoBehaviour
{
    [Header("Hareket ve Can")]
    public float moveSpeed = 5f;
    public float maxHealth = 100f;
    public float currentHealth;
    public float sunDamageRate = 15f; // Saniyede alýnacak güneþ hasarý

    [Header("Durumlar")]
    public bool isInShadow = false;
    public bool isDayTime = true; // Þimdilik test için burada, sonra GameManager yapacak

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        // 1. WASD veya Yön Tuþlarý ile Girdi Alma
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 2. Güneþ Hasarý Kontrolü
        // Eðer gündüzse VE gölgede deðilsek canýmýz azalýr
        if (isDayTime && !isInShadow)
        {
            TakeSunDamage();
        }
    }

    void FixedUpdate()
    {
        // 3. Fiziðe Dayalý Titremeyen Hareket
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    void TakeSunDamage()
    {
        currentHealth -= sunDamageRate * Time.deltaTime; // Saniyeye baðlý pürüzsüz hasar

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Güneþte kül oldun! Oyun Bitti.");
            // Ýleride buraya ölme/restart kodu ekleyeceðiz.
        }
    }

    // --- GÖLGE ALGILAMA SÝSTEMÝ ---

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // "Shadow" etiketli bir objeye girdiðimizde
        if (collision.CompareTag("Shadow"))
        {
            isInShadow = true;
            Debug.Log("Gölgeye girdin, güvendesin.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // "Shadow" etiketli objeden çýktýðýmýzda
        if (collision.CompareTag("Shadow"))
        {
            isInShadow = false;
            Debug.Log("Gölgeden çýktýn! Güneþ yakýyor!");
        }
    }
}