using UnityEngine;
using UnityEngine.InputSystem;

public class CrosshairController : MonoBehaviour
{
    [Header("エイム設定")]
    public float smoothTime = 0.05f;
    private Vector3 velocity = Vector3.zero;

    [Header("クリック演出")]
    public float clickScale = 1.3f;
    public float returnSpeed = 15f;
    private Vector3 baseScale;

    [Header("音の設定")]
    public AudioClip shootSound;
    public AudioClip hitSound;
    private AudioSource audioSource;

    void Start()
    {
        Cursor.visible = false;
        baseScale = transform.localScale;

        // 音を鳴らすためのコンポーネントを自動で取得
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mousePos = new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f);
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
        targetPos.z = 0f;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            transform.localScale = baseScale * clickScale;

            // 射撃音を鳴らす
            if (shootSound != null)
            {
                audioSource.PlayOneShot(shootSound);
            }

            RaycastHit2D hit = Physics2D.Raycast(targetPos, Vector2.zero);

            if (hit.collider != null)
            {
                //命中音を鳴らす
                if (hitSound != null)
                {
                    audioSource.PlayOneShot(hitSound);
                }

                // 的を壊す前に、ScoreManagerに100点を加算する
                if (ScoreManager.instance != null)
                {
                    ScoreManager.instance.AddScore(100);
                }

                Destroy(hit.collider.gameObject);
            }
        }

        transform.localScale = Vector3.Lerp(transform.localScale, baseScale, Time.deltaTime * returnSpeed);
    }
}