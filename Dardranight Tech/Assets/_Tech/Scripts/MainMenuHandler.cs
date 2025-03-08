using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;


public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private Selectable[] m_selecteables;

    [SerializeField] private Selectable m_firstSelected;
    [SerializeField] private Selectable m_lastSelected;
    

    private float m_selectedAnimationScale = 1.1f;
    private float m_scaleDuration = 0.25f;

    private Dictionary<Selectable, Vector3> m_originalScales = new Dictionary<Selectable, Vector3>();

    private Tween m_scaleUpTween;
    private Tween m_scaleDownTween;

    private void Awake()
    {
        foreach (var selectable in m_selecteables)
        {
            m_originalScales.Add(selectable, selectable.transform.localScale);
            AddListeners(selectable);
        }

//        EventSystem.current.SetSelectedGameObject(m_firstSelectable.gameObject);
    }

    private void AddListeners(Selectable selectable)
    {
        if (!selectable.TryGetComponent<EventTrigger>(out var trigger))
        {
            trigger = selectable.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry selectEntry = new EventTrigger.Entry { eventID = EventTriggerType.Select };
        selectEntry.callback.AddListener(OnSelect);
        trigger.triggers.Add(selectEntry);

        EventTrigger.Entry deselectEntry = new EventTrigger.Entry { eventID = EventTriggerType.Deselect };
        deselectEntry.callback.AddListener(OnDeselect);
        trigger.triggers.Add(deselectEntry);

        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        pointerEnterEntry.callback.AddListener(OnPointerEnter);
        trigger.triggers.Add(pointerEnterEntry);

        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        pointerExitEntry.callback.AddListener(OnPointerExit);
        trigger.triggers.Add(pointerExitEntry);
    }

    private void Update()
    {
        if(UserInputsManager.Instance.UINavigate)
        {
            OnNavigate();
        }
    }

    private void OnSelect(BaseEventData eventData)
    {
        m_lastSelected = eventData.selectedObject.GetComponent<Selectable>();
        Vector3 newScale = eventData.selectedObject.transform.localScale * m_selectedAnimationScale;
        m_scaleUpTween = eventData.selectedObject.transform.DOScale(newScale, m_scaleDuration);
    }

    private void OnDeselect(BaseEventData eventData)
    {
        var selectable = eventData.selectedObject.GetComponent<Selectable>();
        m_scaleDownTween = selectable.transform.DOScale(m_originalScales[selectable], m_scaleDuration);
    }

    private void OnPointerEnter(BaseEventData eventData)
    {
        PointerEventData pointerEventData = (PointerEventData)eventData;
        if (pointerEventData != null)
        {
            pointerEventData.selectedObject = pointerEventData.pointerEnter;
        }
    }

    private void OnPointerExit(BaseEventData eventData)
    {
        PointerEventData pointerEventData = (PointerEventData)eventData;
        if (pointerEventData != null)
        {
            pointerEventData.selectedObject = null;
        }
    }

    private void OnNavigate()
    {
        if(EventSystem.current.currentSelectedGameObject == null && m_lastSelected!=null)
        {
            EventSystem.current.SetSelectedGameObject(m_lastSelected.gameObject);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < m_selecteables.Length; i++)
        {
            m_selecteables[i].transform.localScale = m_originalScales[m_selecteables[i]];
        }
    }

    private void OnDisable()
    {
        m_scaleUpTween?.Kill();
        m_scaleDownTween?.Kill();
    }
}