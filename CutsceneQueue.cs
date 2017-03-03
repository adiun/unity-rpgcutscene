using System;
using System.Collections;
using System.Collections.Generic;

public class CutsceneQueue {
    private Queue<ICutsceneAction> _queue;
    private CutsceneQueueContext _context;

    public CutsceneQueue(Cutscene cutscene, CutsceneQueueContext context) {
        _queue = new Queue<ICutsceneAction>();
        foreach (var action in cutscene) {
            _queue.Enqueue(action);
        }
        _context = context;
    }

    public void Run() {
        Next();
    }

    private void Next() {
        if (_queue.Count == 0) {
            _context.OnQueueCompletion();
            return;
        }
        var action = _queue.Dequeue();
        action.Run(_context.CutsceneController, () => {
            Next();
        });
    }
}
