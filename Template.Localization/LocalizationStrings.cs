namespace Template.Localization
{
    public abstract class LocalizationStrings
    {
        public abstract string PublishDate { get; }
        public abstract string ErrorUserNotExist { get; }
        public virtual string AuthorId { get; } = "Author Id";
        public virtual string ErrorWrongUserNamePassword { get; } = "User/password combination is wrong";
    }

}