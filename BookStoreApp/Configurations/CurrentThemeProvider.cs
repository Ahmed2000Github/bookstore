using MudBlazor;

namespace BookStoreApp.Services
{
    public class CurrentThemeProvider
    {
        private MudTheme _theme { get; set; }
        public MudTheme Theme
        {
            get
            {
                return _theme;
            }
        }
        private Palette _palette { get; set; }
        public Palette Palette { get { return _palette; } }
        private CurrentThemeProvider() { }

        public static CurrentThemeProvider Instance { get; } = new CurrentThemeProvider();
        public void Initialize(MudTheme Theme, bool isDark)
        {
            this._theme = Theme;
            if (isDark)
            {
                _palette = Theme.PaletteDark;
            }
            else
            {
                _palette = Theme.Palette;
            }
            //OnThemeChanged();
        }
        public void UpdateMode( bool isDark)
        {
            if (isDark)
            {
                _palette = _theme.PaletteDark;
            }
            else
            {
                _palette = _theme.Palette;
            }
            OnThemeChanged();
        }
        public event EventHandler ThemeChanged;
        protected virtual void OnThemeChanged()
        {
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
