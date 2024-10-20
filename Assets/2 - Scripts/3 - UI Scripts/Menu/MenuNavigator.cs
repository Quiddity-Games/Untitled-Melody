using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuNavigator : MonoBehaviour
{
    [SerializeField] private BaseSubMenu _currSubMenu;

    [SerializeField] private PlayerInput _input;

    public Action onExitMenu;
    // Start is called before the first frame update
    void OnEnable()
    {
        _input.actions["UI/Cancel"].started += new Action<InputAction.CallbackContext>(this.Back);
    }

    public void Back(InputAction.CallbackContext context)
    {
        Back();}

    public PlayerInput GetPlayerInput()
    {
        return _input;
    }

    public void Reset()
    {
        if(this._currSubMenu != (UnityEngine.Object)null)
        {
            this._currSubMenu.gameObject.SetActive(false);
        }
    
        this._currSubMenu = null;
        onExitMenu = null;

    }

    public void Back()
    {

        if ((UnityEngine.Object) this._currSubMenu == (UnityEngine.Object) null){

        }
        else if ((UnityEngine.Object) this._currSubMenu.aboveMenu == (UnityEngine.Object) null)
        {
            _input.actions["UI/Cancel"].started -= new Action<InputAction.CallbackContext>(this.Back);
            EventSystem.current.SetSelectedGameObject((GameObject) null);
            this._currSubMenu.gameObject.SetActive(false);
            onExitMenu?.Invoke();
            onExitMenu = null;
        }
        else
        {
            GameObject aboveButton = this._currSubMenu.aboveButton;
            BaseSubMenu aboveMenu = this._currSubMenu.aboveMenu;
            this._currSubMenu.gameObject.SetActive(false);
            aboveMenu.gameObject.SetActive(true);
            this._currSubMenu = aboveMenu;
            EventSystem.current.SetSelectedGameObject((GameObject) null);
            EventSystem.current.SetSelectedGameObject(aboveButton);
        }

    }

    public void ChangeSubMenu(BaseSubMenu subMenu)
    {
        if ((UnityEngine.Object) this._currSubMenu != (UnityEngine.Object) null){
            this._currSubMenu.gameObject.SetActive(false);
        }
        subMenu.gameObject.SetActive(true);
        subMenu.aboveMenu = _currSubMenu;
        subMenu.aboveButton = EventSystem.current.currentSelectedGameObject;
        _currSubMenu = subMenu;

        if ((UnityEngine.Object) EventSystem.current != (UnityEngine.Object) null)
        {
            EventSystem.current.SetSelectedGameObject((GameObject) null);
            EventSystem.current.SetSelectedGameObject(this._currSubMenu.firstButton);
        }
    }

}
