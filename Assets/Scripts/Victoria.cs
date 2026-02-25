using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Victoria : MonoBehaviour
{
    [Header("UI")]
    public Image imagenVictoria; // UI Image
    public float tiempoReinicio = 2f;

    private bool haGanado = false;

    private void Start()
    {
        if (imagenVictoria != null)
        {
            imagenVictoria.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!haGanado && other.CompareTag("Player"))
        {
            haGanado = true;
            StartCoroutine(SecuenciaVictoria());
        }
    }

    IEnumerator SecuenciaVictoria()
    {
        if (imagenVictoria != null)
        {
            // La ponemos por encima de todo en el Canvas
            imagenVictoria.transform.SetAsLastSibling();
            imagenVictoria.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(tiempoReinicio);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}