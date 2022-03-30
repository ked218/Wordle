using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Test")]
public class Testing : ScriptableObject
{
    public TestUnit[] tests;
    double mSec = 3800;
    public void Test()
    {
        int passCount = 0;
        for(int i = 0; i < tests.Length; ++i)
        {
            DateTime start = DateTime.Now;
            int hash = ("result"+Count(tests[i])).GetHashCode();
            double time = (DateTime.Now - start).TotalMilliseconds;
            if (hash == tests[i].resultHash && time < mSec)
            {
                Debug.Log($"Test {i}: Pass");
                passCount++;
            }
            else
            {
                Debug.Log($"Test {i}: Not Pass");
            }
        }
        Debug.Log($"Pass : {passCount}/{tests.Length}");
    }

    public int Count(TestUnit unit)
    {
       // Code here
        
        return 0;
    }
}

[System.Serializable]
public struct TestUnit
{
    public int[] values;
    public int target;
    public int resultHash;
}