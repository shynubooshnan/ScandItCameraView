using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ScandItCameraView.CustomRenderer
{
    public class ScandItCamera : View
    {
        public Action StartScanning;
        public Action StopScanning;
        public Action EditClicked;

        public bool AllowDuplicate { get; set; }
        public bool ShowFocus { get; set; }

        public static BindableProperty DidScannedCommandProperty = BindableProperty.Create("DidScannedCommand",
          typeof(Command<List<string>>),
          typeof(ScandItCamera));

        public Command<List<string>> DidScannedCommand
        {
            get { return (Command<List<string>>)GetValue(DidScannedCommandProperty); }
            set { base.SetValue(DidScannedCommandProperty, value); }
        }
    }
}
