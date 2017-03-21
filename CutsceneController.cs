
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public delegate void NextPageBtnClickHandler(ICutsceneController controller);

public interface ICutsceneController {
    void ShowPagedDialog(bool show);
    void AnimateTextInPagedDialog(string text, Action completion);
    Coroutine StartCoroutine(IEnumerator routine);
    void StopCoroutine(Coroutine routine);
    GameObject InstantiatePrefab(string id, Vector3 position, Quaternion rotation);
    void DestroyObject(string name);
}

public interface IPagedDialogController {
    void AttachToNextBtnClickEvent(NextPageBtnClickHandler eventHandler);
    void DetachFromNextBtnClickEvent(NextPageBtnClickHandler eventHandler);
    void AnimateTextInDialogPage(string pageText, Action completion);
}

public class CutsceneController 
    : MonoBehaviour
    , ICutsceneController
    , IPagedDialogController 
{
    public GameObject cutsceneCanvas;
    public GameObject dialogPanel;
    public Text dialogText;
    public float dialogTextSpeed;
    public GameObject nextPageButtonObj;
    public CutscenePrefabs prefabs;
    private CutsceneQueue _cutsceneQueue;
    private event NextPageBtnClickHandler NextPageBtnClicked;
    private Coroutine _pagedDialogAnimationCoroutine;
    private CutsceneQueue _pagedDialogCutsceneQueue;

    public void RunCutscene(Cutscene cutscene, Action completion) {
        cutsceneCanvas.SetActive(true);
        var context = new CutsceneQueueContext(this, () => {
            cutsceneCanvas.SetActive(false);
            completion();
        });
        _cutsceneQueue = new CutsceneQueue(cutscene, context);
        _cutsceneQueue.Run();
    }    
    public void ShowPagedDialog(bool show) {
        dialogPanel.SetActive(show);
    }

    public void AnimateTextInPagedDialog(string text, Action completion) {
        IEnumerable<DialogPage> dialogPages = GetDialogPages(text);
        var cutscene = new Cutscene();
        foreach (var page in dialogPages) {
            cutscene.Add(new DialogPageAction(page, this));
        }

        var context = new CutsceneQueueContext(this, completion);
        _pagedDialogCutsceneQueue = new CutsceneQueue(cutscene, context);
        _pagedDialogCutsceneQueue.Run();
    }

    public GameObject InstantiatePrefab(string id, Vector3 position, Quaternion rotation) {
        var prefabObj = prefabs.GetPrefab(id);
        if (prefabObj == null)
            return null;
        return Instantiate(prefabObj, position, rotation);
    }

    public void DestroyObject(string name) {
        var obj = this.transform.Find(name);
        if (obj == null)
            return;
        Destroy(obj);
    }

    private IEnumerable<DialogPage> GetDialogPages(string text) {
        var textGenerator = dialogText.cachedTextGenerator;
        var currentText = text;
        var visibleText = text;
        List<DialogPage> dialogPages = new List<DialogPage>();
        var generationSettings = dialogText.GetGenerationSettings(dialogText.rectTransform.rect.size);
        do {
            textGenerator.Populate(currentText, generationSettings);
            var visibleCharCount = textGenerator.characterCountVisible;
            visibleText = currentText.Substring(0, visibleCharCount);
            dialogPages.Add(new DialogPage(visibleText));
            currentText = currentText.Substring(visibleText.Length);
        } while (currentText.Length > 0);
        return dialogPages;
    }

    public void OnNextPageBtnClick() {
        NextPageBtnClicked(this);
    }

    public void AttachToNextBtnClickEvent(NextPageBtnClickHandler eventHandler) {
        NextPageBtnClicked += eventHandler;
    }

    public void DetachFromNextBtnClickEvent(NextPageBtnClickHandler eventHandler) {
        NextPageBtnClicked -= eventHandler;
    }

    public void AnimateTextInDialogPage(string pageText, Action completion) {
        if (_pagedDialogAnimationCoroutine != null) {
            StopCoroutine(_pagedDialogAnimationCoroutine);
        }
        _pagedDialogAnimationCoroutine = StartCoroutine(AnimateTextInDialogPageCoroutine(pageText, completion));
    }

    private IEnumerator AnimateTextInDialogPageCoroutine(string pageText, Action completion) {
        for (var i = 0; i <= pageText.Length; i++) {
            dialogText.text = pageText.Substring(0, i);
            yield return new WaitForSeconds(dialogTextSpeed);
        }
        completion();
    }
}