using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIFocus : MonoBehaviour
{
    [SerializeField] private GameObject firstSelected;

    private IEnumerator Start()
    {
        yield return null; 

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    private void OnEnable()
    {
        StartCoroutine(SetFocus());
    }

    private IEnumerator SetFocus()
    {
        yield return null;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}