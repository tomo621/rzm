using UnityEngine;

public class RhythmManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float bpm = 130f;
    [SerializeField] private GameObject targetPrefab;

    [Header("出現エリアとターゲット設定")]
    [SerializeField] private Vector2 spawnArea;
    [SerializeField] private float targetLifetime = 0.45f;

    private float secPerBeat;
    private int lastOffBeatCount = -1;

    // 曲の進行状態を管理する変数
    private bool hasMusicStarted = false;
    private bool isGameCleared = false;

    void Start()
    {
        secPerBeat = 60f / bpm;
    }

    void Update()
    {
        // 曲が再生中の時の処理
        if (musicSource.isPlaying)
        {
            hasMusicStarted = true;

            float currentBeat = musicSource.time / secPerBeat;
            int currentOffBeatCount = Mathf.FloorToInt(currentBeat - 0.5f);

            if (currentOffBeatCount > lastOffBeatCount)
            {
                SpawnTarget();
                lastOffBeatCount = currentOffBeatCount;
            }
        }
        // 曲が終わった時の処理
        else
        {
            //  曲が最後まで終わった瞬間
            if (hasMusicStarted && !isGameCleared)
            {
                isGameCleared = true;

                // ScoreManagerにリザルト画面を出すようする
                if (ScoreManager.instance != null)
                {
                    ScoreManager.instance.ShowResult();
                }
            }
        }
    }

    void SpawnTarget()
    {
        float randomX = Random.Range(-spawnArea.x / 2f, spawnArea.x / 2f);
        float randomY = Random.Range(-spawnArea.y / 2f, spawnArea.y / 2f);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0f);

        GameObject newTarget = Instantiate(targetPrefab, randomPosition, Quaternion.identity);
        Destroy(newTarget, targetLifetime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x, spawnArea.y, 0f));
    }
}