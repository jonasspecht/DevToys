#nullable enable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Composition;
using DevToys.Api.Core;
using DevToys.Api.Core.Settings;
using DevToys.Api.Tools;
using DevToys.Helpers.RandomString;
using DevToys.UI.Controls;
using DevToys.Views.Tools.PasswordGenerator;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace DevToys.ViewModels.Tools.PasswordGenerator
{
    [Export(typeof(PasswordGeneratorToolViewModel))]
    public sealed class PasswordGeneratorToolViewModel : ObservableRecipient, IToolViewModel
    {
        /// <summary>
        /// The length of the generated passwords.
        /// </summary>
        private static readonly SettingDefinition<int> LengthSetting
            = new(
                name: $"{nameof(PasswordGeneratorToolViewModel)}.{nameof(LengthSetting)}",
                isRoaming: true,
                defaultValue: 16);

        /// <summary>
        /// The number of passwords generated.
        /// </summary>
        private static readonly SettingDefinition<int> NumberOfPasswordsToGenerateSetting
            = new(
                name: $"{nameof(PasswordGeneratorToolViewModel)}.{nameof(NumberOfPasswordsToGenerateSetting)}",
                isRoaming: true,
                defaultValue: 1);

        /// <summary>
        /// Whether the generated passwords should include lower case characters.
        /// </summary>
        private static readonly SettingDefinition<bool> IncludeLowerCaseSetting
            = new(
                name: $"{nameof(PasswordGeneratorToolViewModel)}.{nameof(IncludeLowerCaseSetting)}",
                isRoaming: true,
                defaultValue: true);

        /// <summary>
        /// Whether the generated passwords should include upper case characters.
        /// </summary>
        private static readonly SettingDefinition<bool> IncludeUpperCaseSetting
            = new(
                name: $"{nameof(PasswordGeneratorToolViewModel)}.{nameof(IncludeUpperCaseSetting)}",
                isRoaming: true,
                defaultValue: true);

        /// <summary>
        /// Whether the generated passwords should include numbers.
        /// </summary>
        private static readonly SettingDefinition<bool> IncludeNumbersSetting
            = new(
                name: $"{nameof(PasswordGeneratorToolViewModel)}.{nameof(IncludeNumbersSetting)}",
                isRoaming: true,
                defaultValue: true);

        /// <summary>
        /// Whether the generated passwords should include special characters.
        /// </summary>
        private static readonly SettingDefinition<bool> IncludeSymbolsSetting
            = new(
                name: $"{nameof(PasswordGeneratorToolViewModel)}.{nameof(IncludeSymbolsSetting)}",
                isRoaming: true,
                defaultValue: true);

        public Type View { get; } = typeof(PasswordGeneratorToolPage);

        internal PasswordGeneratorStrings Strings => LanguageManager.Instance.PasswordGenerator;

        internal ISettingsProvider SettingsProvider { get; }
        private readonly IMarketingService _marketingService;

        private bool _toolSuccessfullyWorked;

        private const int MaxLength = 100;
        internal int Length
        {
            get => SettingsProvider.GetSetting(LengthSetting);
            set
            {
                if (SettingsProvider.GetSetting(LengthSetting) != value)
                {
                    int valueToSet = value <= MaxLength ? value : MaxLength;
                    SettingsProvider.SetSetting(LengthSetting, valueToSet);
                    OnPropertyChanged();
                }
            }
        }

        private const int MaxNumberOfPasswords = 1000;
        internal int NumberOfPasswordsToGenerate
        {
            get => SettingsProvider.GetSetting(NumberOfPasswordsToGenerateSetting);
            set
            {
                if (SettingsProvider.GetSetting(NumberOfPasswordsToGenerateSetting) != value)
                {
                    int valueToSet = value <= MaxNumberOfPasswords ? value : MaxNumberOfPasswords;
                    SettingsProvider.SetSetting(NumberOfPasswordsToGenerateSetting, valueToSet);
                    OnPropertyChanged();
                }
            }
        }

        internal bool IncludeLowerCase
        {
            get => SettingsProvider.GetSetting(IncludeLowerCaseSetting);
            set 
            {
                if (SettingsProvider.GetSetting(IncludeLowerCaseSetting) != value)
                {
                    SettingsProvider.SetSetting(IncludeLowerCaseSetting, value);
                    OnPropertyChanged();
                }
            }
        }

        internal bool IncludeUpperCase
        {
            get => SettingsProvider.GetSetting(IncludeUpperCaseSetting);
            set
            {
                if (SettingsProvider.GetSetting(IncludeUpperCaseSetting) != value)
                {
                    SettingsProvider.SetSetting(IncludeUpperCaseSetting, value);
                    OnPropertyChanged();
                }
            }
        }

        internal bool IncludeNumbers
        {
            get => SettingsProvider.GetSetting(IncludeNumbersSetting);
            set
            {
                if (SettingsProvider.GetSetting(IncludeNumbersSetting) != value)
                {
                    SettingsProvider.SetSetting(IncludeNumbersSetting, value);
                    OnPropertyChanged();
                }
            }
        }

        internal bool IncludeSpecialCharacters
        {
            get => SettingsProvider.GetSetting(IncludeSymbolsSetting);
            set
            {
                if (SettingsProvider.GetSetting(IncludeSymbolsSetting) != value)
                {
                    SettingsProvider.SetSetting(IncludeSymbolsSetting, value);
                    OnPropertyChanged();
                }
            }
        }

        [ImportingConstructor]
        public PasswordGeneratorToolViewModel(ISettingsProvider settingsProvider, IMarketingService marketingService)
        {
            SettingsProvider = settingsProvider;
            _marketingService = marketingService;

            GenerateCommand = new RelayCommand(ExecuteGenerateCommand);
        }

        internal IRelayCommand GenerateCommand { get; }

        private void ExecuteGenerateCommand()
        {
            List<string> passwords = RandomStringGenerator.Generate(Length, NumberOfPasswordsToGenerate, IncludeLowerCase, IncludeUpperCase, IncludeNumbers, IncludeSpecialCharacters);

            Output = string.Join(Environment.NewLine, passwords);
            OutputTextBox?.ScrollToBottom();

            if (!_toolSuccessfullyWorked)
            {
                _toolSuccessfullyWorked = true;
                _marketingService.NotifyToolSuccessfullyWorked();
            }
        }

        private string _output = string.Empty;
        internal string Output
        {
            get => _output;
            set => SetProperty(ref _output, value);
        }

        internal ICustomTextBox? OutputTextBox { private get; set; }
    }
}
