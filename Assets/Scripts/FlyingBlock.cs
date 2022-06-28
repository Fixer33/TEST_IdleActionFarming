using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlyingBlock : MonoBehaviour
{
    [SerializeField] private float speed;

    private Transform _target;
    private UnityEvent onArrival = new UnityEvent();
    private bool _move = false;

    public void StartMovement(Transform target, params UnityAction[] actionsOnArrival)
    {
        _target = target;
        for (int i = 0; i < actionsOnArrival.Length; i++)
        {
            onArrival.AddListener(actionsOnArrival[i]);
        }
        
        _move = true;
    }

    private void Update()
    {
        if (_move == false)
            return;

        if (_target.position == transform.position)
        {
            onArrival.Invoke();
            onArrival.RemoveAllListeners();
            DestroyObject();
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, _target.position, speed * Time.deltaTime);
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
