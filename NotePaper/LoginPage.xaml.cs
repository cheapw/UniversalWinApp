using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace NotePaper
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        Compositor _compositor = Window.Current.Compositor;
        SpringVector3NaturalMotionAnimation _springAnimation;


        private void ExitAppButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Exit();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterPage), string.IsNullOrWhiteSpace(this.Account.Text) ? null : this.Account.Text, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void ResetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ResetPasswordPage), string.IsNullOrWhiteSpace(this.Account.Text)?null: this.Account.Text, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            On_ForwardRequested();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //this.BackButton.IsEnabled = this.Frame.CanGoBack;
            RemoveBackStackPages();
            RemoveForwardStackPages();
            this.BackButton.Visibility = this.Frame.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
            this.ForwardButton.Visibility = this.Frame.CanGoForward ? Visibility.Visible : Visibility.Collapsed;

            if (e.NavigationMode == NavigationMode.New)
            {
                this.Account.Text = (e.Parameter as string) ?? string.Empty;
            }

            base.OnNavigatedTo(e);
        }

        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        private bool On_ForwardRequested()
        {
            if (this.Frame.CanGoForward)
            {
                this.Frame.GoForward();
                return true;
            }
            return false;
        }
        private void RemoveBackStackPages()
        {
            int count = this.Frame.BackStackDepth;
            while (count > 3)
            {
                this.Frame.BackStack.Remove(this.Frame.BackStack.First());
                count = this.Frame.BackStackDepth;
            }
        }
        private void RemoveForwardStackPages()
        {
            int count = this.Frame.ForwardStack.Count;
            while (count > 3)
            {
                this.Frame.ForwardStack.Remove(this.Frame.ForwardStack.First());
                count = this.Frame.ForwardStack.Count;
            }
        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.MoreSettingPanel.Visibility == Visibility.Visible)
            {
                this.MoreSettingPanel.Visibility = Visibility.Collapsed;
                //this.MoreSettingPanel.Background = new SolidColorBrush()
            }
            else
            {
                this.MoreSettingPanel.Visibility = Visibility.Visible;
            }

            CreateOrUpdateSpringAnimation(1.0f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        private void MoreButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            // Scale up to 1.5
            CreateOrUpdateSpringAnimation(0.9f);

            (sender as UIElement).StartAnimation(_springAnimation);

        }

        private void MoreButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            // Scale back down to 1.0
            CreateOrUpdateSpringAnimation(1.0f);

            (sender as UIElement).StartAnimation(_springAnimation);

        }

        private void CreateOrUpdateSpringAnimation(float finalValue)
        {
            if (_springAnimation == null)
            {
                _springAnimation = _compositor.CreateSpringVector3Animation();
                _springAnimation.Target = "Scale";
            }

            _springAnimation.FinalValue = new Vector3(finalValue);
            _springAnimation.DampingRatio = 0.4f;
            _springAnimation.Period = TimeSpan.FromMilliseconds(50d);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.0f);
        }

        private void BackButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(0.9f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        private void BackButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.0f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        private void ForwardButton_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(0.9f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }

        private void ForwardButton_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            CreateOrUpdateSpringAnimation(1.0f);

            (sender as UIElement).StartAnimation(_springAnimation);
        }
    }
}
