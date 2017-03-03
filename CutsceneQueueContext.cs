using System;

public class CutsceneQueueContext {
    private readonly ICutsceneController _cutsceneController;
    public ICutsceneController CutsceneController { get { return _cutsceneController; }}
    private readonly Action _onQueueCompletion;
    public Action OnQueueCompletion { get { return _onQueueCompletion; }}
    public CutsceneQueueContext(ICutsceneController controller, Action onQueueCompletion) {
        _cutsceneController = controller;
        _onQueueCompletion = onQueueCompletion;
    }
}