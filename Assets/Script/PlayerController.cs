using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public GameObject destinationPrefab;
    RaycastHit hit;
    public float knockRadius = 20.0f;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DestroyDestination();
        }

        if (Input.GetMouseButtonUp(0))
        {
            animator.SetBool("Idle", false);
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                this.GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(hit.point);

                SpawndDestination();
            }

        }
        else if (this.GetComponent<UnityEngine.AI.NavMeshAgent>().remainingDistance < 0.1f)
        {
            animator.SetBool("Idle", true);
            DestroyDestination();

        }

        //---------------
        if (Input.GetKey("space"))
        {
            StartCoroutine(PlayKnock()); // Play audio file

            // Create the sphere collider
            Collider[] hitColliders = Physics.
            OverlapSphere(transform.position, knockRadius);
            for (int i = 0; i < hitColliders.Length; i++)
            // check the collisions
            {
                // If it's a guard, trigger the Investigation!
                if (hitColliders[i].tag == "guard")
                {
                    hitColliders[i].GetComponent<GuardController>().InvestigatePoint(this.transform.position);

                }

            }

        }
    }

    IEnumerator PlayKnock()
    {
        AudioSource audio = GetComponent<AudioSource>();

        audio.Play();
        yield return new WaitForSeconds(audio.clip.length);
    }

    void SpawndDestination()
    {
        Instantiate(destinationPrefab, hit.point + new Vector3(0, 1, 0), Quaternion.identity);
    }
    void DestroyDestination()
    {
        GameObject distinationObject = GameObject.FindGameObjectWithTag("Destination");
        Destroy(distinationObject);
    }

}
