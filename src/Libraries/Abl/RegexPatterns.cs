namespace Abl
{
    public static class RegexPatterns
    {
        public const string PlainStringPattern = @"^[ \w\-]+$";
        public const string ObjectReferenceStringPattern = @"^[\w\-]{1,50}$";
        public const string SafeCharacterPattern = @"^[ \w\-\$\£\#]+$";
        public const string SafeHtmlCharacterPattern = @"^[^""\'\`&<>!@$%\(\)\=\+\{\}\[\]]+$";
        public const string EmailPattern = @"^\S+@(([\w-]+\.[\w-]+)(\.[\w-]+)*)$";
        public const string EmailListPattern = @"^((\S+@(([\w-]+\.[\w-]+)(\.[\w-]+)*));?)+$";
        public const string SafeUrlRouteFragmentPattern = @"^[a-z](-?[a-z0-9]+)*$";
    }
}