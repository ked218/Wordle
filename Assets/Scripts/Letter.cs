using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Letter : MonoBehaviour
{
    readonly int k_animatorResetTrigger = Animator.StringToHash("Reset");
    readonly int k_animatorShakeTrigger = Animator.StringToHash("Shake");
    readonly int k_animatorStateParamter = Animator.StringToHash("State");

    Animator m_animator = null;
    Text m_Text = null;

    public char? Entry { get; private set; } = null;
    public LetterState LetterState { get; private set; } = LetterState.Unknow;


    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_Text = GetComponentInChildren<Text>();
    }

    void Start()
    {
        m_Text.text = null;
    }

    public void EnterLetter(char c)
    {
        Entry = c;
        m_Text.text = c.ToString().ToUpper();
    }

    public void DeleteLetter()
    {
        Entry = null;
        m_Text.text = null;
        m_animator.SetTrigger(k_animatorResetTrigger);
    }

    public void Shake()
    {
        m_animator.SetTrigger(k_animatorShakeTrigger);
    }

    public void SetState(LetterState letterState)
    {
        LetterState = letterState;
        m_animator.SetInteger(k_animatorStateParamter, (int)letterState);
    }

    public void Clear()
    {
        LetterState = LetterState.Unknow;
        m_animator.SetInteger(k_animatorStateParamter, -1);
        m_animator.SetTrigger(k_animatorResetTrigger);
        Entry = null;
        m_Text.text = null;

    }

}
