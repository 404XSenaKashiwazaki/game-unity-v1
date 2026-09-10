using UnityEngine;
using System.Collections;

public class ReplayManager : MonoBehaviour
{
    public static ReplayManager Instance;

    [Header("Cameras")]
    public Camera mainCamera;
    public Camera replayTopCamera;
    public Camera replayFollowCamera;

    public CameraFollow followCam;

    

    void Awake()
    {
        Instance = this;
       
    }

    public void StartReplay(Transform drone, Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(ReplayEnemy(drone, collision.transform));
        }
        else
        {
            StartCoroutine(ReplayTop(drone));
        }
    }

    IEnumerator ReplayTop(Transform drone)
    {
        SwitchCamera(replayTopCamera);
        followCam.target = drone;

        yield return new WaitForSeconds(2.5f);

        EndReplay();
    }

    IEnumerator ReplayEnemy(Transform drone, Transform enemy)
    {
        SwitchCamera(replayFollowCamera);
        followCam.target = enemy;

        yield return new WaitForSeconds(0.8f);
        Explosion(enemy.position);

        yield return new WaitForSeconds(2f);

        EndReplay();
    }

    void SwitchCamera(Camera cam)
    {
        mainCamera.enabled = false;
        replayTopCamera.enabled = false;
        replayFollowCamera.enabled = false;

        cam.enabled = true;
    }

    void EndReplay()
    {
        replayTopCamera.enabled = false;
        replayFollowCamera.enabled = false;
        mainCamera.enabled = true;
    }

    void Explosion(Vector3 pos)
    {
        // Debug.Log("BOOM!");
        // Instantiate(explosionPrefab, pos, Quaternion.identity);
    }
}
