using System;
using System.Collections;
using UnityEngine;

class DialogPageAction : ICutsceneAction {
    private DialogPage _dialogPage;
    private IPagedDialogController _pagedDialogController;
    private Action _completion;
    private Coroutine _waitToReadCoroutine;
    private bool _isCompleted;

    public DialogPageAction(DialogPage dialogPage, IPagedDialogController pagedDialogController) {
        _dialogPage = dialogPage;
        _pagedDialogController = pagedDialogController;
        _pagedDialogController.AttachToNextBtnClickEvent(OnNextPageBtnClick);
    }

    public void Run(ICutsceneController controller, Action completion) {
        var startTime = Time.time;
        _completion = completion;
        _pagedDialogController.AnimateTextInDialogPage(_dialogPage.Text, () => {
            var timeToWait = Mathf.Max(0, _dialogPage.ReadingTime - (Time.time - startTime));
            _waitToReadCoroutine = controller.StartCoroutine(Wait(timeToWait, completion));
        });
    }

    private IEnumerator Wait(float seconds, Action completion) {
        yield return new WaitForSeconds(seconds);
        Complete();
    }

    private void OnNextPageBtnClick(ICutsceneController controller) {
        if (_waitToReadCoroutine != null)
            controller.StopCoroutine(_waitToReadCoroutine);
        Complete();
    }

    private void Complete() {
        if (_isCompleted)
            return;
        _pagedDialogController.DetachFromNextBtnClickEvent(OnNextPageBtnClick);
        _completion();
        _isCompleted = true;
    }
}