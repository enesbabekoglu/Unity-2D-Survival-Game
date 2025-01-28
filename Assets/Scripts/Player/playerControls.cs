using System.Collections;
using System.Collections.Generic;

using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;

public class playerController : MonoBehaviour
{
    public float touchDistance = 1.0f;  // Ray uzunluğu
    public LayerMask collisionLayer;
    public GameObject camera;

    public PlayerState pState;
    public float radius = 5f;  // Algılama yarıçapı
    public SpriteRenderer spriteRenderer;

    public Animator animator;  // Animator referansı

    // Ses kaynakları ve efektler
    public AudioSource footstepAudioSource;  // Yürüme sesi kaynağı
    public AudioClip grassFootstepSound;
    public AudioClip woodFootstepSound;
    public AudioClip stoneFootstepSound;
    public AudioClip waterFootstepSound;
    public AudioClip bushFootstepSound;
    public AudioClip damageSound;  // Hasar sesi
    public AudioMixerGroup sfxMixerGroup;  // Merkezi ses kontrol için mixer group

    private string currentSurface = "Grass";  // Varsayılan yüzey tipi

    void Start()
    {
        Debug.Log("hi");

        // Ses kaynağını mixer gruba bağla
        footstepAudioSource.outputAudioMixerGroup = sfxMixerGroup;
        footstepAudioSource.loop = true;  // Sonsuz döngüde çalacak şekilde ayarlandı
    }

    void Update()
    {
        Vector3 posTemp = transform.position;
        posTemp.y -= 0.5f;

        RaycastHit2D hitUp = Physics2D.Raycast(posTemp, Vector2.up, touchDistance, collisionLayer);
        RaycastHit2D hitDown = Physics2D.Raycast(posTemp, Vector2.down, touchDistance, collisionLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(posTemp, Vector2.right, touchDistance, collisionLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(posTemp, Vector2.left, touchDistance, collisionLayer);

        if(Input.GetKey(KeyCode.LeftControl)){
            animator.SetBool("sneaking", true);
            if(!pState.sneakSpeed){
                pState.baseSpeed /= 2.0f;
                pState.sneakSpeed = true;
            }
        }else{
            animator.SetBool("sneaking", false);
            if(pState.sneakSpeed){
                pState.baseSpeed *= 2.0f;
                pState.sneakSpeed = false;
            }
        }

        float horizontalMove = 0.0f;
        float verticalMove = 0.0f;

        if (Input.GetKey(KeyCode.A) && !hitRight)
        {
            horizontalMove -= 1.0f;
        }

        if (Input.GetKey(KeyCode.D) && !hitLeft)
        {
            horizontalMove += 1.0f;
        }

        if (Input.GetKey(KeyCode.W) && !hitUp)
        {
            verticalMove += 1.0f;
        }

        if (Input.GetKey(KeyCode.S) && !hitDown)
        {
            verticalMove -= 1.0f;
        }

        if (horizontalMove != 0.0f || verticalMove != 0.0f)
        {
            animator.SetBool("running", true);
            PlayFootstepSound();  // Yürüme sesini başlat
        }
        else
        {
            animator.SetBool("running", false);
            StopFootstepSound();  // Karakter durduğunda sesi durdur
        }

        if (horizontalMove < 0)
        {
            spriteRenderer.flipX = true;
            pState.isFlip = true;
        }
        else if (horizontalMove > 0)
        {
            spriteRenderer.flipX = false;
            pState.isFlip = false;
        }

        transform.Translate(new Vector3(horizontalMove * pState.speedCharacter * Time.deltaTime, verticalMove * pState.speedCharacter * Time.deltaTime, 0.0f));
        camera.transform.Translate(new Vector3(horizontalMove * pState.speedCharacter * Time.deltaTime, verticalMove * pState.speedCharacter * Time.deltaTime, 0.0f));
    }

    private void PlayFootstepSound()
    {
        if (!footstepAudioSource.isPlaying)
        {
            switch (currentSurface)
            {
                case "Grass":
                    footstepAudioSource.clip = grassFootstepSound;
                    break;
                case "Wood":
                    footstepAudioSource.clip = woodFootstepSound;
                    break;
                case "Stone":
                    footstepAudioSource.clip = stoneFootstepSound;
                    break;
                case "Water":
                    footstepAudioSource.clip = waterFootstepSound;
                    break;
                case "Bush":
                    footstepAudioSource.clip = bushFootstepSound;
                    break;
            }

            footstepAudioSource.Play();  // Ses çalmaya başlar
        }
    }

    private void StopFootstepSound()
    {
        if (footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Stop();  // Ses çalmayı durdurur
        }
    }

    public void PlayDamageSound()
    {
        footstepAudioSource.PlayOneShot(damageSound);  // Hasar sesi tek sefer çal
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Grass"))
        {
            currentSurface = "Grass";
        }
        else if (other.CompareTag("Wood"))
        {
            currentSurface = "Wood";
        }
        else if (other.CompareTag("Stone"))
        {
            currentSurface = "Stone";
        }
        else if (other.CompareTag("Water"))
        {
            currentSurface = "Water";
        }
        else if (other.CompareTag("Bush"))
        {
            currentSurface = "Bush";
        }
    }

    public void teleportPlayer(Vector3 pos){
        transform.transform.position = pos;
        Vector3 tempVec = pos;
        tempVec.z = camera.transform.position.z; 
        camera.transform.position = tempVec;

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bush") || other.CompareTag("Water") || other.CompareTag("Wood") || other.CompareTag("Stone"))
        {
            currentSurface = "Grass";  // Yüzeyden çıkınca varsayılan yüzeye geç
        }
    }
}
