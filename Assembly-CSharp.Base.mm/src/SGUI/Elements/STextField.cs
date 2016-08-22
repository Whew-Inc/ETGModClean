﻿using System;
using UnityEngine;

namespace SGUI {
    public class STextField : SElement {

        public string Text;
        public string TextOnSubmit = "";

        public Action<STextField, string> OnTextUpdate;
        public Action<STextField, Event> OnKey;
        public Action<STextField, string> OnSubmit;

        public override bool IsInteractive {
            get {
                return true;
            }
        }

        public STextField()
            : this("") { }
        public STextField(string text) {
            Text = text;
            Background = Color.white;
            Foreground = Color.black;
        }

        public override void UpdateStyle() {
            if (UpdateBounds) {
                Size.y = Backend.LineHeight;
            }

            base.UpdateStyle();
        }

        public override void Render() {
            Event e = Event.current; // Store the event; check if text field focused after drawing (after events used).
            bool submit = e.type == EventType.KeyDown && e.keyCode == KeyCode.Return;

            string prevText = Text;
            Draw.TextField(this, Vector2.zero, ref Text);
            if (IsFocused = Backend.IsFocused(this)) {
                if (submit) {
                    Text = TextOnSubmit ?? prevText;
                }

                if (prevText != Text) OnTextUpdate?.Invoke(this, prevText);
                if (e.type == EventType.KeyDown || e.type == EventType.KeyUp) OnKey?.Invoke(this, e);
                if (submit) OnSubmit?.Invoke(this, prevText);
            }

            // Focusing should happen when the element has got a valid ID (after rendering the element) and after checking for it.
            if (_ScheduleFocus) {
                _ScheduleFocus = false;
                Backend.Focus(this);
            }
        }

        protected bool _ScheduleFocus;
        public override void Focus() {
            _ScheduleFocus = true;
        }

    }
}