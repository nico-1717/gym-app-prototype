using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    private List<Action> functionStack;

    public NavigationManager()
    {
        functionStack = new List<Action>();
    }

    public void NavigateTo(Action function)
    {
        if (functionStack.Count > 0)
        {
            if (functionStack[functionStack.Count - 1] != function)
            {
                functionStack.Add(function);
            }
        }
        else
            functionStack.Add(function);
        function();
    }

    public void NavigateBack()
    {
        int count = functionStack.Count;
        if (count > 1)
        {
            functionStack.RemoveAt(count - 1);
            Action previousFunction = functionStack[count - 2];
            previousFunction();
        }
        AppManager.Instance.objects.ClosePopup();
    }

    public void RetryNavigation()
    {
        Action function = functionStack[functionStack.Count - 1];
        function();
    }
}
