using KeysReporting.WebAssembly.App.Client.Shared;
using KeysReporting.WebAssembly.App.Client.Static;
using KeysReporting.WebAssembly.App.Shared.Quote;
using Microsoft.AspNetCore.Components;

namespace KeysReporting.WebAssembly.App.Client.Pages.Quote
{
    public partial class Calculator
    {

        public CalculatorDto _calculatorDto = new();

        public string? _buttonClass;
        public string? _errorMessage;
        public string? _loadingMessage;

        public int? _removeID;
        public int _section = 0;

        private async Task HandleYearChange(ChangeEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(e.Value.ToString()))
                {
                    _calculatorDto.Year = null;
                }
                else
                {
                    _calculatorDto.Year = int.Parse(e.Value.ToString());
                }
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

        }

        private async Task HandleAddCampaign()
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _errorMessage = string.Empty;

            try
            {
                var id = 1;

                id = _calculatorDto.CampaignLines.Any()
                    ? _calculatorDto.CampaignLines.OrderByDescending(x => x.ID).FirstOrDefault().ID.Value + 1
                    : id;

                _calculatorDto.CampaignLines.Add(new CampaignLine { ID = id });
            }
            catch
            {
                _errorMessage = Messages.SomethingWentWrong;
            }

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

        private async Task RemoveCampaign(int? id)
        {
            _buttonClass = CSSClasses.ButtonSpin;
            _errorMessage = string.Empty;

            if (id.HasValue)
            {
                var removeLine = _calculatorDto.CampaignLines.Where(x => x.ID == id).FirstOrDefault();

                if (removeLine != null)
                    _calculatorDto.CampaignLines.Remove(removeLine);

                _removeID = null;

                if (!_calculatorDto.CampaignLines.Any())
                    _calculatorDto.CampaignLines.Add(new CampaignLine { ID = 1 });
            }
            else
                _errorMessage = Messages.SomethingWentWrong;

            _loadingMessage = string.Empty;
            _buttonClass = string.Empty;
        }

        private void ToggleRemove(int? id)
        {
            _removeID = id;
        }

        private void ToggleSection(int? id)
        {
            if (id.HasValue)
                _section = id.Value;

            if(_section == 1 && !_calculatorDto.CampaignLines.Any())
                _calculatorDto.CampaignLines.Add(new CampaignLine { ID = 1 });
        }
    }
}
