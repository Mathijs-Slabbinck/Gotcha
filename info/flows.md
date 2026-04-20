 Sign-up flow:
  1. After CreateAsync succeeds, generate a confirmation token via UserManager.GenerateEmailConfirmationTokenAsync()
  2. Build a confirmation URL pointing to a new ConfirmEmail action on AccountsController
  3. Send the email via IEmailService
  4. Don't auto sign-in — redirect to a "check your email" page instead
  5. ConfirmEmail action validates the token via UserManager.ConfirmEmailAsync()

  Guardian consent (on top of that):
  1. If the user is under 16 and has a guardian email, also send a consent email to the guardian
  2. Add a ConfirmGuardianConsent action on AccountsController that sets HasGuardianConsent = true + GuardianConsentDate