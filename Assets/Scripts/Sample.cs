using UnityEngine;

public class Sample : MonoBehaviour
{
    void Start()
    {
        GameObject original = null;
        GameObject duplicate = original.gameObject; // UNT0019
        Debug.Log(duplicate.ToString());

        method2();
    }

    void Update()
    {
        // UNT0001
    }

    public void method2() // IDE1006
    {
        Debug.Log("This is method 2");
    }

    public void Method1()
    {

    }
}
