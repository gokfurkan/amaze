﻿using System;
using UnityEngine;

namespace Template.Scripts
{
    public static class BusSystem
    {
        //Economy
        public static Action <int> OnAddMoneys;
        public static void CallAddMoneys(int amount) { OnAddMoneys?.Invoke(amount); }
        
        public static Action OnResetMoneys;
        public static void CallResetMoneys() { OnResetMoneys?.Invoke(); }
        
        public static Action OnSetMoneys;
        public static void CallSetMoneys() { OnSetMoneys?.Invoke(); }
        
        public static Action OnSpawnMoneys;
        public static void CallSpawnMoneys() { OnSpawnMoneys?.Invoke(); }
        
        //Game Manager
        public static Action OnLevelStart;
        public static void CallLevelStart() { OnLevelStart?.Invoke(); }
     
        public static Action <bool> OnLevelEnd;
        public static void CallLevelEnd(bool win) { OnLevelEnd?.Invoke(win); }
     
        public static Action OnLevelLoad;
        public static void CallLevelLoad() { OnLevelLoad?.Invoke(); }
        
        //Input
        public static Action OnMouseClickDown;
        public static void CallMouseClickDown() { OnMouseClickDown?.Invoke(); }
        
        public static Action OnMouseClicking;
        public static void CallMouseClicking() { OnMouseClicking?.Invoke(); }
        
        public static Action OnMouseClickUp;
        public static void CallMouseClickUp() { OnMouseClickUp?.Invoke(); }
        
        public static Action <SwipeDirection> OnDetectSwipe;
        public static void CallDetectSwipe(SwipeDirection direction) { OnDetectSwipe?.Invoke(direction); }
        
        //Shop
        public static Action OnChangeShopPanelPage;
        public static void CallChangeShopPanelPage() { OnChangeShopPanelPage?.Invoke(); }

        public static Action OnSetPlayerSkin;
        public static void CallSetPlayerSkin() { OnSetPlayerSkin?.Invoke(); }
        
        //Player
        
        public static Action <Vector3> OnSetPlayerStartPos;
        public static void CallSetPlayerStartPos(Vector3 startPos) { OnSetPlayerStartPos?.Invoke(startPos); }
        
        //Grid
        
        public static Action <GameObject> OnActivateGrid;
        public static void CallActivateGrid(GameObject grid) { OnActivateGrid?.Invoke(grid); }
    }
}