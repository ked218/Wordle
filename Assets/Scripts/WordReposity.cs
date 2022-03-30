using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordReposity : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The text asset with the words")]
    TextAsset m_wordList = null;

    List<string> m_words = null;

    [SerializeField]
    [Tooltip("The text asset with the level")]
    TextAsset m_wordLevel = null;

    List<string> m_wordsCheck = null;

    private void Awake()
    {
        m_words = new List<string>(m_wordList.text.Split(new char[] { ',', ' ', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries));

        m_wordsCheck = new List<string>(m_wordLevel.text.Split(new char[] { ',', ' ', '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries));
    }

    public string GetRandomWord()
    {
        return m_wordsCheck[Random.Range(0, m_wordsCheck.Count)];
    }

    public bool CheckWordExists(string word)
    {
        return m_words.Contains(word);
    }

}
