using System;
using System.Collections.Generic;
using System.Text;

namespace Game
{
    public enum LanguageText
    {
        HELLO,
        YES,
        NO,
    }

    public sealed class Language
    {
        public Language()
        {
            Reload();
            _current = _text["en"];
        }

        public string Format(LanguageText value)
        {
            return GetText(value);
        }

        public string Format(LanguageText value, params object[] args)
        {
            return String.Format(GetText(value), args);
        }

        public void Reload()
        {
        }

        public IEnumerable<string> AvailableLanguages
        {
            get
            {
                foreach (var language in _text)
                {
                    yield return language.Key;
                }
            }
        }

        string GetText(LanguageText value)
        {
            if (_current.ContainsKey(value))
            {
                return _current[value];
            }
            return value.ToString();
        }

        Dictionary<string, Dictionary<LanguageText, string> > _text = new Dictionary<string, Dictionary<LanguageText, string> >();
        Dictionary<LanguageText, string> _current;
    }
}
