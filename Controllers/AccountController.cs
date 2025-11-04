namespace PasswordSolution.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return View();

        var validation = await _userManager.PasswordValidators[0].ValidateAsync(_userManager, null!, password);
        if (!validation.Succeeded)
        {
            foreach (var error in validation.Errors)
                ModelState.AddModelError("", error.Description);
            return View();
        }

        var (saltSha, saltArgon) = SecureHasher.GeneratePasswordSalts();

        var passwordHash = SecureHasher.HashPassword(password, saltSha, saltArgon);

        var user = new ApplicationUser
        {
            UserName = username,
            PasswordSHA512Salt = saltSha,
            PasswordARGON2IDSalt = saltArgon,
            PasswordHash = passwordHash
        };

        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: true);
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View();
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            ModelState.AddModelError("", "Benutzer nicht gefunden.");
            return View();
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
            if (lockoutEnd.HasValue)
            {
                var remaining = lockoutEnd.Value - DateTimeOffset.UtcNow;
                var minutes = Math.Max(0, Math.Round(remaining.TotalMinutes));

                if (minutes >= 1)
                    ModelState.AddModelError("", $"Konto gesperrt. Bitte in etwa {minutes} Minute{(minutes != 1 ? "n" : "")} erneut versuchen.");
                else
                    ModelState.AddModelError("", "Konto gesperrt. Bitte in Kürze erneut versuchen.");
            }
            else
            {
                ModelState.AddModelError("", "Konto derzeit gesperrt. Bitte später erneut versuchen.");
            }

            return View();
        }

        bool valid = SecureHasher.VerifyPassword(
            password,
            user.PasswordSHA512Salt,
            user.PasswordARGON2IDSalt,
            user.PasswordHash
        );

        if (!valid)
        {
            await _userManager.AccessFailedAsync(user);

            if (await _userManager.IsLockedOutAsync(user))
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                string msg = lockoutEnd.HasValue ? $"Konto gesperrt bis {lockoutEnd.Value.LocalDateTime:t} Uhr." : "Konto gesperrt nach zu vielen Fehlversuchen.";
                ModelState.AddModelError("", msg);
                return View();
            }

            int attemptsLeft = _userManager.Options.Lockout.MaxFailedAccessAttempts - user.AccessFailedCount;
            ModelState.AddModelError("", $"Ungültige Anmeldeinformationen. Noch {attemptsLeft} Versuch{(attemptsLeft != 1 ? "e" : "")} bis zur Sperre.");
            return View();
        }

        await _userManager.ResetAccessFailedCountAsync(user);
        await _signInManager.SignInAsync(user, isPersistent: true);
        return RedirectToAction("Index", "Home");
    }
    
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login");

        var model = new
        {
            Username = user.UserName,
            PasswordHash = user.PasswordHash,
            SaltSHA512 = Convert.ToBase64String(user.PasswordSHA512Salt),
            SaltArgon2id = Convert.ToBase64String(user.PasswordARGON2IDSalt)
        };

        return View(model);
    }
}
