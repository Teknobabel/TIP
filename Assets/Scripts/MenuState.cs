using UnityEngine;
using System.Collections;

public abstract class MenuState : MonoBehaviour {
	
	public enum State
	{
		None,
		MainMenu,
		Pause,
		GameOver,
		GameState,
		Pause_MacPC,
	}

	public State m_state = State.None;

	public abstract void OnActivate(); // player enters menu
	public abstract void OnHold(); // another state is pushed on top, but player can return
	public abstract void OnDeactivate(); // player leaves menu for previous state
	public abstract void OnUpdate();

	public virtual void DisplayActionPane (){
	}

	public virtual void HideActionPane (){
	}

	public virtual void BackButtonPressed (){
	}

	public virtual void OnReturn()
	{
		// continue going back up the stack if this is not the target menu

//		if (GameManager.instance.targetMenuState != MenuState.State.None && GameManager.instance.targetMenuState != m_state)
//		{
//			GameManager.instance.PopMenuState();
//			return;
//		} else if (GameManager.instance.targetMenuState != MenuState.State.None && GameManager.instance.targetMenuState == m_state)
//		{
//			GameManager.instance.targetMenuState = MenuState.State.None;
//		}
	}

	public State state {get{return m_state;}}
}
