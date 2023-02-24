namespace KeysReporting.WebAssembly.App.Shared.TermCodes
{
    public class TermCodeDto
    {
        public long Id { get; set; }

        public long FkTermCodeCategory { get; set; }

        public string? TermCode1 { get; set; }

        public string? Alias { get; set; }
        public TermCodeCategoryDto Category { get; set; }
    }

}
