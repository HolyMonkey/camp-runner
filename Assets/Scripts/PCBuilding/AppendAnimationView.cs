using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Motherboard))]
public class AppendAnimationView : MonoBehaviour
{
    [Header("In seconds")]
    [SerializeField] private float _appendTime;

    [SerializeField] private AnimationCurve _scaleCurve;

    private Dictionary<Socket, Coroutine> _transtions = new Dictionary<Socket, Coroutine>();
    private Motherboard _motherboard;

    private void Awake()
    {
        _motherboard = GetComponent<Motherboard>();
    }

    private void OnEnable()
    {
        _motherboard.OnAppended += Play;
    }

    private void OnDisable()
    {
        _motherboard.OnAppended -= Play;
    }

    public void Play(Socket socket, PCComponent component)
    {
        if (_transtions.ContainsKey(socket) == false)
            _transtions[socket] = null;

        if (_transtions[socket] != null)
            throw new InvalidOperationException($"Transition already start for socket {socket.gameObject.name}");

        _transtions[socket] = StartCoroutine(LinearTransition(socket, component));
    }

    private IEnumerator LinearTransition(Socket socket, PCComponent component)
    {
        float transitionTime = 0f;

        while (Reached() == false)
        {
            component.transform.position = Vector3.Lerp(component.transform.position, socket.transform.position, transitionTime / _appendTime);
            component.transform.rotation = Quaternion.Lerp(component.transform.rotation, socket.transform.rotation, transitionTime / _appendTime);
            component.transform.localScale = Vector3.Lerp(component.transform.localScale, Vector3.one, _scaleCurve.Evaluate(transitionTime / _appendTime));

            transitionTime += Time.deltaTime;
            yield return null;
        }

        component.transform.parent = socket.transform;
        
        bool Reached()
        {
            return socket.transform.position == component.transform.position &&
                   socket.transform.rotation == component.transform.rotation;
        }
    }
}
