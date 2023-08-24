using System;
using System.Collections.Generic;
using UnityEngine;

public class Motherboard : MonoBehaviour
{
    [SerializeField] private List<Socket> _sockets;

    public event Action<Socket, PCComponent> OnAppended;

    public bool TryAppend(PCComponent pCComponent)
    {
        foreach (Socket socket in _sockets)
        {
            if (socket.CanAppend(pCComponent))
            {
                socket.Append(pCComponent);
                OnAppended(socket, pCComponent);
                return true;
            }
        }

        return false;
    }

    public void Append(GameObject pCComponent)
    {
        if (pCComponent.TryGetComponent<PCComponent>(out PCComponent component))
        {
            if(TryAppend(component) == false)
                throw new ArgumentException("Alredy appended");
        }
        else
        {
            throw new ArgumentException("GameObject must contains PCComponent");
        }
    }
}