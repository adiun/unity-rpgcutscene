
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogAction : ICutsceneAction {
    private string _text;
    public string Text { get { return _text; }}
    public DialogAction(string text) {
        _text = text;
    }

    public void Run(ICutsceneController controller, Action finish) {
        controller.AnimateTextInPagedDialog(_text, finish);
    }
}

public class ShowDialogAction: ICutsceneAction {
    private bool _show;

    public ShowDialogAction(bool show) {
        _show = show;
    }

    public void Run(ICutsceneController controller, Action finish) {
        controller.ShowPagedDialog(_show);
        finish();
    }
}

public class InstantiatePrefabAction : ICutsceneAction {

    private string _prefabId;
    private string _name;
    private Vector3 _position;
    private Quaternion _rotation;

    public InstantiatePrefabAction(string prefabId, string name, Vector3 position, Quaternion rotation) {
        _prefabId = prefabId;
        _name = name;
        _position = position;
        _rotation = rotation;
    }

    public void Run(ICutsceneController controller, Action finish) {
        var obj = controller.InstantiatePrefab(_prefabId, _position, _rotation);
        obj.name = _name;
        finish();
    }
}

public class DestroyObjectAction : ICutsceneAction {

    private string _name;

    public DestroyObjectAction(string name) {
        _name = name;
    }

    public void Run(ICutsceneController controller, Action finish) {
        controller.DestroyObject(_name);
        finish();
    }
}

public class WaitAction : ICutsceneAction {
    private float _secondsToWait;
    public float SecondsToWait { get { return _secondsToWait; }}
    public WaitAction(float secondsToWait) {
        _secondsToWait = secondsToWait;
    }

    public void Run(ICutsceneController controller, Action finish) {
        controller.StartCoroutine(WaitCoroutine(finish)); 
    }

    private IEnumerator WaitCoroutine(Action finish) {
        yield return new WaitForSeconds(_secondsToWait);
        finish();
    }
}
