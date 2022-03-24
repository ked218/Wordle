using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GameFlowManager : MonoBehaviour
{
    const int k_wordLength = 5;

    [SerializeField]
    [Tooltip("the word reposity")]
    WordReposity m_wordReposity = null;

    [SerializeField]
    [Tooltip("Prefabs for the letter")]
    Letter m_letterPrefab = null;

    [SerializeField]
    [Tooltip("Amount of rows")]
    int m_amountOfRows = 6;

    [SerializeField]
    [Tooltip("Grid parent")]
    GridLayoutGroup m_gridLayout = null;

    [SerializeField]
    [Tooltip("offset for letter animation")]
    float m_letterAnimationOffsetTime = .5f;

    [SerializeField]
    [Tooltip("Keys on keyboard")]
    Key[] m_keys = null;

    [SerializeField]
    [Tooltip("Score Point")]
    int score_point = 0;

    List <Letter> m_letters = null;
    int m_index = 0;
    int m_currentRow = 0;
    char?[] m_guess = new char?[k_wordLength];
    char[] m_word = new char[k_wordLength];

    public PuzzleState PuzzleState { get; private set; } = PuzzleState.InProgress;

    public Action Restarted;

    void Awake()
    {
        SetupGrid();

        foreach (Key key in m_keys)
            key.Pressed += OnKeyPressed;
    }

    void Start()
    {
        SetWord();
    }

    void Update()
    {
        if (Input.anyKeyDown)
            ParseInput(Input.inputString);
    }

    public void Restart()
    {
        PuzzleState = PuzzleState.InProgress;

        foreach (Letter letter in m_letters)
            letter.Clear();

        foreach (Key key in m_keys)
            key.ResetState();

        m_index = 0;
        m_currentRow = 0;

        for (int i = 0; i < k_wordLength; i++)
            m_guess[i] = null;

        SetWord();

        Restarted?.Invoke();
    }

    void OnKeyPressed(KeyCode keyCode)
    {
        Debug.Log("Is it work?");
        if(PuzzleState != PuzzleState.InProgress)
        {
            if (keyCode == KeyCode.Return)
                Restart();

            return;
        }

        if(keyCode == KeyCode.Return)
        {
            GuessWord();
        }
        else if(keyCode == KeyCode.Backspace || keyCode == KeyCode.Delete)
        {
            DeleterLetter();
        }
        else if(keyCode >= KeyCode.A && keyCode <= KeyCode.Z)
        {
            int index = keyCode - KeyCode.A;
            
            EnterLetter((char)((int)'A' + index));

        }

    }

    public void ParseInput(string value)
    {
        if (PuzzleState != PuzzleState.InProgress)
        {
            foreach (char c in value)
            {
                if ((c == '\n') || (c == '\r'))
                {
                    Restart();
                }
            }
            return;
        }

        foreach (char c in value)
        {
            if (c == '\b') //backspace
            {
                DeleterLetter();
            }
            else if ((c == '\n') || (c == '\r')){
                GuessWord();
            }
            else
            {
                EnterLetter(c);
            }
        }


    }

    public void SetupGrid()
    {
        if (m_letters == null)
            m_letters = new List<Letter>();

        for(int i = 0; i < m_amountOfRows; ++i)
        {
            for(int j = 0; j < k_wordLength; j++)
            {
                Letter letter = Instantiate<Letter>(m_letterPrefab);
                letter.transform.SetParent(m_gridLayout.transform);
                m_letters.Add(letter);
            }
        }
    }

    public void SetWord()
    {
        string word = m_wordReposity.GetRandomWord();
        for (int i = 0; i < word.Length; i++)
            m_word[i] = word[i];
    }

    public string GetWord()
    {
        return new string(m_word);
    }

    public void EnterLetter(char c)
    {
        if(m_index < k_wordLength)
        {
            c = char.ToUpper(c);

            m_letters[(m_currentRow * k_wordLength) + m_index].EnterLetter(c);
            m_guess[m_index] = c;
            m_index++;
        }
    }

    public void DeleterLetter()
    {
        if(m_index > 0)
        {
            m_index--;
            m_letters[(m_currentRow * k_wordLength) + m_index].DeleteLetter();
            m_guess[m_index] = null;
        }
    }

    public void Shake()
    {
        for(int i=0; i < k_wordLength; i++)
        {
            m_letters[(m_currentRow * k_wordLength) + i].Shake();
        } 
    }

    public void GuessWord()
    {
        if(m_index != k_wordLength)
        {
            Shake();
        }
        else
        {
            StringBuilder word = new StringBuilder();
            for(int i = 0; i < k_wordLength; i++)
                word.Append(m_guess[i].Value);
            if (m_wordReposity.CheckWordExists(word.ToString()))
            {
                bool incorrect = false;
                
                for(int i=0; i < k_wordLength; i++)
                {
                    bool correct = m_guess[i] == m_word[i];

                    //Check letter in word if it have change color
                    if (!correct)
                    {
                        //It is the correct word in data then check
                        incorrect = true;
                        //It is still have in data
                        bool letterExistsInWord = false;
                        //The loop check the letter in word
                        for(int j=0; j < k_wordLength; j++)
                        {
                            //If the letter exist so assign it to the m_word[j]
                            letterExistsInWord = m_guess[i] == m_word[j];
                            if (letterExistsInWord)
                                break;
                        }

                        StartCoroutine(PlayLetter(i * m_letterAnimationOffsetTime, (m_currentRow * k_wordLength) + i, letterExistsInWord ? LetterState.WrongLocation : LetterState.Incorrect));

                    }

                    else
                    {
                        StartCoroutine(PlayLetter(i * m_letterAnimationOffsetTime, (m_currentRow * k_wordLength) + i, LetterState.Correct));
                    }
                }


                //If they guess 6 times and still wrong so the game will be end
                if (incorrect)
                {
                    m_index = 0;
                    m_currentRow++;
                    if(m_currentRow >= m_amountOfRows)
                    {
                        PuzzleState = PuzzleState.Failed;
                    }
                }

                //Or the game will be complete
                else
                {
                    //If they have win the game will receive 6 point

                    PuzzleState = PuzzleState.Complete;
                }

            }
        }
    }

    IEnumerator PlayLetter(float offset, int index, LetterState letterState)
    {
        yield return new WaitForSeconds(offset);
        m_letters[index].SetState(letterState);

        int indexOfChar = (int)m_letters[index].Entry.Value - (int)'A';

        KeyCode keyCode = indexOfChar + KeyCode.A;

        foreach (Key key in m_keys)
        {
            if(key.KeyCode == keyCode)
            {
                key.SetState(letterState);
                break;
            }
        }
    }

}
