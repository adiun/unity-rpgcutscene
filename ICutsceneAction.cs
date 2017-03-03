
using System;
using System.Collections;

public interface ICutsceneAction {
    void Run(ICutsceneController controller, Action completion);
}
