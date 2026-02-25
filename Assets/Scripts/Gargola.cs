using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GargolaIA_Raycast : MonoBehaviour
{
    [Header("Linterna y detección")]
    public Transform origenLinterna; // La punta de la linterna
    public float distanciaVision = 10f;
    public float anguloVision = 30f; // ángulo de cono de luz
    public string tagJugador = "Player";

    [Header("UI")]
    public Image imagenDerrota; // Aquí arrastras tu Image de la escena
    public float tiempoReinicio = 2f;

    private bool jugadorDetectado = false;

    void Start()
    {
        if (imagenDerrota != null)
        {
            // Asegurarse de que está desactivada al inicio
            imagenDerrota.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("No se ha asignado ninguna Image UI de derrota en el Inspector.");
        }

        // Asegurarse que hay origen de linterna
        if (origenLinterna == null)
            origenLinterna = transform;
    }

    void Update()
    {
        if (jugadorDetectado) return;

        DetectarJugador();
    }

    void DetectarJugador()
    {
        GameObject jugador = GameObject.FindGameObjectWithTag(tagJugador);
        if (jugador == null) return;

        Vector3 direccion = (jugador.transform.position - origenLinterna.position).normalized;
        float angulo = Vector3.Angle(origenLinterna.forward, direccion);

        if (angulo < anguloVision / 2f)
        {
            if (Physics.Raycast(origenLinterna.position, direccion, out RaycastHit hit, distanciaVision))
            {
                if (hit.collider.CompareTag(tagJugador))
                {
                    CapturarJugador();
                }
            }
        }
    }

    void CapturarJugador()
    {
        jugadorDetectado = true;

        if (imagenDerrota != null)
        {
            imagenDerrota.transform.SetAsLastSibling(); // asegurarse de que esté encima de otros elementos
            imagenDerrota.gameObject.SetActive(true);
        }

        Invoke(nameof(ReiniciarNivel), tiempoReinicio);
    }

    void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDrawGizmosSelected()
    {
        if (origenLinterna == null) return;

        Gizmos.color = Color.yellow;
        Vector3 forward = origenLinterna.forward * distanciaVision;
        Gizmos.DrawRay(origenLinterna.position, forward);
        Vector3 derecha = Quaternion.Euler(0, anguloVision / 2f, 0) * forward;
        Vector3 izquierda = Quaternion.Euler(0, -anguloVision / 2f, 0) * forward;
        Gizmos.DrawRay(origenLinterna.position, derecha);
        Gizmos.DrawRay(origenLinterna.position, izquierda);
    }
}