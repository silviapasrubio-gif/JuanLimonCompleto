using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FantasmaDucha : MonoBehaviour
{
    [Header("UI de atrapado")]
    public Image imagenCaughtUI;       // UI Image "Caught"
    public float tiempoReinicio = 2f;  // Segundos antes de reiniciar

    [Header("Movimiento")]
    public Transform jugador;          // Referencia al jugador
    private NavMeshAgent agente;

    [Header("Animaciones")]
    public Animator animator;           // Animator del fantasma

    private bool persiguiendo = false;  // Solo activa cuando el jugador entra al baño

    private void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        if (animator == null)
            animator = GetComponent<Animator>();

        // Ocultar la UI al inicio
        if (imagenCaughtUI != null)
            imagenCaughtUI.gameObject.SetActive(false);

        // Mantener al agente detenido hasta que el jugador entre al baño
        agente.isStopped = true;
    }

    private void Update()
    {
        if (persiguiendo && jugador != null)
        {
            // Activar agente si estaba detenido
            if (agente.isStopped)
                agente.isStopped = false;

            // Actualizar destino solo si cambió lo suficiente
            if (Vector3.Distance(agente.destination, jugador.position) > 0.5f)
                agente.SetDestination(jugador.position);

            // Animación de persecución
            if (animator != null)
                animator.SetBool("Persiguiendo", true);
        }
    }

    // Trigger del baño: activa la persecución
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            persiguiendo = true;
        }
    }

    // ===== CAMBIO: ahora usamos OnCollisionEnter para matar al jugador solo si lo toca físicamente =====
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Mostrar UI "Caught"
            if (imagenCaughtUI != null)
            {
                imagenCaughtUI.transform.SetAsLastSibling(); // Asegura que se dibuje encima
                imagenCaughtUI.gameObject.SetActive(true);
            }

            // Detener agente y animaciones
            if (agente != null)
                agente.isStopped = true;

            if (animator != null)
                animator.enabled = false;

            // Reiniciar nivel después de tiempoReinicio
            Invoke(nameof(ReiniciarNivel), tiempoReinicio);
        }
    }

    private void ReiniciarNivel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}