using UIKit;
using Foundation;
using ReactiveUI;
using System.Reactive.Disposables;

namespace SSBakeryAdmin.iOS.Modules.SignIn
{
    [Register("SignInController")]
    public class SignInController : ReactiveViewController<ISignInViewModel>
    {
        private UIButton _triggerGoogleAuthFlowButton;

        public SignInController()
        {
            this.WhenActivated(
                disposables =>
                {
                    this
                        .BindCommand(ViewModel, vm => vm.TriggerGoogleAuthFlow, v => v._triggerGoogleAuthFlowButton)
                        .DisposeWith(disposables);
                });
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            _triggerGoogleAuthFlowButton = new UIButton(UIButtonType.RoundedRect);
            _triggerGoogleAuthFlowButton.SetTitle("Sign in with Google", UIControlState.Normal);
            _triggerGoogleAuthFlowButton.Font = _triggerGoogleAuthFlowButton.Font.WithSize(36);

            View.AddSubview(_triggerGoogleAuthFlowButton);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            _triggerGoogleAuthFlowButton.TranslatesAutoresizingMaskIntoConstraints = false;
            _triggerGoogleAuthFlowButton.WidthAnchor.ConstraintEqualTo(300).Active = true;
            _triggerGoogleAuthFlowButton.HeightAnchor.ConstraintEqualTo(60).Active = true;
            _triggerGoogleAuthFlowButton.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
            _triggerGoogleAuthFlowButton.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor).Active = true;
        }
    }
}