using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]

public class Key : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Keycode representing a key")]
    KeyCode m_keyCode = KeyCode.None;

    [Serializable]
    public struct LetterStateColor
    {
        public LetterState LetterState;
        public Color Color;
    }

    [SerializeField]
    [Tooltip("Color per state")]
    LetterStateColor[] m_letterStateColors = null;

    Image m_image = null;

    Color m_startingColor = Color.grey;

    public Action<KeyCode> Pressed;

    public KeyCode KeyCode { get { return m_keyCode; } }

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
        Text text = GetComponentInChildren<Text>();
        if (text && string.IsNullOrEmpty(text.text))
            text.text = m_keyCode.ToString();

        m_image = GetComponent<Image>();
        m_startingColor = m_image.color;
    }

    private void OnButtonClick()    
    {
        Pressed?.Invoke(m_keyCode);
    }

    public void SetState(LetterState letterState)
    {
        foreach(LetterStateColor letterStateColor in m_letterStateColors)
        {
            if (letterStateColor.LetterState == letterState)
            {
                m_image.color = letterStateColor.Color;
                break;
            }  
        }
    }

    public void ResetState()
    {
        m_image.color = m_startingColor;
    }
}
