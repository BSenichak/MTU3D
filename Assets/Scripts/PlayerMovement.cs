using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 30f;
    [SerializeField] float playerSpeed = 10f;
    [SerializeField] float jumpStrength = 0.5f;
    [SerializeField] ParticleSystem particles;
    [SerializeField] Transform gunTransform;

    private Rigidbody rb;
    private GameObject camera;
    private Coroutine shoot;
    private float verticalRotation = 0f;

    int gunDamage = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camera = Camera.main.gameObject;
        gunTransform = camera.transform.GetChild(0); // якщо впевнений, що це саме той об'єкт
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camera.transform.localEulerAngles = Vector3.zero;
    }


    void Update()
    {
        HandleLook();
        HandleMove();
        HandleJump();
        HandleShoot();
    }

    void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        verticalRotation += mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -60f, 60f);
        camera.transform.localEulerAngles = new Vector3(-verticalRotation, 0f, 0f);
    }

    void HandleMove()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        Vector3 moveDir = transform.TransformDirection(input).normalized;
        Vector3 moveAmount = moveDir * playerSpeed * Time.deltaTime;

        rb.MovePosition(rb.position + moveAmount);
    }


    void HandleJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        }
    }

    void HandleShoot()
    {
        if (Input.GetMouseButtonDown(0) && shoot == null)
        {
            shoot = StartCoroutine(Shoot());
        }
        if (Input.GetMouseButtonUp(0) && shoot != null)
        {
            StopCoroutine(shoot);
            shoot = null;
        }
    }

    IEnumerator Shoot()
    {
        while (true)
        {
            gunTransform.localEulerAngles = new Vector3(0f, 90f, Random.Range(0f, 10f));
            particles.Play();

            Ray ray = new Ray(camera.transform.position, camera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if(hitInfo.transform.gameObject.tag == "Enemy")
                {
                    GameObject enemy = hitInfo.transform.gameObject;
                    enemy.GetComponent<EnemyScript>().GetDamage(hitInfo.point, gunDamage);
                }
            }

            yield return new WaitForSeconds(0.05f);
            gunTransform.localEulerAngles = new Vector3(0f, 90f, 0f);
        }
    }

    private void OnDrawGizmos()
    {
        if (camera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(camera.transform.position, camera.transform.forward * 10f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Finish")
        {
            StartCoroutine(PremiumGun());
            Destroy(other.gameObject);
        }
    }

    IEnumerator PremiumGun()
    {
        gunDamage = 3;
        gunTransform.GetComponent<MeshRenderer>().material.color = Color.yellow;
        yield return new WaitForSeconds(10f);
        gunDamage = 1;
        gunTransform.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
