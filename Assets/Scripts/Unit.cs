using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private GameObject m_SelectedIndicator;

    // Start is called before the first frame update
    void Start()
    {
        InteractionHandler.s_Instance.a_UnitSelected += SetSelected;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetSelected(InteractionHandler.UnitSelectedArgs args)
    {
        if (args.m_SelectedUnit == this)
        {
            m_SelectedIndicator.SetActive(true);
        }
        else
        {
            m_SelectedIndicator.SetActive(false);
        }
    }
}
