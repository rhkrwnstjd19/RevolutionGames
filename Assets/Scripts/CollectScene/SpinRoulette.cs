using UnityEngine;
using System.Collections;

public class SpinRoulette : MonoBehaviour
{
    public Transform rouletteWheel; // ∑Í∑ø ø¿∫Í¡ß∆Æ¿« Transform
    public GameObject rouletteObject;
    public float spinSpeed; // »∏¿¸ º”µµ
    public bool isSpinning;
    public int stopSpeed;
    public SpawnMonster spawnMonster;

    public bool isRouletteOn = true;

    void Start()
    {
        isSpinning = true;
        stopSpeed = Random.Range(1, 4);
        spinSpeed = Random.Range(1000, 2000);
    }

    void Update()
    {
        if (isSpinning)
        {
            // ∑Í∑ø »∏¿¸
            rouletteWheel.Rotate(0, 0, spinSpeed * Time.deltaTime);

            // º≠º≠»˜ º”µµ∏¶ ¡Ÿ¿”
            spinSpeed = Mathf.Lerp(spinSpeed, 0, Time.deltaTime * 1f);

            // ∏ÿ√„
            if (spinSpeed < 1f)
            {
                isSpinning = false;
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