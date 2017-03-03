using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Matisco.Wpf.Controls.Buttons
{
    [TemplatePart(Name = "PART_Border", Type = typeof(Border))]
    public class Button : Control
    {
        private const string PartBorder = "PART_Border";

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(Button), new PropertyMetadata(default(ICommand)));

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(Button), new PropertyMetadata(default(string)));

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
            "Image", typeof(ButtonImageEnum), typeof(Button), new PropertyMetadata(ButtonImageEnum.None));

        public ButtonImageEnum Image
        {
            get { return (ButtonImageEnum) GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
        static Button()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Button), new FrameworkPropertyMetadata(typeof(Button)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var border = GetTemplateChild(PartBorder) as Border;
            border.MouseLeftButtonUp += BorderOnMouseLeftButtonUp;
        }

        private void BorderOnMouseLeftButtonUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (ReferenceEquals(Command, null))
                return;

            if (Command.CanExecute(null))
            {
                Command.Execute(null);
            }
        }
    }
}
