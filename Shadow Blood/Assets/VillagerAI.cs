using UnityEngine;

public class VillagerAI : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float runSpeed = 3f;
    public float detectionRange = 5f; // Vampiri fark etme mesafesi

    private Transform player;
    private VampireController vampireScript;
    private GameManager gameManager;
    private Rigidbody2D rb;

    void Start()
    {
        // Sahnedeki objeleri otomatik buluyoruz (1 günlük jam için en hýzlý yöntem)
        player = GameObject.FindGameObjectWithTag("Player").transform;
        vampireScript = player.GetComponent<VampireController>();
        gameManager = FindObjectOfType<GameManager>();

        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (player == null || gameManager == null) return;

        // Vampir ile aramýzdaki mesafeyi ölçüyoruz
        float distance = Vector2.Distance(transform.position, player.position);

        if (gameManager.isDayTime)
        {
            // GÜNDÜZ: Vampir gölgede deðilse ve yakýnsa KOVALA
            if (distance <= detectionRange && !vampireScript.isInShadow)
            {
                Vector2 moveDirection = (player.position - transform.position).normalized;
                rb.MovePosition(rb.position + moveDirection * runSpeed * Time.fixedDeltaTime);
            }
        }
        else
        {
            // GECE: Vampir yakýnsa ondan KAÇ (Ters yöne koþ)
            if (distance <= detectionRange * 1.5f) // Gece karanlýðýnda daha uzaktan korksunlar
            {
                Vector2 moveDirection = (transform.position - player.position).normalized;
                rb.MovePosition(rb.position + moveDirection * runSpeed * Time.fixedDeltaTime);
            }
        }
    }

    // Çarpýþma Gerçekleþtiðinde Ne Olacak?
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameManager.isDayTime)
            {
                // Gündüz vampir yakalandý! (Ýstersen buraya vampir can kaybetme kodu ekleyebiliriz)
                Debug.Log("Köylü sana vurdu! Gündüzleri çok tehlikeli.");
            }
            else
            {
                // Gece vampir köylüyü yakaladý! (Av zamaný)
                Debug.Log("Köylüyü yedin! (Skor veya Can kazanmalýsýn)");
                Destroy(gameObject); // Köylüyü sahneden sil
            }
        }
    }
}