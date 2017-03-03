
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Cutscenes
{
    public static Cutscene IntroCutscene {
        get {
            return new Cutscene() {
                new ShowDialogAction(false),
                new InstantiatePrefabAction("CutsceneSpaceship", "CutsceneSpaceship", Vector3.zero, Quaternion.identity),
                new WaitAction(2),
                new ShowDialogAction(true),
                new DialogAction("This is a dialog test..."),
                new DialogAction("Another dialog that runs automatically."),
                new DialogAction("Dialog with very long text that should be split into multiple pages. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Etiam sed sem nisl. Duis vitae erat eget tellus rhoncus tristique. Nam lobortis luctus lacus, fermentum cursus arcu auctor sit amet. Vivamus semper ullamcorper urna ut viverra. Aenean tristique, tellus sit amet condimentum blandit, lacus purus facilisis elit, id fermentum enim nulla scelerisque lacus. Praesent blandit ut dui id maximus. Maecenas et neque in arcu auctor varius a vitae tellus. Aenean pretium nibh eu viverra auctor. In dapibus nisl sed nisl mattis, non posuere libero pretium. Cras dui lacus, rhoncus rhoncus hendrerit et, aliquet eu risus. Donec gravida viverra dictum. Cras nec."),
                new ShowDialogAction(false),
                new DestroyObjectAction("CutsceneSpaceship")
            };
        }
    }
}
