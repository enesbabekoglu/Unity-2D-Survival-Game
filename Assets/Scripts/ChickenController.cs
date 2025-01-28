using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class ChickenController : MonoBehaviour
{
    public float speed = 0.7f;  // Normal yürüme hızı
    public float scaredSpeedMultiplier = 7f;  // Korktuğunda 7x hız
    public float minScaredSpeedMultiplier;  // Gideceği mesafeye ulaştıkça hız bu değere kadar azalır
    public float jumpHeight = 1.5f;  // Zıplama yüksekliği
    public float jumpDuration = 0.5f;  // Zıplama süresi
    public float waitTime = 1f;  // Bekleme süresi
    public EdgeCollider2D walkingArea;  // Yürüme alanı
    public LayerMask obstacleLayer;  // Engel katmanı

    public AudioClip primarySound;
    public AudioClip secondarySound;
    public AudioClip scaredSound;
    private AudioSource audioSource;
    public AudioMixerGroup sfxMixerGroup;

    public float primarySoundChance = 0.8f;
    public float secondarySoundDuration = 5f;
    public float fadeDuration = 1f;

    public bool canLayEgg = true; // Tavuk yumurtlayabilir mi kontrolü
    public float layEggInterval = 30f; // Yumurtlama aralığı (saniye)
    public GameObject eggPrefab; // Yumurtanın prefabı

    private Vector2 targetPosition;
    private Animator animator;
    private bool isWaiting = false;
    private bool isScared = false;
    private Vector3 originalScale;
    private float originalSpeed;
    private bool isJumping = false;
    private Coroutine scaredMoveCoroutine;

    private FireControl fireControl;

    void Start()
    {
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
        originalSpeed = speed;
        minScaredSpeedMultiplier = speed;

        if (walkingArea == null)
        {
            Debug.LogError("Walking Area atanmadı! Lütfen EdgeCollider2D ekleyin.");
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
        audioSource.outputAudioMixerGroup = sfxMixerGroup;

        fireControl = FindObjectOfType<FireControl>();

        SetRandomTargetPosition();
        StartCoroutine(PlayChickenSounds());

        if (canLayEgg)
        {
            StartCoroutine(LayEggRoutine());
        }
    }

    void Update()
    {
        if (!isWaiting && !isScared && walkingArea != null)
        {
            Vector2 direction = targetPosition - (Vector2)transform.position;

            // Engel kontrolü
            if (IsObstacleInPath(direction))
            {
                Debug.Log("Engel algılandı! Yeni bir rota belirleniyor...");
                StartCoroutine(FindNewPath());
                return;
            }

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                StartCoroutine(WaitAndSetNewTarget());
            }

            UpdateSpriteDirection(direction);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                if (scaredMoveCoroutine != null)
                {
                    StopCoroutine(scaredMoveCoroutine);
                }
                scaredMoveCoroutine = StartCoroutine(ScaredMoveToRandomFarPoint(withJump: true, withSound: true));
            }
        }
    }

    private void UpdateSpriteDirection(Vector2 direction)
    {
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        if (direction.x < 0)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        else if (direction.x > 0)
            transform.localScale = originalScale;
    }

    private bool IsObstacleInPath(Vector2 direction)
    {
        float rayDistance = 1f;  // Engel kontrolü için mesafe
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, rayDistance, obstacleLayer);
        Debug.DrawRay(transform.position, direction.normalized * rayDistance, Color.red);
        return hit.collider != null;
    }

    private IEnumerator FindNewPath()
    {
        yield return new WaitForSeconds(0.2f);  // Tavuğun kısa süre bekleyip karar vermesi için
        SetRandomTargetPosition();
    }

    private void SetRandomTargetPosition()
    {
        targetPosition = GetRandomPointInsideArea();

        // Engel olan hedef noktaları engelle
        int attempts = 0;
        while (IsObstacleInPath(targetPosition - (Vector2)transform.position) && attempts < 10)
        {
            targetPosition = GetRandomPointInsideArea();
            attempts++;
        }
    }

    private Vector2 GetRandomPointInsideArea()
    {
        Vector2 randomPoint = Vector2.zero;
        if (walkingArea != null && walkingArea.pointCount > 0)
        {
            int randomIndex = Random.Range(0, walkingArea.pointCount);
            Vector2 localPoint = walkingArea.points[randomIndex];
            randomPoint = walkingArea.transform.TransformPoint(localPoint);
        }
        return randomPoint;
    }

    private IEnumerator WaitAndSetNewTarget()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        SetRandomTargetPosition();
        isWaiting = false;
    }

    private IEnumerator PlayChickenSounds()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));

            if (canLayEgg)
            {
                // Yumurta bırakabilen tavuğun ikinci sesi sadece yumurtlama sırasında çalınır
                continue;
            }

            AudioClip chosenClip = primarySound;

            if (Random.value > primarySoundChance)
            {
                chosenClip = secondarySound;
                yield return PlaySoundWithFade(chosenClip, secondarySoundDuration);
            }
            else
            {
                yield return PlaySoundWithFade(primarySound, primarySound.length);
            }
        }
    }

    private IEnumerator PlaySoundWithFade(AudioClip clip, float duration)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = clip;
            float elapsedTime = 0f;

            audioSource.Play();
            while (elapsedTime < fadeDuration)
            {
                audioSource.volume = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            audioSource.volume = 1;

            yield return new WaitForSeconds(duration - fadeDuration);

            elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                audioSource.volume = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            audioSource.Stop();
        }
    }

    private IEnumerator ScaredMoveToRandomFarPoint(bool withJump, bool withSound)
    {
        isScared = true;
        float scaredSpeed = originalSpeed * scaredSpeedMultiplier;

        if (withJump && !isJumping)
        {
            isJumping = true;
            Vector3 originalPosition = transform.position;
            float jumpElapsed = 0f;

            while (jumpElapsed < jumpDuration)
            {
                float progress = jumpElapsed / jumpDuration;
                transform.position = new Vector3(originalPosition.x, originalPosition.y + Mathf.Sin(progress * Mathf.PI) * jumpHeight, originalPosition.z);
                jumpElapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = originalPosition;
            isJumping = false;
        }

        if (withSound)
        {
            audioSource.clip = scaredSound;
            audioSource.Play();
        }

        Vector2 randomFarPoint = GetRandomPointInsideArea();
        float initialDistance = Vector2.Distance(transform.position, randomFarPoint);

        while (Vector2.Distance(transform.position, randomFarPoint) > 0.1f)
        {
            float currentDistance = Vector2.Distance(transform.position, randomFarPoint);
            float normalizedDistance = currentDistance / initialDistance;
            float currentMultiplier = Mathf.Lerp(minScaredSpeedMultiplier, scaredSpeedMultiplier, normalizedDistance);

            scaredSpeed = originalSpeed * currentMultiplier;
            transform.position = Vector2.MoveTowards(transform.position, randomFarPoint, scaredSpeed * Time.deltaTime);

            Vector2 direction = randomFarPoint - (Vector2)transform.position;
            UpdateSpriteDirection(direction);

            animator.SetFloat("SpeedMultiplier", scaredSpeed / originalSpeed);

            yield return null;
        }

        speed = originalSpeed;
        animator.SetFloat("SpeedMultiplier", 1f);
        isScared = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player") || collision.CompareTag("Bush"))
        {
            if (scaredMoveCoroutine != null)
            {
                StopCoroutine(scaredMoveCoroutine);
            }
            scaredMoveCoroutine = StartCoroutine(ScaredMoveToRandomFarPoint(withJump: false, withSound: false));
        }
        else if (collision.CompareTag("Fire"))
        {
            if (fireControl != null && fireControl.isBurning)
            {
                if (scaredMoveCoroutine != null)
                {
                    StopCoroutine(scaredMoveCoroutine);
                }
                scaredMoveCoroutine = StartCoroutine(ScaredMoveToRandomFarPoint(withJump: false, withSound: true));
            }
            else
            {
                SetRandomTargetPosition();  // Ateş yanmıyorsa yön değiştir
            }
        }
        else if (collision.CompareTag("Water") || collision.CompareTag("Barrel") || collision.CompareTag("Scarecow") || collision.CompareTag("House") || collision.CompareTag("Tree"))
        {
            if (scaredMoveCoroutine != null)
            {
                StopCoroutine(scaredMoveCoroutine);
            }

            SetRandomTargetPosition();
        }
    }

    private IEnumerator LayEggRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(layEggInterval);

            if (canLayEgg && Random.value <= 0.2f) // %20 ihtimalle yumurtla
            {
                if (eggPrefab != null)
                {
                    Instantiate(eggPrefab, transform.position, Quaternion.identity);
                    Debug.Log("Tavuk yumurtladı!");

                    // İkinci sesi yumurtlama sırasında çal
                    StartCoroutine(PlaySoundWithFade(secondarySound, secondarySoundDuration));
                }
                else
                {
                    Debug.LogWarning("EggPrefab atanmamış! Yumurtlama gerçekleştirilemedi.");
                }
            }
        }
    }
}
