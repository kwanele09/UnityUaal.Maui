namespace UnityUaal.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            UnityUaalMaui.Unity.UnityBridge.ShowUnityWindow();
        }
    }
}
