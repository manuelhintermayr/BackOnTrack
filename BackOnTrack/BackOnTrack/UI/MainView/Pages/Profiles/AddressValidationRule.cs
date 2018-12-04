using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using BackOnTrack.Services.UserConfiguration;

namespace BackOnTrack.UI.MainView.Pages.Profiles
{
    public class AddressValidationRule : ValidationRule
    {
        private RunningApplication _runningApplication;

        public AddressValidationRule()
        {
            _runningApplication = RunningApplication.Instance();
        }

        public override System.Windows.Controls.ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string newUrl = (string)value;
            if (!IsCorrectAddress(newUrl))
            {
                return new System.Windows.Controls.ValidationResult(false, "Invalid address");
            }

            string oldUrl = SpecificProfileView.CurrentSelectedUrlName;
            var urlList = GetListOfAllAlreadyUsedAddresses();
            urlList.Remove(oldUrl);

            if (urlList.Contains(newUrl))
            {
                return new System.Windows.Controls.ValidationResult(false, "Address already used");
            }

            return System.Windows.Controls.ValidationResult.ValidResult;
        }

        public static bool IsCorrectAddress(string addressToCheck)
        {
            string correctAddressPattern =
                @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$";
            Match correctAddress = Regex.Match(addressToCheck, correctAddressPattern, RegexOptions.IgnoreCase);

            return correctAddress.Success;
        }

        public static bool IsAddressAlreadyUsed(string addressToCheck)
        {
            return GetListOfAllAlreadyUsedAddresses().Contains(addressToCheck);
        }

        public static List<string> GetListOfAllAlreadyUsedAddresses()
        {
            return (from profile in RunningApplication.Instance().UI.MainView.UserConfiguration.ProfileList from entry in profile.EntryList select entry.Url).ToList();
        }
    }
}
