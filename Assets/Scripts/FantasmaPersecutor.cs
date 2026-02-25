using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FantasmaIA3D : MonoBehaviour
{
    [Header("Patrullaje")]
    public Transform puntoA;
    public Transform puntoB;
    public float velocidadPatrulla = 2f;
    public float velocidadRotacion = 5f;

    [Header("Persecución")]
    public float velocidadPersecucion = 4f;
    public float distanciaVision = 80f;
    public float distanciaCaptura = 1f;

    [Header("UI")]
    public Image imagenDerrota;
    public float tiempoReinicio = 2f;

    private Transform jugador;
    private Transform[] puntos;
    private int indiceActual = 0;

    private bool persiguiendo = false;
    private bool capturado = false;

    void Start()
    {
        jugador = GameObject.FindGameObjectWithTag("Player")?.transform;

        puntos = new Transform[2];
        puntos[0] = puntoA;
        puntos[1] = puntoB;

        indiceActual = 0;

        if (imagenDerrota != null)
            imagenDerrota.gameObject.SetActive(false);
    }

    void Update()
    {
        if (jugador == null || capturado) return;

        if (persiguiendo)
        {
            Perseguir();
        }
        else
        {
            Patrullar();
            DetectarJugador();
        }
    }

    void Patrullar()
    {
        Transform objetivo = puntos[indiceActual];

        RotarHacia(objetivo.position);

        transform.position = Vector3.MoveTowards(
            transform.position,
            objetivo.position,
            velocidadPatrulla * Time.deltaTime
        );

        float distancia = Vector3.Distance(transform.position, objetivo.position);

        if (distancia <= 0.1f)
        {
            indiceActual++;
            if (indiceActual >= puntos.Length)
                indiceActual = 0;
        }
    }

    void Perseguir()
    {
        RotarHacia(jugador.position);

        transform.position = Vector3.MoveTowards(
            transform.position,
            jugador.position,
            velocidadPersecucion * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, jugador.position) <= distanciaCaptura)
        {
            Capturar();
        }
    }

    void RotarHacia(Vector3 destino)
    {
        Vector3 direccion = destino - transform.position;
        direccion.y = 0f; // Solo rotación horizontal

        if (direccion == Vector3.zero) return;

        Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotacionObjetivo,
            velocidadRotacion * Time.deltaTime
        );
    }

    void DetectarJugador()
    {
        if (Vector3.Distance(transform.position, jugador.position) > distanciaVision)
            return;

        Vector3 direccion = (jugador.position - transform.position).normalized;

        RaycastHit hit;
        Vector3 origen = transform.position + Vector3.up * 1f;

        if (Physics.Raycast(origen, direccion, out hit, distanciaVision))
        {
            if (hit.collider.CompareTag("Player"))
            {
                persiguiendo = true;
            }
        }
    }

    void Capturar()
    {
        capturado = true;

        if (imagenDerrota != null)
        {
            imagenDerrota.transform.SetAsLastSibling();
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanciaVision);
    }
}