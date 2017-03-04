using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Matisco.Wpf.Controls.Editors
{
    internal class PopUpWindow : Window
    {
        private bool _closing;

        static PopUpWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PopUpWindow), new FrameworkPropertyMetadata(typeof(PopUpWindow)));
        }

        public PopUpWindow()
        {
            ContentRendered += OnContentRendered;
            Closing += OnClosing;
            Deactivated += PopUpWindow_Deactivated;
        }

        private void OnContentRendered(object sender, EventArgs eventArgs)
        {
            var content = (Content as IHasFocusElement)?.GetFocusElement();
            if (content != null)
            {
                content.Focus();
                Keyboard.Focus(content);
            }
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            _closing = true;
        }

        private void PopUpWindow_Deactivated(object sender, System.EventArgs e)
        {
            if (!_closing)
            {
                Close();
            }
        }
    }
}
