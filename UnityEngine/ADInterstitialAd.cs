﻿namespace UnityEngine
{
    using System;
    using System.Runtime.CompilerServices;

    [Obsolete("ADInterstitialAd class is obsolete, Apple iAD service discontinued", true)]
    public sealed class ADInterstitialAd
    {
        public static  event InterstitialWasLoadedDelegate onInterstitialWasLoaded
        {
            add
            {
            }
            remove
            {
            }
        }

        public ADInterstitialAd()
        {
        }

        public ADInterstitialAd(bool autoReload)
        {
        }

        ~ADInterstitialAd()
        {
        }

        public void ReloadAd()
        {
        }

        public void Show()
        {
        }

        public static bool isAvailable
        {
            get
            {
                return false;
            }
        }

        public bool loaded
        {
            get
            {
                return false;
            }
        }

        public delegate void InterstitialWasLoadedDelegate();
    }
}

