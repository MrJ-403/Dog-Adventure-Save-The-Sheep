using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Samples.RebindUI;

public class RebindPanel : MonoBehaviour
{
    public InputActionReference left;
    public InputActionReference right;
    public InputActionReference shift;
    public InputActionReference space;
    public InputActionReference tab;
    public InputActionReference interact;

    public InputActionAsset actionAsset;
    public bool isInMainMenu = false;

    // Start is called before the first frame update
    void OnEnable()
    {
        left.action.Disable();
        right.action.Disable();
        space.action.Disable();
        tab.action.Disable();
        shift.action.Disable();
        interact.action.Disable();
        var rebinds = PlayerPrefs.GetString("BindingOverrides");
        if (!string.IsNullOrEmpty(rebinds))
            actionAsset.LoadBindingOverridesFromJson(rebinds);
        //actionAsset.LoadBindingOverridesFromJson(PlayerPrefs.HasKey("BindingOverrides") ? PlayerPrefs.GetString("BindingOverrides") : "");
        RebindActionUI[] comps = GetComponentsInChildren<RebindActionUI>();
        for(int i = 0; i < comps.Length; i++)
        {
            comps[i].UpdateBindingDisplay();
        }
    }
    void OnDisable()
    {
        if(isInMainMenu){
            left.action.Enable();
            right.action.Enable();
            space.action.Enable();
            tab.action.Enable();
            shift.action.Enable();
            interact.action.Enable();
        }
    }

    public void onRebindClicked() 
    {
        PlayerPrefs.SetString("BindingOverrides",actionAsset.SaveBindingOverridesAsJson());
        PlayerPrefs.Save();
        left.action.Disable();
        right.action.Disable();
        space.action.Disable();
        tab.action.Disable();
        shift.action.Disable();
        interact.action.Disable();
    }
}
