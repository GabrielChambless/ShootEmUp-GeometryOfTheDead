using System.Collections;
using UnityEngine;

public class CorpsePieces : MonoBehaviour
{
    private float timeUntilDestroy = 5f;
    private bool timeReached = false;


    private void Update()
    {
        timeUntilDestroy -= Time.deltaTime;

        if (timeUntilDestroy <= 0 && timeReached == false)
        {
            timeReached = true;

            StartCoroutine(ShrinkAway());   // shrink piece before destroying it to make it look nicer
        }
    }

    private IEnumerator ShrinkAway()
    {
        for (int i = 0; i < 20; i++)
        {
            transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(transform.parent.gameObject);
        Destroy(gameObject);
    }
}
