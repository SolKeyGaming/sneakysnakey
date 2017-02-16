// Grid - a utility used in the whole project

// it's this simple .. this is a handy class which allows you to call other "stuff" easily
// you can now simply call anything you wish like this:

//                        Grid.purchaseManager.whatever()

// it's that simple.  it is so simple you should be saying "this can't be right?!" Hah hah! But it is right!

// typically .. the "things" that Grid handles for you are all
// PERSISTENT THROUGHOUT THE GAME and hence part of our __preScene scene

// For the stuff you have to call, it is almost as if they are static classes - BUT - 
// they are normal game engine -style monobehaviors in the scene attached to a real
// normal game object you can see and use in the normal ways with no confusion.

// Philosophy:
// (1) singletons are useless in game engines, so that's out
// (2) generally in the real world in game engines, statics are out since you want stuff
//        to actually be "attached to a game object" so that you can enjoy critically three things,
//        (A) coroutines and (B) dragging-in-the-editor-variables and finally
//        (C) easy consistent serialization, persistence over scenes, blah blah

// Note that this is also incredibly useful if not essential for crap like objects that connect to
// the internet, perform "background processes" in Unity so to speak, and so on and on.

// note for clarity, the word "Grid" is meaningless.  it could be any term like "Switchboard"
// "HandyThing"  "HyperConnector" or some general exciting acronym from your company
// or whatever you want. "Grid" is short. means like "power grid".

//                HOW TO USE

// (1) Look at the constructor below. Do not hesitate to add in more "stuff" you want to access easily.

// (2) If you have something that is a singleton. firstly, completely eliminate the singleton aspect.
// then, using Hypnonsis, make it so that you never knew about singletons. and then you
// won't have any singletons. problem solved.  make it a totally normal thing on a game object and use this.

// (3) if you have something that is a static.  It's very hard to put in to words when things "are ok to be"
// a static.  (Something like: "If it's a mathematical-like calculation" or "almost never changes in any way
// and has nothing whatsoever to do with game data")  But if it's causing the slightest confusion as a static,
// is hard to serialise, it changes, it needs coroutines, or anything else - just convert it as in (2).

// (4) Notice you will now have a peaceful easy feeling and in particular that "It can't be this easy?" feeling.
// Life is ridiculously simple with game engines! enjoy the paradigm!

/*
using UnityEngine;

static class Grid
{
    public static ScreenManager blahSystem;
    public static Procs procs;

    public static Cloud cloud;

    public static SuperAudio superAudio;
    public static PurchaseManager purchaseManager;

    // when the program launches, Grid will check that all the needed elements are in place
    // that's exactly what you do in the static constructor here:
    static Grid()
    {
        GameObject g;


        g = safeFind("__Blah"); // (some persistent gema object)
        blahSystem = (BlahSystem)safeComponent(g, "BlahSystem");
        procs = (Procs)safeComponent(g, "Procs");

        g = safeFind("__AudioRelated");
        superAudio = (SuperAudio)safeComponent(g, "SuperAudio");

        // etc .. 
        // (you could use just the one persistent game object, or many - irrelevant)


        // PS. annoying arcane technical note - remember that really, in c# static constructors do not run
        // until the first time you use them.  almost certainly in any large project like this, Grid
        // would be called zillions of times by all the Awake (etc etc) code everywhere, so it is
        // a non-issue. but if you're just testing or something, it may be confusing that (for example)
        // the wake-up alert only appears just before you happen to use Grid, rather than "when you hit play"
    }

    // this has no purpose other than for developers wondering HTF you use Grid
    // just type Grid.SayHello() anywhere in the project.
    // it is useful to add a similar routine to (example) PurchaseManager.cs
    // then from anywhere in the project, you can type Grid.purchaseManager.SayHello()
    // to check everything is hooked-up properly.
    public static void SayHello()
    {
        Debug.Log("Confirming to developer that the Grid is working fine.");
    }

    // just some convenience routines to save people copy pasting 

    // when Grid wakes up, it checks everything is in place...
    private static GameObject safeFind(string s)
    {
        GameObject g = GameObject.Find(s);
        if (g == null) bigProblem("The " + s + " game object is not in this scene. You're stuffed.");
        // next .... see Vexe to check that there is strictly ONE of these fuckers. you never know.
        return g;
    }
    private static Component safeComponent(GameObject g, string s)
    {
        Component c = g.GetComponent(s);
        if (c == null) bigProblem("The " + s + " component is not there. You're stuffed.");
        return c;
    }
    private static void bigProblem(string error)
    {
        for (int i = 10; i > 0; --i) Debug.LogError(" >>> Cannot proceed... " + error);
        for (int i = 10; i > 0; --i) Debug.LogError(" !!!!!  Is it possible you just forgot to launch from scene zero.");
        Debug.Break();
    }
}
//////////////////////////////////////////////////////////////////////////////
*/