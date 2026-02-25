using UnityEngine;
using UnityEngine.XR;

public class Limoncio : MonoBehaviour
{
    private Animator animacion;
    private Rigidbody rb;

    //VARIABLES MOVIMIENTO
    public float velocidadMovimiento = 5f;
    public float velocidadGiro = 100;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        animacion = GetComponent<Animator>();

        // BLOQUEAR EL CURSOR
        Cursor.lockState = CursorLockMode.Locked; // captura el ratón
        Cursor.visible = false;                     // oculta el cursor
    }
    private void Update()
    {
        animacion.SetBool("Walk", Input.GetKey(KeyCode.W));
        animacion.SetBool("WalkB", Input.GetKey(KeyCode.S));
        animacion.SetBool("Izq", Input.GetKey(KeyCode.A));
        animacion.SetBool("Der", Input.GetKey(KeyCode.D));

    }
    private void FixedUpdate()
    {
        //MOVIMIENTO
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 direccion = (transform.forward * v + transform.right * h).normalized;
        rb.linearVelocity = new Vector3(direccion.x * velocidadMovimiento, rb.linearVelocity.y, direccion.z * velocidadMovimiento);

        //GIRO
        float giro = Input.GetAxis("Mouse X");

        float giroY = giro * velocidadGiro * Time.fixedDeltaTime;
        Quaternion rotacion = Quaternion.Euler(0.0f, giroY, 0.0f);
        rb.MoveRotation(rb.rotation * rotacion);

    }
}
