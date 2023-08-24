using UnityEngine;

public class Socket : MonoBehaviour
{
    [SerializeField] public PCComponent PCComponentTypeTemplate;

    private PCComponent _appended;

    public bool CanAppend(PCComponent pCComponent)
    {
        if (_appended != null)
            return false;

        Debug.Log(PCComponentTypeTemplate.GetType());
        Debug.Log(pCComponent.GetType());

        return PCComponentTypeTemplate.GetType() == pCComponent.GetType(); 
    }

    public void Append(PCComponent pCComponent)
    {
        _appended = pCComponent;
    }
}
