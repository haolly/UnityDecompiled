﻿namespace UnityEngine
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using UnityEngine.Internal;

    /// <summary>
    /// <para>Interface into the native iPhone, Android, Windows Phone and Windows Store Apps on-screen keyboards - it is not available on other platforms.</para>
    /// </summary>
    public sealed class TouchScreenKeyboard
    {
        [NonSerialized]
        internal IntPtr m_Ptr;

        public TouchScreenKeyboard(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure, bool alert, string textPlaceholder)
        {
            TouchScreenKeyboard_InternalConstructorHelperArguments arguments = new TouchScreenKeyboard_InternalConstructorHelperArguments {
                keyboardType = Convert.ToUInt32(keyboardType),
                autocorrection = Convert.ToUInt32(autocorrection),
                multiline = Convert.ToUInt32(multiline),
                secure = Convert.ToUInt32(secure),
                alert = Convert.ToUInt32(alert)
            };
            this.TouchScreenKeyboard_InternalConstructorHelper(ref arguments, text, textPlaceholder);
        }

        [MethodImpl(MethodImplOptions.InternalCall), ThreadAndSerializationSafe]
        private extern void Destroy();
        ~TouchScreenKeyboard()
        {
            this.Destroy();
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void GetSelectionInternal(out int start, out int length);
        [MethodImpl(MethodImplOptions.InternalCall)]
        private static extern void INTERNAL_get_area(out Rect value);
        /// <summary>
        /// <para>Opens the native keyboard provided by OS on the screen.</para>
        /// </summary>
        /// <param name="text">Text to edit.</param>
        /// <param name="keyboardType">Type of keyboard (eg, any text, numbers only, etc).</param>
        /// <param name="autocorrection">Is autocorrection applied?</param>
        /// <param name="multiline">Can more than one line of text be entered?</param>
        /// <param name="secure">Is the text masked (for passwords, etc)?</param>
        /// <param name="alert">Is the keyboard opened in alert mode?</param>
        /// <param name="textPlaceholder">Text to be used if no other text is present.</param>
        [ExcludeFromDocs]
        public static TouchScreenKeyboard Open(string text)
        {
            string textPlaceholder = "";
            bool alert = false;
            bool secure = false;
            bool multiline = false;
            bool autocorrection = true;
            TouchScreenKeyboardType keyboardType = TouchScreenKeyboardType.Default;
            return Open(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder);
        }

        /// <summary>
        /// <para>Opens the native keyboard provided by OS on the screen.</para>
        /// </summary>
        /// <param name="text">Text to edit.</param>
        /// <param name="keyboardType">Type of keyboard (eg, any text, numbers only, etc).</param>
        /// <param name="autocorrection">Is autocorrection applied?</param>
        /// <param name="multiline">Can more than one line of text be entered?</param>
        /// <param name="secure">Is the text masked (for passwords, etc)?</param>
        /// <param name="alert">Is the keyboard opened in alert mode?</param>
        /// <param name="textPlaceholder">Text to be used if no other text is present.</param>
        [ExcludeFromDocs]
        public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType)
        {
            string textPlaceholder = "";
            bool alert = false;
            bool secure = false;
            bool multiline = false;
            bool autocorrection = true;
            return Open(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder);
        }

        /// <summary>
        /// <para>Opens the native keyboard provided by OS on the screen.</para>
        /// </summary>
        /// <param name="text">Text to edit.</param>
        /// <param name="keyboardType">Type of keyboard (eg, any text, numbers only, etc).</param>
        /// <param name="autocorrection">Is autocorrection applied?</param>
        /// <param name="multiline">Can more than one line of text be entered?</param>
        /// <param name="secure">Is the text masked (for passwords, etc)?</param>
        /// <param name="alert">Is the keyboard opened in alert mode?</param>
        /// <param name="textPlaceholder">Text to be used if no other text is present.</param>
        [ExcludeFromDocs]
        public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection)
        {
            string textPlaceholder = "";
            bool alert = false;
            bool secure = false;
            bool multiline = false;
            return Open(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder);
        }

        /// <summary>
        /// <para>Opens the native keyboard provided by OS on the screen.</para>
        /// </summary>
        /// <param name="text">Text to edit.</param>
        /// <param name="keyboardType">Type of keyboard (eg, any text, numbers only, etc).</param>
        /// <param name="autocorrection">Is autocorrection applied?</param>
        /// <param name="multiline">Can more than one line of text be entered?</param>
        /// <param name="secure">Is the text masked (for passwords, etc)?</param>
        /// <param name="alert">Is the keyboard opened in alert mode?</param>
        /// <param name="textPlaceholder">Text to be used if no other text is present.</param>
        [ExcludeFromDocs]
        public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline)
        {
            string textPlaceholder = "";
            bool alert = false;
            bool secure = false;
            return Open(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder);
        }

        /// <summary>
        /// <para>Opens the native keyboard provided by OS on the screen.</para>
        /// </summary>
        /// <param name="text">Text to edit.</param>
        /// <param name="keyboardType">Type of keyboard (eg, any text, numbers only, etc).</param>
        /// <param name="autocorrection">Is autocorrection applied?</param>
        /// <param name="multiline">Can more than one line of text be entered?</param>
        /// <param name="secure">Is the text masked (for passwords, etc)?</param>
        /// <param name="alert">Is the keyboard opened in alert mode?</param>
        /// <param name="textPlaceholder">Text to be used if no other text is present.</param>
        [ExcludeFromDocs]
        public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure)
        {
            string textPlaceholder = "";
            bool alert = false;
            return Open(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder);
        }

        /// <summary>
        /// <para>Opens the native keyboard provided by OS on the screen.</para>
        /// </summary>
        /// <param name="text">Text to edit.</param>
        /// <param name="keyboardType">Type of keyboard (eg, any text, numbers only, etc).</param>
        /// <param name="autocorrection">Is autocorrection applied?</param>
        /// <param name="multiline">Can more than one line of text be entered?</param>
        /// <param name="secure">Is the text masked (for passwords, etc)?</param>
        /// <param name="alert">Is the keyboard opened in alert mode?</param>
        /// <param name="textPlaceholder">Text to be used if no other text is present.</param>
        [ExcludeFromDocs]
        public static TouchScreenKeyboard Open(string text, TouchScreenKeyboardType keyboardType, bool autocorrection, bool multiline, bool secure, bool alert)
        {
            string textPlaceholder = "";
            return Open(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder);
        }

        /// <summary>
        /// <para>Opens the native keyboard provided by OS on the screen.</para>
        /// </summary>
        /// <param name="text">Text to edit.</param>
        /// <param name="keyboardType">Type of keyboard (eg, any text, numbers only, etc).</param>
        /// <param name="autocorrection">Is autocorrection applied?</param>
        /// <param name="multiline">Can more than one line of text be entered?</param>
        /// <param name="secure">Is the text masked (for passwords, etc)?</param>
        /// <param name="alert">Is the keyboard opened in alert mode?</param>
        /// <param name="textPlaceholder">Text to be used if no other text is present.</param>
        public static TouchScreenKeyboard Open(string text, [DefaultValue("TouchScreenKeyboardType.Default")] TouchScreenKeyboardType keyboardType, [DefaultValue("true")] bool autocorrection, [DefaultValue("false")] bool multiline, [DefaultValue("false")] bool secure, [DefaultValue("false")] bool alert, [DefaultValue("\"\"")] string textPlaceholder) => 
            new TouchScreenKeyboard(text, keyboardType, autocorrection, multiline, secure, alert, textPlaceholder);

        [MethodImpl(MethodImplOptions.InternalCall)]
        private extern void TouchScreenKeyboard_InternalConstructorHelper(ref TouchScreenKeyboard_InternalConstructorHelperArguments arguments, string text, string textPlaceholder);

        /// <summary>
        /// <para>Is the keyboard visible or sliding into the position on the screen?</para>
        /// </summary>
        public bool active { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Returns portion of the screen which is covered by the keyboard.</para>
        /// </summary>
        public static Rect area
        {
            get
            {
                Rect rect;
                INTERNAL_get_area(out rect);
                return rect;
            }
        }

        /// <summary>
        /// <para>Specifies whether the TouchScreenKeyboard supports the selection property. (Read Only)</para>
        /// </summary>
        public bool canGetSelection { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        /// <summary>
        /// <para>Specifies if input process was finished. (Read Only)</para>
        /// </summary>
        public bool done { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        /// <summary>
        /// <para>Will text input field above the keyboard be hidden when the keyboard is on screen?</para>
        /// </summary>
        public static bool hideInput { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Is touch screen keyboard supported.</para>
        /// </summary>
        public static bool isSupported
        {
            get
            {
                RuntimePlatform platform = Application.platform;
                switch (platform)
                {
                    case RuntimePlatform.MetroPlayerX86:
                    case RuntimePlatform.MetroPlayerX64:
                    case RuntimePlatform.MetroPlayerARM:
                        return false;

                    case RuntimePlatform.TizenPlayer:
                    case RuntimePlatform.PSM:
                    case RuntimePlatform.IPhonePlayer:
                    case RuntimePlatform.Android:
                        break;

                    default:
                        if ((platform != RuntimePlatform.WiiU) && (platform != RuntimePlatform.tvOS))
                        {
                            return false;
                        }
                        break;
                }
                return true;
            }
        }

        /// <summary>
        /// <para>Returns the character range of the selected text within the string currently being edited. (Read Only)</para>
        /// </summary>
        public RangeInt selection
        {
            get
            {
                RangeInt num;
                this.GetSelectionInternal(out num.start, out num.length);
                return num;
            }
        }

        /// <summary>
        /// <para>Specified on which display the software keyboard will appear.</para>
        /// </summary>
        public int targetDisplay { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Returns the text displayed by the input field of the keyboard.</para>
        /// </summary>
        public string text { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

        /// <summary>
        /// <para>Returns true whenever any keyboard is completely visible on the screen.</para>
        /// </summary>
        public static bool visible { [MethodImpl(MethodImplOptions.InternalCall)] get; }

        /// <summary>
        /// <para>Specifies if input process was canceled. (Read Only)</para>
        /// </summary>
        public bool wasCanceled { [MethodImpl(MethodImplOptions.InternalCall)] get; }
    }
}

