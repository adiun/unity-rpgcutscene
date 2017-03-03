Simple Unity3D cutscene sequencer with RPG-style animated paged dialog support

## Features

* Declarative cutscene creation
* Completion notifications
* Ability to run coroutines within cutscene actions
* Support for instantiating prefabs
* Showing RPG-style paged, docked dialogs with animating text, reading time, and next-page button support


## Setup

1. Create a Unity Canvas to hold the dialog, e.g. `CutsceneCanvas`.
1. Attach the `Cutscene Controller` script component to it.
1. Create an empty GameObject under `CutsceneCanvas` to hold your prefabs: e.g. `CutscenePrefabs`.
1. Attach the `Cutscene Prefabs` script component to it.
1. Create a Panel under `CutsceneCanvas` to represent your dialog. You can name it `DialogPanel`.
1. Create a Text under `DialogPanel` to represent your dialog text, e.g. `DialogText`.
1. Create a Button under `DialogPanel` to represent your next page button, e.g. `NextPageButton`.
1. Wire up all the Inspector fields for `Cutscene Controller`. Make sure to also hook up the Next Button's Click event to the `CutsceneController's` `OnNextPageBtnClick()`.
1. Add any prefabs you will instantiate (with ids) for `Cutscene Prefabs`.


## Basic Usage

Create a cutscene:
```c#
var cutscene = new Cutscene() {
    new DialogAction("Very basic cutscene"),
    new WaitAction(2),
    new ShowDialogAction(false),
};
```

Run the cutscene through the cutscene controller, attached as a component to the cutscene canvas:
```c#
var cutsceneCanvas = GameObject.Find("CutsceneCanvas");
var cutsceneController = cutsceneCanvas.GetComponent<CutsceneController>();
cutsceneController.RunCutscene(cutscene, () => {
    // Cutscene has ended. CutsceneCanvas will automatically deactivate. Open another panel, etc.
});
```


## How it Works

The `CutsceneController` is the main API to interact with. Internally it uses a `CutsceneQueue`. The `CutsceneQueue` is just a runner for the `Cutscene` and also accepts a `CutsceneQueueContext` that should provide completion actions and can also provide coroutine start/stop functionality (provided by `MonoBehaviour`).

The paged dialog support reuses the `CutsceneQueue`. Within a CutsceneAction, dialog is split into N pages depending on how long it is (and how big the `DialogText` game object is) and a queue of N actions is created. The next button fires an event that navigates to the next action in the internal queue.

## Feature Details

### Declarative Cutscene Creation
Check out [`IntroCutscene.cs`](https://github.com/adiun/unity-rpgcutscene/blob/master/IntroCutscene.cs) to see how to set this up. Here is an example:

```c#
return new Cutscene() {
    new ShowDialogAction(false),
    new InstantiatePrefabAction("CutsceneSpaceship", "CutsceneSpaceship", Vector3.zero, Quaternion.identity),
    new WaitAction(2),
    new ShowDialogAction(true),
    new DialogAction("This is a dialog test..."),
    new DialogAction("Another dialog that runs automatically."),
    new ShowDialogAction(false),
    new DestroyObjectAction("CutsceneSpaceship")
};
```


### Completion Notifications
Completion notification of a `CutsceneQueue` can be provided through the `CutsceneQueueContext`. 

There are also completion notifications for individual `CutsceneActions`. 


### Coroutines within CutsceneActions
When implementing your own `CustceneActions` - just remember to call the `finish()` Action parameter after your logic is finished in `Run()`. `finish()` is not automatically called - this is to support coroutines. See `WaitAction` in [`CutsceneActions.cs`](https://github.com/adiun/unity-rpgcutscene/blob/master/CutsceneActions.cs) for an example.


### Prefab Instantiation
`CutscenePrefabs` is a `Monobehaviour` that acts like a ResourceDictionary in XAML - attach your prefabs to it in the Inspector at edit-time (with unique IDs) so that they will be ready to load at runtime in the game. This is to avoid the slow performance of Resources.Load. 

You can use the `InstantiatePrefabAction` to load these into your cutscene:
```
var cutscene = new Cutscene() {
  new InstantiatePrefabAction("CutsceneSpaceship", "CutsceneSpaceship", Vector3.zero, Quaternion.identity)
};
```


### RPG-style paged, animating dialogs
You only need a `Text` within a `Panel` in a `Canvas` (see Basic Usage section above) to set this up. Next page button is optional.

* `ShowDialogAction(bool)` shows and hides the `Panel`. 
* `DialogAction` adds text into the dialog panel and animates it, like old-school RPGs. Use the `dialogTextSpeed` to control how fast it animates. 
* After the text has finished animating, the cutscene will run to the next `CutsceneAction` automatically.
* The next page button allows you to skip to the next action (dialog action or otherwise) in the cutscene. 
* `CutsceneController` will automatically split long text into several dialog pages. You can navigate to the next page with the next button.
* Each `DialogPage` has a reading time that is computed based on the length of the text. See [`CutsceneController.DialogPage.cs`](https://github.com/adiun/unity-rpgcutscene/blob/master/CutsceneController.DialogPage.cs)


## Future features
* Support for concurrent `CutsceneActions`
* UnityPackage support
* Migrating tests into this package


