using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Guy : MonoBehaviour
{
    public int speed = 10;
    [FormerlySerializedAs("radius")] public int detectingRadius = 5;
    [SerializeField] private List<Guy> closeGuys = new List<Guy>();
    public MeshRenderer hat;
    
    private CharacterController _controller;
    
    private Vector3 _currentDirection;
    private Vector3 _lookAt = Vector3.zero;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        ChangeDirection();
        
        var sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = detectingRadius;
        hat.material.color = Random.ColorHSV(0, 1, 0.5f, 1,0.8f,1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeDirection();
        }

        if (closeGuys.Count > 0)
        {
            _lookAt = Vector3.Lerp(_lookAt, GetClosestPlayer(), Time.deltaTime * 7);
        }
        else
        {
            _lookAt = Vector3.Lerp(_lookAt, _currentDirection * 5 + transform.position, Time.deltaTime * 6);
        }

        transform.LookAt(_lookAt);
        _controller.SimpleMove(_currentDirection * speed);
    }

    private Vector3 GetClosestPlayer()
    {
        var result = Vector3.zero;
        float shortest = detectingRadius+1;
        foreach (var player in closeGuys)
        {
            var dist = Vector3.Distance(transform.position, player.transform.position);
            if (!(dist < shortest)) continue;
            shortest = dist;
            result = player.transform.position;
        }

        return result;
    }

    private void ChangeDirection()
    {
        _currentDirection = (new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f))).normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        var guy = other.GetComponent<Guy>();
        if (!guy || guy == this || closeGuys.Contains(guy)) return;
        closeGuys.Add(guy);
    }

    private void OnTriggerExit(Collider other)
    {
        var guy = other.GetComponent<Guy>();
        if (!guy || !closeGuys.Contains(guy)) return;
        closeGuys.Remove(guy);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Ground")) return;
        ChangeDirection();
    }
}