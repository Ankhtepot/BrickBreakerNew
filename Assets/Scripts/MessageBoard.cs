using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MessageBoard : MonoBehaviour {

    [System.Serializable]
    public enum DismmissType { OnMouseClick, ExternalTrigger }

    [SerializeField] bool isHint = true;
    [SerializeField] bool waitForDismiss = false;
    [SerializeField] float timeBeforeDismiss = 1.5f;
    [SerializeField] DismmissType dismmissType;
    

    //chached states
    Animator animator;

    

    private void Start() {
        animator = GetComponent<Animator>();
        Options options = FindObjectOfType<Options>();
        if (isHint && options != null && animator != null && options.showHintBoards) SwoopBoardIn();
        if (!waitForDismiss) StartCoroutine(DelayDismiss());
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && dismmissType == DismmissType.OnMouseClick) DismissBoard();
    }

    private void SwoopBoardIn() {
        GetComponent<Animator>().SetTrigger("Activate");
    }

    private IEnumerator DelayDismiss() {
        yield return new WaitForSeconds(timeBeforeDismiss);
        DismissBoard();
    }

    public void DismissBoard() {
        Dismiss();
    }

    private void Dismiss() {
        if (animator) animator.SetTrigger("Dismiss");
    }

}
