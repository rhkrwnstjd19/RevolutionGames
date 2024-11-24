using UnityEngine;
using System.Collections;

public class SpinRoulette : MonoBehaviour
{
    public Transform rouletteWheel; // �귿 ������Ʈ�� Transform
    public GameObject rouletteObject;
    public float spinSpeed; // ȸ�� �ӵ�
    public bool isSpinning;
    public int stopSpeed;
    public SpawnMonster spawnMonster;

    public bool isRouletteOn = true;

    public AudioSource audioSource;  // 오디오 소스 참조
    public AudioClip spinSound;      // 룰렛 돌아가는 효과음

    void Start()
    {
        isSpinning = true;
        stopSpeed = Random.Range(1, 4);
        spinSpeed = Random.Range(1000, 2000);

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isSpinning)
        {
            // �귿 ȸ��
            rouletteWheel.Rotate(0, 0, spinSpeed * Time.deltaTime);

            // ������ �ӵ��� ����
            spinSpeed = Mathf.Lerp(spinSpeed, 0, Time.deltaTime * 1f);

            // ����
            if (spinSpeed < 1f)
            {
                isSpinning = false;

                // 효과음 정지
                audioSource.PlayOneShot(spinSound);
                
                float selectedAngle = rouletteWheel.eulerAngles.z;
                DetermineResult(selectedAngle);

            }
        }
    }

    void DetermineResult(float angle)
    {
        int selectedItem = Mathf.FloorToInt(angle / 72f);
        spawnMonster.SetSelectedMonster(selectedItem);

        StartCoroutine(DisableRoulette());
    }

    IEnumerator DisableRoulette()
    {
        yield return new WaitForSeconds(1f);

        if (rouletteObject != null)
        {
            isRouletteOn = false;
            rouletteObject.SetActive(false);

            spawnMonster.Spawn();
        }
    }
}